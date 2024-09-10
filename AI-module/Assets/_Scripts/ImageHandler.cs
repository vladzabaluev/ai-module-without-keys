using NeuralNetworksContenet;
using static CsvHandler;
using static CsvDataTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageHandler : MonoBehaviour
{
    private List<CsvDataTypes.TranslatedLogicImage> translatedLogicImages = new List<CsvDataTypes.TranslatedLogicImage>();

    private void Start()
    {
    }

    private void ReuseImage(string imageName, DataTypes.LogicImageCategoriesEnumeration startCategory, string startValue, DataTypes.LogicImageCategoriesEnumeration newCategory, string newValue)
    {
        TranslatedLogicImage image = translatedLogicImages.FirstOrDefault(image => image.ImageName.Trim() == imageName.Trim() && image.Category == startCategory && image.ParameterValue == startValue);
        if (image != null)

        {
            image.ChangeParameterValue(newCategory, newValue);
            CsvHandler.WriteTranslatedLogicImages(image, DataTypes.ImageValidation.Correct, PathStore.LogicFolder + "ReusedImages");
        }
    }
}