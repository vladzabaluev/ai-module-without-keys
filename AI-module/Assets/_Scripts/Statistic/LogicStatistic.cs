using NeuralNetworksContenet;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CsvDataTypes;
using TMPro;
using System.IO;
using OpenAI;

public class LogicStatistic : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> textSections = new List<TMP_Text>();

    // Start is called before the first frame update
    private void Start()
    {
        PrintLogicStatistics("CurrentRes");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void PrintLogicStatistics(string fileName)
    {
        List<TranslatedLogicImage> logicImages = CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + fileName).Keys.ToList();
        int i = 0;
        foreach (var category in DataTypes.LogicCategoriesArray)
        {
            List<TranslatedLogicImage> temp = logicImages.Where(x => x.Category == category).ToList();
            if (category != DataTypes.LogicImageCategoriesEnumeration.Profession)
            {
                textSections[i].text = "";
                textSections[i].text += $"{category} {temp.Count}\n";
                var types = temp.GroupBy(x => x.ParameterValue).Select(g => new { Type = g.Key, Count = g.Count() });
                foreach (var typeCount in types)
                {
                    textSections[i].text += ($"{typeCount.Type}: {typeCount.Count}\n");
                    var countObject = temp.Count(x => x.ParameterValue == typeCount.Type && x.ImageType == DataTypes.ImageType.Object);
                    textSections[i].text += $"OB - {countObject}, ";

                    // Count of images with ImageType = Background
                    var countBackground = temp.Count(x => x.ParameterValue == typeCount.Type && x.ImageType == DataTypes.ImageType.Background);
                    textSections[i].text += $"BG - {countBackground}\n";
                }
                i++;
            }
            else
            {
                Debug.Log("Professions" + temp.Count);
                var types = temp.GroupBy(x => x.ParameterValue).Select(g => new { Type = g.Key, Count = g.Count() });
                string path = Application.dataPath + $"/Resources/CSV/ProfessionsStat.txt";

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    foreach (var typeCount in types)
                    {
                        // Count of images with ImageType = Object
                        var countObject = temp.Count(x => x.ParameterValue == typeCount.Type && x.ImageType == "Object");

                        // Count of images with ImageType = Background
                        var countBackground = temp.Count(x => x.ParameterValue == typeCount.Type && x.ImageType == "Background");
                        writer.WriteLine($"{typeCount.Type} , OB - {countObject}, BG - {countBackground}");
                        writer.WriteLine();
                    }
                }
                //foreach (var typeCount in types)
                //{
                //    var countObject = temp.Count(x => x.ParameterValue == typeCount.Type && x.ImageType == "Object");

                //    // Count of images with ImageType = Background
                //    var countBackground = temp.Count(x => x.ParameterValue == typeCount.Type && x.ImageType == "Background");
                //    Debug.Log($"{typeCount.Type}: {typeCount.Count}, OB - {countObject}, BG - {countBackground}");

                //}
            }
        }
    }
}