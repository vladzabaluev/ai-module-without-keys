using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static NeuralNetworksContenet.DataTypes;
using Random = UnityEngine.Random;

namespace NeuralNetworksContenet
{
    public class NeuralContentAPI : MonoBehaviour
    {
        private static bool useRussianURL;

        [SerializeField] private bool _useRussianURL;

        private static List<CsvDataTypes.TranslatedLogicImage> logicImages = new List<CsvDataTypes.TranslatedLogicImage>();
        private static List<CsvDataTypes.TranslatedELAImage> elaImages = new List<CsvDataTypes.TranslatedELAImage>();

        private void Awake()
        {
            Debug.LogWarning("API ничего не возвращет");
            elaImages = CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "Finally").Keys.Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1).ToList();
            logicImages = CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "CurrentRes").Keys.Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1).ToList();

            //logicImages = CsvHandler.ReadImagesLogicFromCsvFile("Reviewed/LogicImages").Where(x => x.ImageName.Length > 5).ToList();
            //elaImages = CsvHandler.ReadImagesELAFromCsvFile().Where(x => x.ImageName.Length > 5).ToList();

            useRussianURL = _useRussianURL;
        }

        public static async Task<string> GetRewrittenTask(string task, string name, int age)
        {
            return await TextGenerator.GetRewrittenTask(task, name, age);
        }

        public static async Task<string> GetSubtask(string subtask, int age)
        {
            return await TextGenerator.GetSubtask(subtask, age);
        }

        public static string GetBackgroundByInterest(DataTypes.Interest interest)
        {
            List<CsvDataTypes.TranslatedELAImage> images = elaImages.Where(image => image.ImageType == DataTypes.ImageType.Background)
                .Where(image => image.Interest == interest.ToString()).ToList();

            int index = Random.Range(0, images.Count);
            return images[index].ImageName;
        }

        public static string GetObjectImageByInterest(DataTypes.Interest interest)
        {
            List<CsvDataTypes.TranslatedELAImage> images = elaImages.Where(image => image.ImageType == DataTypes.ImageType.Object)
               .Where(image => image.Interest == interest.ToString()).Where(image => image.Word == "").ToList();
            int index = Random.Range(0, images.Count);

            return images[index].ImageName;
        }

        public static string GetObjectImageByWordType(DataTypes.WordType wordType, out string word)
        {
            List<CsvDataTypes.TranslatedELAImage> images = elaImages.Where(image => image.WordType == wordType.ToString())
              .ToList();
            int index = Random.Range(0, images.Count);
            word = images[index].Word;
            return images[index].ImageName;
        }

        public static string GetImageByFirstLetter(string letter, out string word)
        {
            List<CsvDataTypes.TranslatedELAImage> images = elaImages.Where(image => image.StartSymbol.ToUpper() == letter.ToUpper())
                .Where(image => image.WordType == DataTypes.WordType.WordsStartingWithLetters.ToString()).ToList();
            int index = Random.Range(0, images.Count);
            word = images[index].Word;
            return images[index].ImageName;
        }

        public static async Task<AudioClip> GetTextVoiceover(string text)
        {
            return await TextToSpeechConverter.GetTextVoiceover(text, new VoiceSettings());
        }

        public static async Task<Sprite> GetImageByName(string name)
        {
            return await ImageLoader.LoadImageByName(name, useRussianURL);
        }

        public static void GetLogicImageNames(LogicImageCategoriesEnumeration parameterBeingCompared, int correctImagesCount, out List<string> correctImageNames,
            int incorrectImagesCount, out List<string> incorrectImageNames)
        {
            List<string> uniqueValues = logicImages.Where(x => x.Category == parameterBeingCompared).Select(img => img.ParameterValue)
                .Distinct().ToList();

            List<string> comparableValues = GetRandomValuesFromList(uniqueValues, 2);

            correctImageNames = GetRandomValuesFromList(logicImages.Where(x => x.ParameterValue == comparableValues[0] && x.ImageType == ImageType.Object)
                .Select(img => img.ImageName).ToList(), correctImagesCount);
            incorrectImageNames = GetRandomValuesFromList(logicImages.Where(x => x.ParameterValue == comparableValues[1] && x.ImageType == ImageType.Object)
                  .Select(img => img.ImageName).ToList(), incorrectImagesCount);
        }

        public static void GetImageNamesWithBackground(LogicImageCategoriesEnumeration parameterBeingCompared, List<int> imagesByCategoriesCount, out Dictionary<string, List<string>> images, out Dictionary<string, List<string>> backgrounds)
        {
            images = new Dictionary<string, List<string>>();
            backgrounds = new Dictionary<string, List<string>>();
            List<string> uniqueValues = logicImages.Where(x => x.Category == parameterBeingCompared).Select(img => img.ParameterValue)
                .Distinct().ToList();
            List<string> comparableValues = GetRandomValuesFromList(uniqueValues, imagesByCategoriesCount.Count);
            for (int i = 0; i < comparableValues.Count; i++)
            {
                Debug.Log(comparableValues[i]);
                List<string> names = GetRandomValuesFromList(logicImages.Where(x => x.ParameterValue == comparableValues[i] && x.ImageType == ImageType.Object).Select(img => img.ImageName).ToList(), imagesByCategoriesCount[i]);
                images.Add(comparableValues[i], names);
            }
            comparableValues.RemoveAt(comparableValues.Count - 1);
            for (int i = 0; i < comparableValues.Count; i++)
            {
                Debug.Log(comparableValues[i]);
                List<string> names = GetRandomValuesFromList(logicImages.Where(x => x.ParameterValue == comparableValues[i] && x.ImageType == ImageType.Background).Select(img => img.ImageName).ToList(), 1);
                backgrounds.Add(comparableValues[i], names);
            }
        }

        public static async Task<List<Sprite>> GetImagesByNames(List<string> imageNames)
        {
            List<Sprite> images = new List<Sprite>();
            foreach (string imageName in imageNames)
            {
                Sprite sprite = await GetImageByName(imageName);
                images.Add(sprite);
            }
            return images;
        }

        public static void GetImageNameWithCategories(LogicImageCategoriesEnumeration parameterBeingCompared, out string ImageName, out string CorrectValue, out string IncorrectValue)
        {
            List<string> uniqueValues = logicImages.Where(x => x.Category == parameterBeingCompared).Select(img => img.ParameterValue)
    .Distinct().ToList();

            List<string> comparableValues = GetRandomValuesFromList(uniqueValues, 2);
            CorrectValue = comparableValues[0];
            IncorrectValue = comparableValues[1];

            ImageName = GetRandomValuesFromList(logicImages.Where(x => x.ParameterValue == comparableValues[0]).Select(x => x.ImageName).ToList(), 1)[0];
        }

        private static List<string> GetRandomValuesFromList(List<string> values, int count)
        {
            List<string> randomValues = values.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).Take(count).ToList();

            return randomValues;
        }
    }
}