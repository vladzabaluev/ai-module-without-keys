using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using static NeuralNetworksContenet.DataTypes;
using static VisuallySimilarLetterBase;
using static CsvDataTypes;
using UnityEditor;
using OpenAI;
using Unity.VisualScripting;

namespace NeuralNetworksContenet
{
    public class ContentGenerator : MonoBehaviour
    {
        public static ContentGenerator Instance;

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

            VisuallySimilarLetterBase.Initialize();
        }

        [SerializeField] private bool isHandleMode = false;
        public bool IsHandleMode => isHandleMode;

        private async void Start()
        {
            if (isHandleMode)
            {
                return;
            }
            //List<TranslatedELAImage> translatedELAImages = CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "Correct").Keys
            // .Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1).ToList();
            //translatedELAImages.AddRange(CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "ToReview").Keys
            //    .Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1));

            //foreach (string wordType in DataTypes.WordType.CVCes)
            //{
            //    var tempList = translatedELAImages.Where(x => x.WordType == wordType);
            //    await GenerateWordByType(60 - tempList.Count(), wordType, tempList.Select(x => x.Word).ToList());
            //}
            //Debug.LogError("¬—≈ готово");

            //var regenerateImage = CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "aaa")
            //    .ToList();
            //Debug.Log(regenerateImage.Count);
            //foreach (TranslatedLogicImage image in regenerateImage)
            //{
            //    await GenerateBackgroundLogic(image);
            //}
            //regenerateImage = CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "Regenerate").Keys.ToList();
            //Debug.LogError("bg ready");

            //Debug.Log(regenerateImage[0].Word);

            //foreach (var image in regenerateImage)
            //{
            //    CsvHandler.WriteTranslatedLogicImages(image.Key, image.Value, PathStore.LogicFolder + "CorrectImages");

            //    //Debug.Log(image.ImageName);
            //    //if (image.ImageType == DataTypes.ImageType.Object)
            //    //{
            //    //    await GenerateImage(image, "ToTolya");
            //    //}
            //    //else
            //    //{
            //    //    await GenerateBackgroundLogic(image);
            //    //}
            //}
            Dictionary<TranslatedLogicImage, DataTypes.ImageValidation> logicImages = new Dictionary<TranslatedLogicImage, DataTypes.ImageValidation>();
            //logicImages.AddRange(CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "CorrectImagesReviewed"));
            logicImages.AddRange(CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "ToRegenerate 1"));
            //var images = logicImages.Keys.Where(x => x.DeffectChar.Length < 1 && x.ImageName.Length > 5).ToList();
            //foreach (var image in images)
            //{
            //    CsvHandler.WriteTranslatedLogicImages(image, DataTypes.ImageValidation.Correct, "CorrectWithDuplicates");
            //}

            var toGenerateImages = logicImages.Keys/*.Where(x => x.DeffectChar.Length < 1 && x.ImageName.Length < 5)*/.ToList();
            Debug.Log(toGenerateImages.Count);
            //int i = toGenerateImages.Count;
            //foreach (var image in toGenerateImages.Where(x => x.ImageType == DataTypes.ImageType.Background))
            //{
            //    await GenerateBackgroundLogic(image, "ProfReview4");
            //}
            foreach (var image in toGenerateImages.Where(x => x.ImageType == DataTypes.ImageType.Object))
            {
                await GenerateImage(image, "ProfReview6");
                await GenerateImage(image, "ProfReview6");
            }
            //foreach (var image in toGenerateImages)
            //{
            //    Debug.Log("ќсталось " + i);

            //    await GenerateImage(image, "ToReview");
            //    await GenerateImage(image, "ToReview");

            //    i--;
            //}

