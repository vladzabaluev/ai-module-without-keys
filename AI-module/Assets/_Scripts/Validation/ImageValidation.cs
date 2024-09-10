using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NeuralNetworksContenet;
using static CsvDataTypes;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Android;
using System.IO;

public class ImageValidation : MonoBehaviour
{
    [SerializeField] private TMP_Text miniGamesText;
    [SerializeField] private TMP_Text imageInformation;
    [SerializeField] private TMP_Text imageRemainedCount;

    [SerializeField] private Image image;
    private int currentIndex = 0;
    private List<TranslatedELAImage> elaImages;
    private List<TranslatedLogicImage> logicImages;
    [SerializeField] private bool isElaComplete = true;

    [SerializeField] private Button correctImageButton;
    [SerializeField] private Button badWordButton;
    [SerializeField] private Button badImageButton;
    [SerializeField] private Button resetImageCount;

    [SerializeField] private DataTypes.LogicImageCategoriesEnumeration logicImageCategory;
    [SerializeField] private string ELAGroup;

    private void PermissonAccept()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        miniGamesText.text = Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) + "";

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Application.Quit();
        }
    }

    private void ButtonInitialization()
    {
        correctImageButton.onClick.AddListener(() =>
        {
            ValidateImage(DataTypes.ImageValidation.Correct);
        });
        badWordButton.onClick.AddListener(() =>
        {
            ValidateImage(DataTypes.ImageValidation.BadWord);
        });
        badImageButton.onClick.AddListener(() =>
        {
            ValidateImage(DataTypes.ImageValidation.BadImage);
        });
        resetImageCount.onClick.AddListener(async () =>
        {
            PlayerPrefs.SetInt("ImagesCount", 0);
            currentIndex = PlayerPrefs.GetInt("ImagesCount");
            await UpdateScreenInfo();
        });
    }

    // Start is called before the first frame update
    private async void Start()
    {
        PermissonAccept();
        ButtonInitialization();

        //elaImages = CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "ToReview").Keys.Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length == 0).ToList();
        //elaImages.AddRange(CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "MissingWords").Keys
        //    .Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1));
        //Debug.Log(elaImages.Count);
        elaImages = CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "ToReview").Keys.Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length == 0).ToList();
        elaImages.AddRange(CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "MissingWords").Keys
            .Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length < 1));
        Debug.Log(elaImages.Count);

        //List<TranslatedELAImage> reviewedElaImages = CsvHandler.ReadTranslatedReviewedELAImages(PathStore.ELAFolder + "ELA_2").Keys.Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length == 0).ToList();
        //Debug.Log(reviewedElaImages.Count);

        //List<TranslatedELAImage> duplicatedImages = new List<TranslatedELAImage>();
        //foreach (TranslatedELAImage logicImage in elaImages)
        //{
        //    foreach (TranslatedELAImage temp in reviewedElaImages)
        //    {
        //        if (logicImage.ImageName.Trim() == temp.ImageName.Trim())
        //        {
        //            duplicatedImages.Add(logicImage);
        //        }
        //    }
        //}
        //Debug.Log(duplicatedImages.Count);

        //reviewedElaImages = elaImages.Except(duplicatedImages).ToList();
        //Debug.Log(reviewedElaImages.Count);
        //elaImages = reviewedElaImages.Where(x => x.Word.Length > 1).ToList();
        //Debug.Log(elaImages.Count);
        logicImages = new List<TranslatedLogicImage>();
        logicImages.AddRange(CsvHandler.ReadTranslatedReviewedLogicImages(PathStore.LogicFolder + "TolyaReview").Keys.Where(x => x.ImageName.Length > 5 && x.DeffectChar.Length == 0));
        Debug.Log(logicImages.Count);

        #region Обработчик групп ELA

        //switch (ELAGroup)
        //{
        //    case "1":
        //        miniGamesText.text = DataTypes.WordType.CVC + " и т.д";
        //        elaImages = elaImages.Where(x => x.WordType == DataTypes.WordType.CVC
        //        || x.WordType == DataTypes.WordType.CCVC || x.WordType == DataTypes.WordType.CVCC
        //        || x.WordType == DataTypes.WordType.CVCe).ToList();
        //        break;

        //    case "2":
        //        miniGamesText.text = "Диграфы и дифтонги";
        //        elaImages = elaImages.Where(x => x.WordType == DataTypes.WordType.Digraph
        //        || x.WordType == DataTypes.WordType.Diphthong).ToList();
        //        break;

        //    case "3":
        //        miniGamesText.text = "Слова начинающиеся с символов";
        //        elaImages = elaImages.Where(x => x.WordType == DataTypes.WordType.WordsStartingWithLetters).ToList();
        //        break;

        //    case "4":
        //        miniGamesText.text = "Слова по интересам";
        //        elaImages = elaImages.Where(x => x.ImageType == DataTypes.ImageType.Object && x.WordType.Length < 2).ToList();
        //        break;

        //    case "5":
        //        miniGamesText.text = "Фона по интересам";
        //        elaImages = elaImages.Where(x => x.ImageType == DataTypes.ImageType.Background && x.WordType.Length < 2).ToList();
        //        break;
        //}

        #endregion Обработчик групп ELA

        #region Обработчик групп Logic

        //switch (logicImageCategory)
        //{
        //    case DataTypes.LogicImageCategoriesEnumeration.Color:
        //        miniGamesText.text = "Цвета";
        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Color).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.Shape:
        //        miniGamesText.text = "Форма";
        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Shape).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.Season:
        //        miniGamesText.text = "Время года";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Season).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.WildLife:
        //        miniGamesText.text = "Сухопут, рыба, птица + растения";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.WildLife).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.Habitat:
        //        miniGamesText.text = "Среда обитания";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Habitat).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.Room:
        //        miniGamesText.text = "Комнаты";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Room).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.LifeFormat:
        //        miniGamesText.text = "Живые или неживые";
        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.LifeFormat).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.AnimalByHomeType:
        //        miniGamesText.text = "Дикие или домашние";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.AnimalByHomeType).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.Residence:
        //        miniGamesText.text = "Ферма или город";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Residence).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.Profession:
        //        miniGamesText.text = "Профессии";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.Profession).ToList();
        //        break;

        //    case DataTypes.LogicImageCategoriesEnumeration.MovementMethod:
        //        miniGamesText.text = "Способ передвижения";

        //        logicImages = logicImages.Where(x => x.Category == DataTypes.LogicImageCategoriesEnumeration.MovementMethod).ToList();
        //        break;
        //}

        #endregion Обработчик групп Logic

        currentIndex = PlayerPrefs.GetInt("ImagesCount");

        await UpdateScreenInfo();
    }

    // Update is called once per frame

    private async Task SetELAImage()
    {
        if (currentIndex > logicImages.Count)
        {
            imageInformation.text = "ВРЕМЯ НАЖИМАТЬ НА КНОПКУ";
        }
        else
        {
            imageInformation.text = $"Word: {elaImages[currentIndex].Word} \n Translation: {elaImages[currentIndex].Translation}\nType: {elaImages[currentIndex].WordType} \n" +
    $"Interest: {elaImages[currentIndex].Interest} \nSymbol: {elaImages[currentIndex].StartSymbol} \n" +
    $"ImageType: {elaImages[currentIndex].ImageType} \n";

            image.sprite = await ImageLoader.LoadImageByName(elaImages[currentIndex].ImageName, true);
        }
    }

    private async Task SetLogicImage()
    {
        if (currentIndex > logicImages.Count)
        {
            imageInformation.text = "ВРЕМЯ НАЖИМАТЬ НА КНОПКУ";
        }
        else
        {
            imageInformation.text = $"Слово: {logicImages[currentIndex].Word} \nПеревод: {logicImages[currentIndex].Translation} \n" +
$"Значение: {logicImages[currentIndex].ParameterValue} \n" +
$"Тип изображения: {logicImages[currentIndex].ImageType} \n";
            Resources.UnloadUnusedAssets();

            image.sprite = await ImageLoader.LoadImageByName(logicImages[currentIndex].ImageName, true);
        }
    }

    private void ChangeValue()
    {
        currentIndex++;
        PlayerPrefs.SetInt("ImagesCount", currentIndex);
        if (isElaComplete)
        {
            if (currentIndex >= logicImages.Count)
            {
                Application.Quit();
            }
        }
        else
        {
            if (currentIndex >= elaImages.Count)
            {
                Debug.Log("aaaaaaaaaaaa");
                Application.Quit();
            }
        }

        //if (currentIndex == elaImages.Count && !isElaComplete)
        //{
        //    currentIndex = 0;
        //    isElaComplete = true;
        //}
        //else if (currentIndex == logicImages.Count && isElaComplete)
        //{
        //    //miniGamesText.text = "ВСЕ";
        //    //imageRemainedCount.text = "КОНЕЦ";
        //    //foreach (Button button in buttons)
        //    //{
        //    //    button.gameObject.SetActive(false);
        //    //}
        //    Debug.Log(PlayerPrefs.GetString("Images"));
        //    GUIUtility.systemCopyBuffer = PlayerPrefs.GetString("Images");
        //    Application.Quit();
        //}
    }

    private async Task UpdateScreenInfo()
    {
        if (!isElaComplete)
        {
            //miniGamesText.text = "ELA";
            imageRemainedCount.text = "Осталось " + (elaImages.Count - currentIndex);

            await SetELAImage();
        }
        else
        {
            //miniGamesText.text = "Logic";
            imageRemainedCount.text = "Осталось " + (logicImages.Count - currentIndex);

            await SetLogicImage();
        }
    }

    public async void ValidateImage(DataTypes.ImageValidation validValue)
    {
        if (!isElaComplete)
        {
            CsvHandler.WriteTranslatedELAmageOnDevice(elaImages[currentIndex], validValue);
            //string temp = PlayerPrefs.GetString("Images");
            //PlayerPrefs.SetString("Images", temp + elaImages[currentIndex].ImageName + "\n");
            // CsvHandler.WriteELAFromDevice(elaImages[currentIndex]);
        }
        else
        {
            CsvHandler.WriteTranslatedLogicImagesOnDevice(logicImages[currentIndex], validValue);

            //CsvHandler.WriteLogicFromDevice(logicImages[currentIndex]);
            //string temp = PlayerPrefs.GetString("Images");
            //PlayerPrefs.SetString("Images", temp + logicImages[currentIndex].ImageName + "\n");
        }

        ChangeValue();
        await UpdateScreenInfo();
    }
}