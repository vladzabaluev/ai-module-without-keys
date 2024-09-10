using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static CsvDataTypes;

public class StringFileHandler
{
    public static List<string> ReadStrings(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(path);

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources/{path}");
            return null;
        }
        List<string> data = new List<string>();
        try
        {
            string[] lines = csvFile.text.Split('\n');

            foreach (var line in lines)
            {
                if (line != null && line != " " && line != "")
                {
                    data.Add(line);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from CSV: {e.Message}");
        }

        return data;
    }

    public static void WriteString(string content, string path, string fileFormat)
    {
        string filePath = Application.dataPath + $"/Resources/CSV/{path}.{fileFormat}";

        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            File.AppendAllText(path, string.Join(",", content));

            Debug.Log("CSV файл успешно записан: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка записи в файл: " + e.Message);
        }
    }
}