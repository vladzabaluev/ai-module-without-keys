using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.VisualBasic;
using System.Globalization;
using Unity.VisualScripting;
using System.Linq;
using static CsvDataTypes;
using static NeuralNetworksContenet.DataTypes;
using NeuralNetworksContenet;

public class CsvHandler
{
    private static string folderPath;
    public const string ProfessionFolderPath = "Professions/";
    public const string LogicImagesFolderPath = "Logic/";
    public const string ELAImagesFolderPath = "ELA/";

    public static List<ELAImage> ReadImagesELAFromCsvFile()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/ELAImages");

        if (csvFile == null)
        {
            Debug.LogError("CSV file not found in Resources/CSV/ELAImages");
            return null;
        }
        List<ELAImage> data = new List<ELAImage>();
        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    ELAImage image = new ELAImage(row[0], row[1], row[2], row[4], row[5], row[8]);
                    data.Add(image);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}" + $"{e.Source}");
        }

        return data;
    }

    public static void WriteImageELA(string word, string wordType, string interest, string startSymbol, string imageType, string imageName)
    {
        folderPath = Application.dataPath + "/Resources/CSV/ELAImages.csv";

        try
        {
            ELAImage image = new ELAImage(word, wordType, interest, startSymbol, imageType, imageName);

            using (StreamWriter writer = new StreamWriter(folderPath, true))
            {
                writer.WriteLine(string.Join(",", image.Word, image.WordType, image.Interest, image.StartSymbol, image.ImageType,
                     image.ImageName.Trim()));
            }

            Debug.Log("CSV файл успешно записан: " + folderPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка записи в файл: " + e.Message);
        }
    }

    public static void WriteImageELA(ELAImage ELAImages)
    {
        folderPath = Application.dataPath + "/Resources/CSV/ELA/ELAImages.csv";

        try
        {
            using (StreamWriter writer = new StreamWriter(folderPath, true))
            {
                writer.WriteLine(string.Join(",", ELAImages.Word, ELAImages.WordType, ELAImages.Interest, ELAImages.StartSymbol, ELAImages.ImageType, ELAImages.ImageName.Trim()));
            }

            Debug.Log("CSV файл успешно записан: " + folderPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка записи в файл: " + e.Message);
        }
    }

    public static List<LogicImage> ReadLogicImagesFromCsvFile(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"CSV/{path}");

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/CSV/{path}");
            return null;
        }

        List<LogicImage> data = new List<LogicImage>();

        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    LogicImageCategoriesEnumeration parameterBeingCompared = EnumStringConverter.StringToLogicCategoryConvert(row[2]);

                    LogicImage image = new LogicImage(row[0], row[1], parameterBeingCompared, row[3], row[4]);
                    data.Add(image);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}");
        }

        return data;
    }

    public static void WriteImageLogic(string imageType, string word, LogicImageCategoriesEnumeration parameterBeingCompared, string parameterValue, string imageName)
    {
        folderPath = Application.dataPath + $"/Resources/CSV/{LogicImagesFolderPath + "LogicImages"}.csv";
        try
        {
            LogicImage image = new LogicImage(imageType, word, parameterBeingCompared, parameterValue, imageName);

            using (StreamWriter writer = new StreamWriter(folderPath, true))
            {
                writer.WriteLine(string.Join(",", image.ImageType, image.Word, image.Category, image.ParameterValue.Trim(), image.ImageName.Trim()));
            }
            Debug.Log($"Файл успешно сохранен в : {folderPath}");
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при записи в файл: " + e.Message);
        }
    }

    public static void WriteImageLogic(LogicImage logicImage)
    {
        folderPath = Application.dataPath + $"/Resources/CSV/{LogicImagesFolderPath + "LogicImages"}.csv";

        try
        {
            using (StreamWriter writer = new StreamWriter(folderPath, true))
            {
                writer.WriteLine(string.Join(",", logicImage.ImageType, logicImage.Word, logicImage.Category, logicImage.ParameterValue, logicImage.ImageName.Trim()));
            }

            Debug.Log($"Файл успешно сохранен в : {folderPath}");
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при записи в файл: " + e.Message);
        }
    }

    public static void WriteTranslatedELAmageOnDevice(TranslatedELAImage elaImage, DataTypes.ImageValidation imageValidation)
    {
        string path;
        path = Path.Combine(Application.persistentDataPath, "CSV");
        path = Path.Combine(path, "ELA.csv");
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        string validateImageParametr = imageValidation + "\n";

        File.AppendAllText(path, string.Join(",", elaImage.Word, elaImage.Translation, "", elaImage.WordType,
            elaImage.Interest, elaImage.StartSymbol, elaImage.ImageType, elaImage.ImageName.Trim(), validateImageParametr));
    }

    public static void WriteTranslatedLogicImagesOnDevice(TranslatedLogicImage logicImage, DataTypes.ImageValidation imageValidation)
    {
        string path;
        path = Path.Combine(Application.persistentDataPath, "CSV");
        path = Path.Combine(path, "Logic.csv");
        Debug.Log(path);

        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        string validateImageParametr = imageValidation + "\n";
        File.AppendAllText(path, string.Join(",", logicImage.ImageType, logicImage.Word, logicImage.Translation, "", logicImage.Category,
                logicImage.ParameterValue, logicImage.ImageName.Trim(), validateImageParametr));
    }

    public static void WriteTranslatedELAImage(TranslatedELAImage elaImage, DataTypes.ImageValidation imageValidation, string filePath)
    {
        string path = Application.dataPath + $"/Resources/CSV/{filePath}.csv";

        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        string validateImageParametr = imageValidation + "\n";

        File.AppendAllText(path, string.Join(",", elaImage.Word, elaImage.Translation, "", elaImage.WordType,
            elaImage.Interest, elaImage.StartSymbol, elaImage.ImageType, elaImage.ImageName.Trim(), validateImageParametr));
    }

    public static void WriteTranslatedLogicImages(TranslatedLogicImage logicImage, DataTypes.ImageValidation imageValidation, string filePath)
    {
        string path = Application.dataPath + $"/Resources/CSV/Logic/{filePath}.csv";

        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        string validateImageParametr = imageValidation + "\n";
        File.AppendAllText(path, string.Join(",", logicImage.ImageType, logicImage.Word, logicImage.Translation, "", logicImage.Category,
                logicImage.ParameterValue, logicImage.ImageName.Trim(), validateImageParametr));
    }

    public static List<CsvDataTypes.TranslatedLogicImage> ReadTranslatedLogicImages(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"CSV/{path}");

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/CSV/{path}");
            return null;
        }

        List<TranslatedLogicImage> data = new List<TranslatedLogicImage>();

        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    LogicImageCategoriesEnumeration parameterBeingCompared = EnumStringConverter.StringToLogicCategoryConvert(row[3]);

                    TranslatedLogicImage image = new TranslatedLogicImage(row[0], row[1], parameterBeingCompared, row[4], row[5], row[2], "");
                    Debug.Log($"Тип картинки: {image.ImageType} " +
                        $"Слово {image.Word} Категория {image.Category} Значение {image.ParameterValue} Имя картинки {image.ImageName} Перевод {image.Translation}");
                    data.Add(image);
                }
                Debug.Log(line);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}");
        }

        return data;
    }

    public static List<TranslatedELAImage> ReadTranslatedImagesELA(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"CSV/{path}");

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/CSV/{path}");
            return null;
        }
        List<TranslatedELAImage> data = new List<TranslatedELAImage>();
        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    TranslatedELAImage image = new TranslatedELAImage(row[0], row[2], row[3], row[6], row[7], row[10], row[1], row[4]);
                    data.Add(image);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}" + $"{e.Source}");
        }

        return data;
    }

    public static List<CsvDataTypes.TranslatedLogicImage> ReadTranslatedLogicImagesWithDeffectChair(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"CSV/{path}");

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/CSV{path}");
            return null;
        }

        List<TranslatedLogicImage> data = new List<TranslatedLogicImage>();

        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    LogicImageCategoriesEnumeration parameterBeingCompared = EnumStringConverter.StringToLogicCategoryConvert(row[4]);

                    TranslatedLogicImage image = new TranslatedLogicImage(row[0], row[1], parameterBeingCompared, row[5], row[6], row[2], row[3]);
                    data.Add(image);
                }
                //Debug.Log(line);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}");
        }

        return data;
    }

    public static Dictionary<CsvDataTypes.TranslatedLogicImage, DataTypes.ImageValidation> ReadTranslatedReviewedLogicImages(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"CSV/{path}");

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/CSV/{path}");
            return null;
        }

        Dictionary<CsvDataTypes.TranslatedLogicImage, DataTypes.ImageValidation> data = new Dictionary<CsvDataTypes.TranslatedLogicImage, DataTypes.ImageValidation>();

        try
        {
            string[] lines = csvFile.text.Split('\n');
            //Debug.Log(csvFile.text);
            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    LogicImageCategoriesEnumeration parameterBeingCompared = EnumStringConverter.StringToLogicCategoryConvert(row[4]);
                    //Debug.Log(line);

                    TranslatedLogicImage image = new TranslatedLogicImage(row[0], row[1], parameterBeingCompared, row[5], row[6], row[2], row[3]);
                    //Debug.Log($"Тип картинки: {image.ImageType} " +
                    //$"Слово {image.Word} Категория {image.Category} Значение {image.ParameterValue} Имя картинки {image.ImageName} Перевод {image.Translation}");
                    data.Add(image, EnumStringConverter.StringToImageValidation(row[7].Trim()));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}");
        }

        return data;
    }

    public static Dictionary<CsvDataTypes.TranslatedELAImage, DataTypes.ImageValidation> ReadTranslatedReviewedELAImages(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"CSV/{path}");

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/CSV/{path}");
            return null;
        }

        Dictionary<CsvDataTypes.TranslatedELAImage, DataTypes.ImageValidation> data = new Dictionary<CsvDataTypes.TranslatedELAImage, DataTypes.ImageValidation>();

        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                //Debug.Log(line);

                if (line != null && line != " " && line != "")
                {
                    string[] row = line.Split(',');

                    //File.AppendAllText(path, string.Join(",", elaImage.Word, elaImage.Translation, "", elaImage.WordType,
                    //    elaImage.Interest, elaImage.StartSymbol, elaImage.ImageType, elaImage.ImageName.Trim(), validateImageParametr));
                    TranslatedELAImage image = new TranslatedELAImage(row[0], row[3], row[4], row[5], row[6], row[7], row[1], row[2]);

                    data.Add(image, EnumStringConverter.StringToImageValidation(row[8].Trim()));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}");
        }

        return data;
    }
}