using System;
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class LOM_Export_Tests : BaseTest
{
    [Test]
    public void Should_export_LOM()
    {
        var category = ContextCategory.New().Add("Example category").Persist().All.First();
        var lomXmlCategory = category.ToLomXml();

        Console.Write(lomXmlCategory);

        var question = ContextQuestion.New().AddQuestion("Example question").AddCategory("cat 1").Persist().All.First();
        var lomXmlQuestion = question.ToLomXml();

        Console.Write(lomXmlQuestion);

    }
}

