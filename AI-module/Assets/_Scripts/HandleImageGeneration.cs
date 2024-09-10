using NeuralNetworksContenet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextGenerator = NeuralNetworksContenet.TextGenerator;

public class HandleImageGeneration : MonoBehaviour
{
    public static HandleImageGeneration Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!ContentGenerator.Instance.IsHandleMode)
        {
            Instance.enabled = false;
        }
        else
        {
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public async void ConfirmImage(CsvDataTypes.TranslatedLogicImage imageLogic, string fileName)
    {
        List<string> parameters = new List<string>();
        if (imageLogic.Category == DataTypes.LogicImageCategoriesEnumeration.Color || imageLogic.Category == DataTypes.LogicImageCategoriesEnumeration.Shape)
        {
            parameters.Add(imageLogic.ParameterValue);
        }
        string visualDescription = await TextGenerator.GetShortDescriptionOf(imageLogic.Word, parameters);
        DataTypes.ImageInformation generatedImageInformation = await ImageGenerator.GetObjectImage(visualDescription);
        imageLogic.ImageName = generatedImageInformation.Name;
        CsvHandler.WriteTranslatedLogicImages(imageLogic, DataTypes.ImageValidation.None, fileName);
    }
}