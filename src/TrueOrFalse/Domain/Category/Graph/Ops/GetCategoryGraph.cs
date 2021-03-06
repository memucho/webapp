﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphJsonDtos;

public class GetCategoryGraph
{
    public static JsonResult AsJson(CategoryCacheItem category)
    {
        var graphData = Get(category);

        var links = GetLinks(graphData);
        var nodes = GetNodes(graphData);

        AssignNodeLevels(nodes, links);
        AssignLinkLevels(nodes, links);

        return new JsonResult
        {
            Data = new
            {
                nodes = nodes,
                links = links
            }
        };
    }

    private static List<Node> GetNodes(CategoryGraph graphData)
    {
        var nodes = graphData.nodes.Select((node, index) =>
            new Node
            {
                Id = index,
                CategoryId = node.CategoryCacheItem.Id,
                Title = (node.CategoryCacheItem.Name).Replace("\"", ""),
                Knowledge = GetKnowledgeData(node.CategoryCacheItem.Id, Sl.SessionUser.UserId),
            }).ToList();
        return nodes;
    }

    private static KnowledgeSummary GetKnowledgeData(int categoryId, int userId)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);
        return KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, userId);
    }

    private static List<Link> GetLinks(CategoryGraph graphData)
    {
        var links = new List<Link>();
        foreach (var link in graphData.links)
        {
            var parentIndex = graphData.nodes.FindIndex(node => node.CategoryCacheItem == link.Parent);
            var childIndex = graphData.nodes.FindIndex(node => node.CategoryCacheItem == link.Child);
            if (childIndex >= 0 && parentIndex >= 0)
                links.Add(new Link
                {
                    source = parentIndex,
                    target = childIndex,
                });
        }

        return links;
    }

    private static void AssignNodeLevels(IList<Node> nodes, List<Link> links)
    {
        var maxLevels = 10;
        var nodesDictionary = nodes.ToDictionary(node => node.Id);
        var rootNode = nodes.First(node => node.Id == 0);

        //initialize all levels to -1
        foreach (var node in nodes) node.Level = -1;

        rootNode.Level = 0;

        for (int currentLevel = 0; currentLevel < maxLevels; currentLevel++)
        {
            foreach(var nodeCurrentLevel in nodes.Where(node => node.Level == currentLevel))
            {
                foreach (var link in links)
                {
                    if (link.source == nodeCurrentLevel.Id)
                    {
                        var targetNode = nodesDictionary[link.target];
                        if (targetNode.Level == -1)
                            targetNode.Level = currentLevel + 1;
                    }
                }
            }
        }
    }

    private static void AssignLinkLevels(IList<Node> nodes, List<Link> links)
    {
        var nodesDictionary = nodes.ToDictionary(node => node.Id);

        foreach (var link in links) link.level = -1;

        foreach (var link in links)
        {
            var sourceNode = nodesDictionary[link.source];
            var targetNode = nodesDictionary[link.target];
            if (sourceNode.Level != -1 && targetNode.Level != -1)
                link.level = Math.Max(sourceNode.Level, targetNode.Level);
        }
    }

    public static void Test_AssignLinkLevels(IList<Node> nodes, List<Link> links) => AssignLinkLevels(nodes, links);

    public static CategoryGraph Get(CategoryCacheItem category)
    {
        var descendants = GetCategoryChildren.WithAppliedRules(category);

        var nodes = new List<CategoryNode>{new CategoryNode{CategoryCacheItem = category}};

        foreach (var descendant in descendants)
            nodes.Add(new CategoryNode {CategoryCacheItem = descendant});

        var links = new List<CategoryLink>();
        foreach (var categoryNode in nodes)
        {
            var categoryNodeLinks = GetLinksFromCategory(
                categoryNode.CategoryCacheItem,CategoryCacheItem.ToCacheCategories(
                Sl.CategoryRepo.GetChildren(categoryNode.CategoryCacheItem.Id)).ToList()
            );

            links.AddRange(categoryNodeLinks);
        }
        
        return new CategoryGraph
        {
            nodes = nodes,
            links = links
        };
    }

    private static IEnumerable<CategoryLink> GetLinksFromCategory(CategoryCacheItem parent, List<CategoryCacheItem> children) => 
        children.Select(child => new CategoryLink{
            Parent = parent,
            Child = child
        });
}