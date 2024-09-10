using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextHandler
{
    public static string GetSample(string variants)
    {
        string[] samples = variants.Split('\n');

        int randomIndex = Random.Range(0, samples.Length);
        samples[randomIndex] = Regex.Replace(samples[randomIndex], "[^a-zA-Z\\s-]", "");

        Debug.Log($"Ёкземпл€р: {samples[randomIndex]}");
        return samples[randomIndex];
    }

    public static string GetCleanString(string words)
    {
        return Regex.Replace(words, "[^a-zA-Z\\s-]", "");
    }

    public static string GetStringFromList(List<string> text)
    {
        string words = "";
        if (text.Count > 0)
        {
            foreach (string word in text)
            {
                words += $"{word}, ";
            }
            words = words.Trim().Remove(words.Length - 2);
        }
        return words;
    }

    public static List<string> GetListFromString(string text)
    {
        text.Trim();
        string[] wordsArray = text.Split('\n');

        // —оздаем список и добавл€ем в него слова
        List<string> wordList = new List<string>();
        foreach (string word in wordsArray)
        {
            // ”бираем лишние пробелы
            if (word != "")
            {
                string trimmedWord = word.Trim();
                wordList.Add(trimmedWord);
            }
        }

        // ¬озвращаем список слов
        return wordList;
    }
}