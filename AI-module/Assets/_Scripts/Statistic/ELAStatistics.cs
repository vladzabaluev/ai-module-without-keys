using NeuralNetworksContenet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static CsvDataTypes;

public class ELAStatistics : MonoBehaviour
{
    [SerializeField] private TMP_Text WordsTypesText;
    [SerializeField] private TMP_Text SymbolsText;
    [SerializeField] private TMP_Text ObjectByInterestText;
    [SerializeField] private TMP_Text BackgroundsText;

    private List<TranslatedELAImage> translatedELAImages = new List<TranslatedELAImage>();

    // Start is called before the first frame update
    private void Start()
    {
        translatedELAImages = CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "Finally").Keys
            .Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1).ToList();
        //translatedELAImages.AddRange(CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "ELA_2").Keys
        //    .Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1));
        var wordGroups = translatedELAImages.Where(x => x.WordType.Length > 1).GroupBy(x => x.WordType)
            .Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var wordGroup in wordGroups)
        {
            WordsTypesText.text += wordGroup.Type + ": " + wordGroup.Count + "\n";
        }

        SymbolsText.text += "����� ������������ � ��������\n";
        var letters = translatedELAImages.Where(x => x.WordType == DataTypes.WordType.WordsStartingWithLetters.ToString()).GroupBy(x => x.StartSymbol).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var lettersCount in letters)
        {
            SymbolsText.text += ($"������ {lettersCount.Type} : {lettersCount.Count}\n");
        }

        ObjectByInterestText.text += "������� �� ���������\n";

        var objectImages = translatedELAImages.Where(x => x.WordType.Length < 2 && x.ImageType == DataTypes.ImageType.Object).GroupBy(x => x.Interest).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var objectsCount in objectImages)
        {
            ObjectByInterestText.text += ($"�������: {objectsCount.Type} - {objectsCount.Count}\n");
        }

        BackgroundsText.text += "������ ���� �� ���������\n";
        var backgroundImages = translatedELAImages.Where(x => x.WordType.Length < 2 && x.ImageType == DataTypes.ImageType.Background).GroupBy(x => x.Interest).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var backgroundCount in backgroundImages)
        {
            BackgroundsText.text += ($"�������: {backgroundCount.Type} - {backgroundCount.Count}\n");
        }
        Debug.Log(translatedELAImages.Count);
        //foreach(var image in translatedELAImages)
        //{
        //    CsvHandler.WriteTranslatedELAmages(image,)
        //}
    }

    private void PrintELAStatistics()
    {
        List<TranslatedELAImage> elaImages = CsvHandler.ReadTranslatedImagesELA("Reviewed/ELAImages").Where(x => x.ImageName.Length > 5).ToList();
        WordsTypesText.text = $"����� �������� {elaImages.Count}\n";
        elaImages = elaImages.Where(x => x.DeffectChar.Length == 0).ToList();
        WordsTypesText.text += $"�������� �������� {elaImages.Count}\n";

        var types = elaImages.GroupBy(x => x.WordType).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var typeCount in types)
        {
            WordsTypesText.text += ($"��� ��������: {typeCount.Type}, ����������: {typeCount.Count}\n");
        }

        List<TranslatedELAImage> startingWithLetters = elaImages.Where(x => x.WordType == DataTypes.WordType.WordsStartingWithLetters.ToString()).ToList();
        WordsTypesText.text += "����� ������������ � ��������\n";
        var letters = startingWithLetters.GroupBy(x => x.StartSymbol).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var lettersCount in letters)
        {
            WordsTypesText.text += ($"������: {lettersCount.Type}, ����������: {lettersCount.Count}\n");
        }

        List<TranslatedELAImage> objects = elaImages.Where(x => x.WordType.Length < 2 && x.ImageType == DataTypes.ImageType.Object).ToList();
        WordsTypesText.text += "������� �� ���������\n";

        var objectImages = objects.GroupBy(x => x.Interest).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var objectsCount in objectImages)
        {
            WordsTypesText.text += ($"�������: {objectsCount.Type}, ����������: {objectsCount.Count}\n");
        }

        List<TranslatedELAImage> backgrounds = elaImages.Where(x => x.WordType.Length < 2 && x.ImageType == DataTypes.ImageType.Background).ToList();
        WordsTypesText.text += "������ ���� �� ���������\n";
        var backgroundImages = backgrounds.GroupBy(x => x.Interest).Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var backgroundCount in backgroundImages)
        {
            WordsTypesText.text += ($"�������: {backgroundCount.Type}, ����������: {backgroundCount.Count}\n");
        }
    }
}