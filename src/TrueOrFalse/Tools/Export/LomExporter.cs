using System;
using System.IO;

public class LomExporter
{
    public static void AllToFileSystem()
    {
        lock ("c6fc2ccc-bf87-4b6f-b286-d2438e117cc1")
        {
            AllToFileSystem_();
        }    
    }

    private static void AllToFileSystem_()
    {
        var exportPath = Settings.LomExportPath;

        if (!Directory.Exists(exportPath))
            throw new Exception("directory does not exist");

        foreach (FileInfo file in new DirectoryInfo(exportPath).GetFiles())
            file.Delete();

        ExportCategories(exportPath);
        ExportQuestions(exportPath);
    }

    private static void ExportCategories(string exportPath)
    {
        var listOfSelectedCategories = new[] {84, 724, 725, 841, 264, 265, 285, 606, 615, 618, 619, 607, 635, 644};
        var categories = Sl.CategoryRepo.GetByIds(listOfSelectedCategories);

        foreach (var category in categories)
        {
            if (!category.Creator.IsMemuchoUser && !category.Creator.IsBeltz)
                continue;

            try
            {
                File.WriteAllText(Path.Combine(exportPath, $"topic-{category.Id}.xml"), LomXml.From(category));
            }
            catch (Exception e)
            {
                Logg.Error(new Exception($"Error exporting set {category.Id}", e));
            }
        }        
    }

    private static void ExportQuestions(string exportPath)
    {
        var allQuestions = Sl.QuestionRepo.GetAll();

        foreach (var question in allQuestions)
        {
            if(!question.Creator.IsMemuchoUser && !question.Creator.IsBeltz)
                continue;

            try
            {
                File.WriteAllText(Path.Combine(exportPath, $"question-{question.Id}.xml"), LomXml.From(question));
            }
            catch (Exception e)
            {
                Logg.Error(new Exception($"Error exporting question {question.Id}", e));
            }
        }
    }
}