            Debug.LogError("delete spring");
        }

        //#region ELA, непеределанна€

        /// <summary>
        /// ƒл€ создани€ слов, нетребующих интересов пользовател€, т.е CVC и дифтонга с диграфами
        /// </summary>
        /// <param name="exampleCount"></param>
        /// <param name="wordType">DataTypes.WordType</param>
        /// <param name="childAge"></param>
        /// <returns></returns>
        private async Task GenerateWordByType(int exampleCount, string wordType, List<string> exceptions)
        {
            string result = await TextGenerator.GetWordByType(exampleCount, wordType, exceptions);
            List<string> words = TextHandler.GetListFromString(result);
            foreach (var word in words)
            {
                TranslatedELAImage translatedELAImage = new TranslatedELAImage(word, wordType, "", "", ImageType.Object, "", "", "");
                await GenerateImage(translatedELAImage);
            }
            Debug.LogError(wordType + "готовы");
        }

        ///// <summary>
        ///// √енерирует диграфы и дифтонги
        ///// </summary>
        ///// <param name="exampleCount"></param>
        ///// <param name="wordType">DataTypes.WordType</param>
        ///// <param name="childAge"></param>
        ///// <returns></returns>
        //private async Task GeneratePhonemeWord(int exampleCount, string wordType)
        //{
        //    //—оздали, записали, почистили, новый возраст
        //    List<string> words = new List<string>();

        //    await TextGenerator.GetPhoneme(exampleCount, wordType, words);

        //    CsvHandler.WriteELAWordsToCsvFile(wordType, words);
        //}

        ///// <summary>
        ///// √енерирует слова, начинающеес€ с определенной буквы, не затрагива€ интересы пользовател€
        ///// </summary>
        ///// <param name="exampleCount"></param>
        ///// <param name="wordType">DataTypes.WordType</param>
        ///// <returns></returns>
        //private async Task GenerateWordsStartingWithSymbol(int exampleCount, string wordType, string letter)
        //{
        //    List<string> words = new List<string>();
        //    TextRequestHandler.TryGetList(wordType, out words);
        //    string answer = await TextGenerator.GetWorldStartingWithLetter(exampleCount, letter, words);

        //    CsvHandler.WriteELAWordsToCsvFile(wordType, TextHandler.GetListFromString(answer), startSymbol: letter);
        //}

        /// <summary>
        /// —оздает картинку дл€ DataTypes.WordType
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        //private async Task GenerateImageByWord(List<WordELA> words)
        //{
        //    foreach (var word in words)
        //    {
        //        string description = await TextGenerator.GetShortDescriptionOf(word.StoredWord);

        //        DataTypes.ImageInformation imageInformation = await imageGenerator.GetObjectImage(description);

        //        CsvHandler.WriteImageELA(word.StoredWord, word.Type, word.Interest, word.Age, word.StartSymbol, DataTypes.ImageType.Object, imageInformation.Size.x, imageInformation.Size.y, imageInformation.Name);
        //    }

        //    //—читываем файл со словами, шлем запрос, сохран€ем в csv дл€ картинок
        //}

        /// <summary>
        /// —оздает картинку в зависимости от интересов пользовател€
        /// </summary>
        /// <param name="exampleCount"></param>
        /// <param name="interest"></param>
        /// <returns></returns>
        private async Task GenerateELAImageByInterest(int exampleCount, string interest)
        {
            List<string> examples = new List<string>();
            examples = TextHandler.GetListFromString(await TextGenerator.GetObjectDescriptionByInterest(exampleCount, interest, examples));
            foreach (var example in examples)
            {
                Debug.Log(example);
                DataTypes.ImageInformation imageInformation = await ImageGenerator.GetObjectImage(example);
                CsvHandler.WriteImageELA("", "", interest, "", DataTypes.ImageType.Object, imageInformation.Name);
            }

            //Ѕерем тему, генерируем пример-описание (несколько), шлем запросы на генерацию картинок по описанию, сохран€ем в csv по интересу
        }

        /// <summary>
        /// —оздает картинку дл€ заднего фона по заданной тематике
        /// </summary>
        /// <param name="exampleCount"></param>
        /// <param name="interest"></param>
        /// <returns></returns>
        private async Task GenerateBackground(int exampleCount, string interest)
        {
            for (int i = 0; i < exampleCount; i++)
            {
                DataTypes.ImageInformation imageInformation;
                string description = await TextGenerator.GetBackgroundDescriptionByInterest(interest);
                if (description == null)
                {
                    imageInformation = await ImageGenerator.GetBackgroundImage($"Draw a background for child accociated with {interest}");
                }
                else
                {
                    imageInformation = await ImageGenerator.GetBackgroundImage(description);
                }
                CsvHandler.WriteImageELA("", "", interest, "", DataTypes.ImageType.Background, imageInformation.Name);
            }
            //Ѕерем тему, генерируем пример-описание (несколько), шлем запросы на генерацию картинок по описанию, сохран€ем в csv по интересу
        }

        //#endregion ELA, непеределанна€

        //private async Task GenerateExampleByColor(int exampleCount, DataTypes.Color color)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetExamplesByColor(exampleCount, color));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Color, color + "", "");
        //    }
        //}

        //private async Task GenerateExampleByShape(int exampleCount, DataTypes.Shape shape)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetExamplesByShape(exampleCount, shape));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Shape, shape + "", "");
        //    }
        //}

        //private async Task GenerateAliveExamples(int exampleCount)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetLivingBeing(exampleCount));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, LogicImageCategoriesEnumeration.LifeFormat, LifeType.LivingBeing + "", "");
        //    }
        //}

        //private async Task GenerateInanimateExamples(int exampleCount)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetInanimateObject(exampleCount));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, LogicImageCategoriesEnumeration.LifeFormat, LifeType.InanimateObject + "", "");
        //    }
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="exampleCount"></param>
        ///// <param name="category">ƒомашность или класс животного, т.е DataTypes.LogicImageCategories.AnimalByHomeType
        ///// или DataTypes.LogicImageCategories.AnimalClass</param>
        ///// <param name="animalType">DataTypes.AnimalByHomeType или DataTypes.AnimalClass</param>
        ///// <returns></returns>
        //private async Task GenerateWildLifeExamples(int exampleCount, DataTypes.Wildlife wildLifeType)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetWildLifeExamples(exampleCount, wildLifeType));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, LogicImageCategoriesEnumeration.WildLife, wildLifeType + "", "");
        //    }
        //}

        //private async Task GenerateAnimalByHomeType(int exampleCount, DataTypes.AnimalByHomeType wildLifeType)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetAnimalsByHomeType(exampleCount, wildLifeType));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, LogicImageCategoriesEnumeration.AnimalByHomeType, wildLifeType + "", "");
        //    }
        //}

        //private async Task GenerateAnimalByMovementMethod(int exampleCount, DataTypes.AnimalByMethodOfMovement movementMethod)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetAnimalByMovementMethod(exampleCount, movementMethod));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, LogicImageCategoriesEnumeration.MovementMethod, movementMethod + "", "");
        //    }
        //}

        //private async Task GenerateSeasonExamples(int exampleCount, DataTypes.Season season)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetSeasonsExamples(exampleCount, season));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Season, season + "", "");
        //    }
        //}

        //private async Task GenerateResidencesExamples(int exampleCount, DataTypes.Residence residence)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetResidenceExamples(exampleCount, residence));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Residence, residence + "", "");
        //    }
        //}

        //private async Task GenerateAnimalsByHabitat(int exampleCount, DataTypes.Habitat habitat)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetAnimalByHabitat(exampleCount, habitat));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Habitat, habitat + "", "");
        //    }
        //}

        //private async Task GenerateRoomExamples(int exampleCount, DataTypes.Room room)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetRoomExamples(exampleCount, room));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Room, room + "", "");
        //    }
        //}

        //private async Task GenerateProfessionalObjectsExamples(int exampleCount, string profession)
        //{
        //    List<string> examples = new List<string>();
        //    examples = TextHandler.GetListFromString(await TextGenerator.GetProfessionalObjectsExamples(exampleCount, profession));
        //    foreach (string example in examples)
        //    {
        //        CsvHandler.WriteWordLogic(example, DataTypes.LogicImageCategoriesEnumeration.Profession, profession, "");
        //    }
        //}

        private async Task GenerateBackgroundLogic(TranslatedLogicImage translatedLogicImage, string filePath)
        {
            string description = await TextGenerator.GetBackgroundDescription(translatedLogicImage.ParameterValue);
            DataTypes.ImageInformation imageInformation = new ImageInformation(new Texture2D(2, 2), "", ImageSize.Size256);
            if (description == null)
            {
                imageInformation = await ImageGenerator.GetBackgroundImage(translatedLogicImage.ParameterValue);
            }
            else
            {
                imageInformation = await ImageGenerator.GetBackgroundImage(description);
            }
            translatedLogicImage.ImageName = imageInformation.Name;
            CsvHandler.WriteTranslatedLogicImages(translatedLogicImage, DataTypes.ImageValidation.None, PathStore.LogicFolder + filePath);

            //Ѕерем тему, генерируем пример-описание (несколько), шлем запросы на генерацию картинок по описанию, сохран€ем в csv по интересу
        }

        private async Task GenerateImage(CsvDataTypes.ELAImage elaImage)
        {
            List<string> parameters = new List<string>();
            string visualDescription = await TextGenerator.GetShortDescriptionOf(elaImage.Word, parameters);
            DataTypes.ImageInformation generatedImageInformation = await ImageGenerator.GetObjectImage(visualDescription);
            CsvHandler.WriteImageELA(elaImage.Word, elaImage.WordType, elaImage.Interest, elaImage.StartSymbol, elaImage.ImageType, elaImage.ImageName);
        }

        private async Task GenerateImage(CsvDataTypes.LogicImage imageLogic)
        {
            List<string> parameters = new List<string>() { imageLogic.ParameterValue };
            string visualDescription = await TextGenerator.GetShortDescriptionOf(imageLogic.Word, parameters);
            DataTypes.ImageInformation generatedImageInformation = await ImageGenerator.GetObjectImage(visualDescription);
            CsvHandler.WriteImageLogic(imageLogic.ImageType, imageLogic.Word, imageLogic.Category, imageLogic.ParameterValue, generatedImageInformation.Name);
        }

        public async Task GenerateImage(CsvDataTypes.TranslatedELAImage translatedElaImage, string fileName)
        {
            List<string> parameters = new List<string>();
            string visualDescription = await TextGenerator.GetShortDescriptionOf(translatedElaImage.Word, parameters);
            DataTypes.ImageInformation generatedImageInformation = await ImageGenerator.GetObjectImage(visualDescription);
            translatedElaImage.ImageName = generatedImageInformation.Name;
            CsvHandler.WriteTranslatedELAImage(translatedElaImage, DataTypes.ImageValidation.None, PathStore.ELAFolder + $"{fileName}");
        }

        private async Task GenerateImage(CsvDataTypes.TranslatedLogicImage imageLogic, string fileName)
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

        private async Task GenerateBackgroundLogic(CsvDataTypes.LogicImage imageLogic)
        {
            string description = await TextGenerator.GetBackgroundDescriptionByInterest(imageLogic.ParameterValue);
            DataTypes.ImageInformation imageInformation = new ImageInformation(new Texture2D(2, 2), "", ImageSize.Size256);
            if (description == null)
            {
                imageInformation = await ImageGenerator.GetBackgroundImage(imageLogic.ParameterValue);
            }
            else
            {
                imageInformation = await ImageGenerator.GetBackgroundImage(description);
            }
            CsvHandler.WriteImageLogic(imageLogic.ImageType, imageLogic.Word, imageLogic.Category, imageLogic.ParameterValue, imageInformation.Name);

            //Ѕерем тему, генерируем пример-описание (несколько), шлем запросы на генерацию картинок по описанию, сохран€ем в csv по интересу
        }

        public async Task GenerateObjectELAByInterest(CsvDataTypes.TranslatedELAImage ELAImage)
        {
            DataTypes.ImageInformation imageInformation;
            string description = await TextGenerator.GetObjectDescriptionByInterest(ELAImage.Interest);

            imageInformation = await ImageGenerator.GetObjectImage(description);
            ELAImage.ImageName = imageInformation.Name;
            CsvHandler.WriteTranslatedELAImage(ELAImage, DataTypes.ImageValidation.None, PathStore.ELAFolder + "ToReview");
        }

        public async Task GenerateBackgroundELAByInterest(CsvDataTypes.TranslatedELAImage ELAImage)
        {
            DataTypes.ImageInformation imageInformation;
            string description = await TextGenerator.GetBackgroundDescriptionByInterest(ELAImage.Interest);

            imageInformation = await ImageGenerator.GetBackgroundImage(description);
            ELAImage.ImageName = imageInformation.Name;
            CsvHandler.WriteTranslatedELAImage(ELAImage, DataTypes.ImageValidation.None, PathStore.ELAFolder + "ToReview");
        }
    }
}