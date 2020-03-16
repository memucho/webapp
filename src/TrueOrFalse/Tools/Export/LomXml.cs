using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class LomXml
{
    private static XAttribute _langAttributeDe => new XAttribute("language", "de");
    private static XElement _sourceElementLom => new XElement("source", "LOMv1.0");
    private static XElement _sourceElementLre => new XElement("source", "LREv3.0");
    private static readonly XCData _vCardCData = new XCData("BEGIN:vCard VERSION:3.0 FN:memucho.de N:memucho.de END:vcard");

    public static string From(Category category)
    {
        return From(new LomXmlParams(category));
    }

    public static string From(Question question)
    {
        return From(new LomXmlParams(question));
    }

    public static string From(LomXmlParams objectParams)
    {
        var xDoc = new XDocument(
            new XElement("lom",
                GetGeneral(objectParams),
                GetLifecycle(objectParams),
                GetMetaMetadata(objectParams),
                GetTechnical(objectParams),
                GetEducational(),
                GetRights(objectParams)
            )
        );

        return xDoc.ToString();
    }

    private static XElement GetGeneral(LomXmlParams p)
    {
        return new XElement("general",
            new XElement("identifier",
                new XElement("catalog", "memucho"),
                new XElement("entry", p.GeneralIdentifier)
            ),
            new XElement("title",
                new XElement("string", p.GeneralTitle.Truncate(1000), _langAttributeDe)
            ),
            new XElement("language", "de"),
            new XElement("description",
                new XElement("string", p.GeneralDescription.Truncate(2000), _langAttributeDe)
            ),
            p.Categories.Select(c =>
                new XElement("keyword",
                    new XElement("string", c.Name, _langAttributeDe)
                )
            ).ToList(),
            new XElement("structure",
                _sourceElementLom,
                new XElement("value", LomGeneralStructure.Atomic.GetValue())
            ),
            new XElement("aggregationLevel",
                _sourceElementLom,
                new XElement("value", p.AggregationLevel)
            )
        );
    }

    private static XElement GetLifecycle(LomXmlParams p)
    {
        return new XElement("lifeCycle",
            new XElement("version",
                new XElement("string", "1.0", _langAttributeDe)
            ),
            new XElement("status",
                _sourceElementLom,
                new XElement("value", LomLifecycleStatus.Final.GetValue())
            ),
            new XElement("contribute",
                new XElement("role",
                    _sourceElementLom,
                    new XElement("value", LomLifecycleRole.Author.GetValue())
                ),
                new XElement("entity", _vCardCData),
                new XElement("date",
                    new XElement("dateTime", p.LifecycleDate)
                )
            )
        );
    }

    private static XElement GetMetaMetadata(LomXmlParams p)
    {
        return new XElement("metaMetadata",
            new XElement("identifier",
                new XElement("catalog", "memucho"),
                new XElement("entry", p.MetaMetaCatalogEntry)
            ),
            new XElement("contribute", 
                new XElement("role",
                    _sourceElementLre,
                    new XElement("value", LomMetaMetadataRoleLre.Creator.GetValue())
                ),
                new XElement("entity", _vCardCData),
                new XElement("date", DateTime.Now)
            )
        );
    }

    private static XElement GetTechnical(LomXmlParams p)
    {
        return new XElement("technical",
            new XElement("format", "text/html"),
            new XElement("location", p.TechnicalLocation)
        );
    }

    private static XElement GetEducational()
    {
        return new XElement("educational",
            new XElement("learningResourceType", "web page"),
            new XElement("context",
                _sourceElementLre,
                new XElement("value", LomEducationalContext.CompulsoryEducation)),
            new XElement("typicalAgeRange", "0-99")
        );
    }

    private static XElement GetRights(LomXmlParams p)
    {
        return new XElement("rights",
            new XElement("copyrightAndOtherRestrictions",
                _sourceElementLom,
                new XElement("value", "yes")
            ),
            new XElement("description",
                new XElement("string", p.RightsDescription, _langAttributeDe))
        );
    }

    //private static XElement GetClassification()
    //{
    //    return new XElement();
    //}
}

public class LomXmlParams
{
    public string GeneralIdentifier;
    public string GeneralTitle;
    public string GeneralDescription;
    public IList<Category> Categories;
    public int AggregationLevel;
    public DateTime LifecycleDate;
    public string MetaMetaCatalogEntry;
    public string TechnicalLocation;
    public string RightsDescription;


    public LomXmlParams(Category category)
    {
        GeneralIdentifier = "thema-" + category.Id;
        GeneralTitle = category.Name;
        GeneralDescription = category.Description;
        Categories = category.ParentCategories();
        AggregationLevel = LomAggregationLevel.Level3Course.GetValue();
        LifecycleDate = category.DateCreated;
        MetaMetaCatalogEntry = "metadata.memucho-thema-" + category.Id;
        TechnicalLocation = "https://memucho.de" + Links.CategoryDetail(category);
        RightsDescription = "CC BY, Autor: " + category.Creator.Name + " (Nutzer auf memucho.de)";
    }

    public LomXmlParams(Question question)
    {
        GeneralIdentifier = "frage-" + question.Id;
        GeneralTitle = question.Text;
        GeneralDescription = "Lernfrage \"" + question.Text + "\" mit Antwortmöglichkeit";
        Categories = question.Categories;
        AggregationLevel = LomAggregationLevel.Level1Fragment.GetValue();
        LifecycleDate = question.DateCreated;
        MetaMetaCatalogEntry = "metadata.memucho-frage-" + question.Id;
        TechnicalLocation = "https://memucho.de" + Links.AnswerQuestion(question);
        RightsDescription = "CC BY, Autor: " + question.Creator.Name + " (Nutzer auf memucho.de)";
    }
}