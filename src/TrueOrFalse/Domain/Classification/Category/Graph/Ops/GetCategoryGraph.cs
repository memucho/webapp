using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphJsonDtos;

public class GetCategoryGraph
{
    public static JsonResult AsJson(Category category)
    {
        var graphData = Get(category);

        var links = new List<Link>();
        foreach (var link in graphData.Links)
        {
            var parentIndex = graphData.Nodes.FindIndex(node => node.Category == link.Parent);
            var childIndex = graphData.Nodes.FindIndex(node => node.Category == link.Child);
            links.Add(new Link { source = parentIndex, target = childIndex });
        }

        var nodes = graphData.Nodes.Select(node => 
            new Node
            {
                CategoryId = node.Category.Id,
                Text = node.Category.Name
            });

        return new JsonResult
        {
            Data = new
            {
                Nodes = nodes,
                Links = links
            }
        };
    }

    public static CategoryGraph Get(Category category)
    {
        var descendants = GetCategoriesDescendants.WithAppliedRules(category);

        var nodes = new List<CategoryNode>{new CategoryNode{Category = category}};

        foreach (var descendant in descendants)
            nodes.Add(new CategoryNode {Category = descendant});

        var links = new List<CategoryLink>();
        foreach (var categoryNode in nodes)
        {
            var categoryNodeLinks = GetLinksFromCategory(
                categoryNode.Category,
                Sl.CategoryRepo.GetChildren(categoryNode.Category.Id).ToList()
            );

            links.AddRange(categoryNodeLinks);
        }
        
        return new CategoryGraph
        {
            Nodes = nodes,
            Links = links
        };
    }

    private static IEnumerable<CategoryLink> GetLinksFromCategory(Category parent, List<Category> children) => 
        children.Select(child => new CategoryLink{
            Parent = parent,
            Child = child
        });
}