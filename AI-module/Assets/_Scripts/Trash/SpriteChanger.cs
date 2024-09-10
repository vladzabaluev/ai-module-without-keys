using NeuralNetworksContenet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image objectImageInterest;

    [SerializeField] private TMP_Text wordByType;
    [SerializeField] private Image objectImageByWordType;
    [SerializeField] private TMP_Text wordByLetter;
    [SerializeField] private Image objecImageBySymbol;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float similarity;
    [SerializeField] private float stability;
    [SerializeField] private float style;

    [SerializeField] private List<Image> logicImages;
    [SerializeField] private List<TMP_Text> texts;

    // Start is called before the first frame update
    private async void Start()
    {
        //Destroy(backgroundImage.sprite.texture);
        //backgroundImage.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetBackgroundByInterest(DataTypes.Interest.Candy), true);
        //Destroy(objectImageInterest.sprite.texture);

        //objectImageInterest.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetObjectImageByInterest(DataTypes.Interest.Candy), true);

        //string word;
        //Destroy(objectImageByWordType.sprite.texture);

        //objectImageByWordType.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetObjectImageByWordType(DataTypes.WordType.CCVC, out word), true);
        //wordByType.text = word;
        //Destroy(objecImageBySymbol.sprite.texture);

        //objecImageBySymbol.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetImageByFirstLetter("m", out word), true);
        //wordByLetter.text = word;

        List<string> corrcetImagesNames = new List<string>();
        List<string> incorrectImagesNames = new List<string>();

        NeuralContentAPI.GetLogicImageNames(DataTypes.LogicImageCategoriesEnumeration.Room, 5, out corrcetImagesNames, 2, out incorrectImagesNames);

        List<Sprite> corrcetImages = await NeuralContentAPI.GetImagesByNames(corrcetImagesNames);
        List<Sprite> incorrectImages = await NeuralContentAPI.GetImagesByNames(incorrectImagesNames);

        for (int i = 0; i < corrcetImagesNames.Count; i++)
        {
            logicImages[i].sprite = corrcetImages[i];
            texts[i].text = "correct";
        }

        for (int i = corrcetImagesNames.Count; i < (corrcetImagesNames.Count + incorrectImagesNames.Count); i++)
        {
            logicImages[i].sprite = incorrectImages[i - corrcetImagesNames.Count];
            texts[i].text = "WRONG";
        }

        //string name, habitat1, habitat2;

        //NeuralContentAPI.GetImageNameWithCategories(DataTypes.LogicImageCategoriesEnumeration.Habitat, out name, out habitat1, out habitat2);

        //logicImages[5].sprite = await NeuralContentAPI.GetImageByName(name);

        //texts[5].text = $"Correct : {habitat1}";

        //texts[6].text = $"WRONG : {habitat2}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testAction?.Invoke(audioSource, similarity, stability, style);
        }
    }

    private async void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Destroy(backgroundImage.sprite.texture);
            backgroundImage.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetBackgroundByInterest(DataTypes.Interest.Candy), true);
            Destroy(objectImageInterest.sprite.texture);

            objectImageInterest.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetObjectImageByInterest(DataTypes.Interest.Candy), true);

            string word;
            Destroy(objectImageByWordType.sprite.texture);

            objectImageByWordType.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetObjectImageByWordType(DataTypes.WordType.CCVC, out word), true);
            wordByType.text = word;
            Destroy(objecImageBySymbol.sprite.texture);

            objecImageBySymbol.sprite = await ImageLoader.LoadImageByName(NeuralContentAPI.GetImageByFirstLetter("m", out word), true);
            wordByLetter.text = word;
        }
    }

    private Action<AudioSource, float, float, float> testAction = async (AudioSource audioSource, float a, float b, float c) =>
    {
        //string taskText = "On corrcetImagesNames screen, there are  bubbles. Inside these bubbles are random letters. Press all the bubbles that have the letter the voice says";
        string taskText = "Hey Vanya! Let's play a game with letters. They're all in a line, just like the alphabet! But uh-oh, one letter is missing. Can you find out which letter it is and write it down? Have fun!";
        audioSource.PlayOneShot(await TextToSpeechConverter.GetTextVoiceover(taskText, new VoiceSettings(a, b, c, true)));
    };
}