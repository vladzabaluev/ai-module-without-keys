using NeuralNetworksContenet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static CsvDataTypes;

public class GradeController : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        #region ELA

        //Dictionary<TranslatedELAImage, DataTypes.ImageValidation> elaImages = new Dictionary<TranslatedELAImage, DataTypes.ImageValidation>();
        //elaImages.AddRange(CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "Correct"));
        //elaImages.AddRange(CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "ELA_2"));

        //foreach (var image in elaImages)
        //{
        //    if (image.Value == DataTypes.ImageValidation.Correct)
        //    {
        //        CsvHandler.WriteTranslatedELAImage(image.Key, image.Value, PathStore.ELAFolder + "Finally");
        //    }
        //    else
        //    {
        //        CsvHandler.WriteTranslatedELAImage(image.Key, image.Value, PathStore.ELAFolder + "ToRegenerate");
        //    }
        //}

        #endregion ELA

        #region Шаблон того, как заполнить CSV файл с пропусками под слова

        //Dictionary<TranslatedLogicImage, DataTypes.ImageValidation> logicImages = new Dictionary<TranslatedLogicImage, DataTypes.ImageValidation>();
        //logicImages.AddRange(CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "Correct"));
        //var ProfessionLogicImages = logicImages.Keys.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Profession).ToList();
        //var uniqueProfession = ProfessionLogicImages.GroupBy(image => image.ParameterValue).Select(x => x.Key).ToList();
        //foreach (var profession in uniqueProfession)
        //{
        //    var professionImages = ProfessionLogicImages.Where(x => x.ParameterValue == profession);
        //    foreach (var image in professionImages)
        //    {
        //        CsvHandler.WriteTranslatedLogicImages(image, DataTypes.ImageValidation.Correct, PathStore.LogicFolder + "Professions");
        //    }
        //    var backgrounds = professionImages.Where(x => x.ImageType == DataTypes.ImageType.Background);
        //    if (backgrounds.Count() == 0)
        //    {
        //        TranslatedLogicImage backGroundLogicImage = new TranslatedLogicImage(DataTypes.ImageType.Background, "",
        //            DataTypes.LogicImageCategoriesEnumeration.Profession, profession, "", "", "");
        //        CsvHandler.WriteTranslatedLogicImages(backGroundLogicImage, DataTypes.ImageValidation.None, PathStore.LogicFolder + "Professions");
        //    }

        //    var objects = professionImages.Where(x => x.ImageType == DataTypes.ImageType.Object);

        //    if (objects.Count() < 5)
        //    {
        //        for (int i = 0; i < 5 - objects.Count(); i++)
        //        {
        //            TranslatedLogicImage objectLogicImage = new TranslatedLogicImage(DataTypes.ImageType.Object, "",
        //          DataTypes.LogicImageCategoriesEnumeration.Profession, profession, "", "", "");
        //            CsvHandler.WriteTranslatedLogicImages(objectLogicImage, DataTypes.ImageValidation.None, PathStore.LogicFolder + "Professions");
        //        }
        //    }
        //}

        #endregion Шаблон того, как заполнить CSV файл с пропусками под слова

        Dictionary<TranslatedLogicImage, DataTypes.ImageValidation> logicImages = new Dictionary<TranslatedLogicImage, DataTypes.ImageValidation>();
        logicImages.AddRange(CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "ProfReview6"));

        //Dictionary<TranslatedLogicImage, DataTypes.ImageValidation> professionsResult = new Dictionary<TranslatedLogicImage, DataTypes.ImageValidation>();
        //professionsResult.AddRange(CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "ProPrimary"));

        Debug.Log(logicImages.Count);

        MarkAnalize(logicImages);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void MarkAnalize(Dictionary<TranslatedLogicImage, DataTypes.ImageValidation> logicImages)
    {
        foreach (var image in logicImages)
        {
            if (image.Value != DataTypes.ImageValidation.BadWord)
            {
                if (image.Value == DataTypes.ImageValidation.Correct)
                {
                    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, "CurrentRes");
                }
                //else if (image.Key.ImageType == DataTypes.ImageType.Background)
                //{
                //    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, PathStore.LogicFolder + "Backgrounds");
                //}
                else
                {
                    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, "ToRegenerate 1");
                }
                //if (image.Value == DataTypes.ImageValidation.Correct)
                //{
                //    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, PathStore.LogicFolder + "TolyaReview");
                //}
                ////else if (image.Key.ImageType == DataTypes.ImageType.Background)
                ////{
                ////    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, PathStore.LogicFolder + "Backgrounds");
                ////}
                //else
                //{
                //    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, PathStore.LogicFolder + "ProfRegen3");
                //}
            }
        }
    }

    public static List<TranslatedLogicImage> ReturnNotReviewdLogicImages()
    {
        Dictionary<TranslatedLogicImage, DataTypes.ImageValidation> logicImagesResult = CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "Logic").Where(x => x.Key.ImageName.Length > 5 && x.Key.DeffectChar.Length == 0).ToDictionary(x => x.Key, x => x.Value);
        Debug.Log("Ключей в коллекции " + logicImagesResult.Count);
        List<TranslatedLogicImage> viewedImages = logicImagesResult.Keys.ToList();

        var duplicateImageNames = viewedImages.GroupBy(image => image.ImageName)
                                           .Where(group => group.Count() > 1)
                                           .Select(group => group.Key);
        var matchingElements = logicImagesResult
            .Where(pair => duplicateImageNames.Contains(pair.Key.ImageName))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var matchingElement in matchingElements)
        {
            logicImagesResult.Remove(matchingElement.Key);
        }
        viewedImages = logicImagesResult.Keys.ToList();
        List<TranslatedLogicImage> allLogicImages = CsvHandler.ReadTranslatedLogicImagesWithDeffectChair(PathStore.ReviewedLogicPath).Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length == 0).ToList();
        Debug.Log("Всего картинок " + allLogicImages.Count);
        Debug.Log("Уникальных имен точно " + allLogicImages.GroupBy(image => image.ImageName).Count());
        List<TranslatedLogicImage> duplicatedImages = new List<TranslatedLogicImage>();
        foreach (TranslatedLogicImage logicImage in allLogicImages)
        {
            foreach (TranslatedLogicImage temp in viewedImages)
            {
                if (logicImage.ImageName.Trim() == temp.ImageName.Trim())
                {
                    duplicatedImages.Add(logicImage);
                }
            }
        }
        Debug.Log("Найдено дубляжей " + duplicatedImages.Count);

        List<TranslatedLogicImage> notReviewedImages = allLogicImages.Except(duplicatedImages).ToList();
        Debug.Log("Уникальных картинок " + notReviewedImages.ToList().Count);

        return notReviewedImages;
    }

    public static void CheckListOnDuplicates(IEnumerable<TranslatedLogicImage> logicImages)
    {
        var comparer = new TranslatedLogicImageComparer();

        // Получаем уникальные объекты без дубликатов
        Debug.Log($"Всего изображений: {logicImages.Count()}");

        var uniqueImages = logicImages.Distinct(comparer).ToList();
        foreach (var image in uniqueImages)
        {
            CsvHandler.WriteTranslatedLogicImages(image, DataTypes.ImageValidation.Correct, "Correct");
        }
    }

    public class TranslatedLogicImageComparer : IEqualityComparer<TranslatedLogicImage>
    {
        public bool Equals(TranslatedLogicImage x, TranslatedLogicImage y)
        {
            return x.ImageName == y.ImageName &&
                   x.Category == y.Category &&
                   x.ParameterValue == y.ParameterValue;
        }

        public int GetHashCode(TranslatedLogicImage obj)
        {
            // Генерируем хэш-код на основе параметров ImageName, Category и ParameterValue
            return HashCode.Combine(obj.ImageName, obj.Category, obj.ParameterValue);
        }
    }
}