using NeuralNetworksContenet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    [SerializeField] private Image picture;

    private void Awake()
    {
        ImageGenerator.OnImageGenerated.AddListener(ChangeImage);
    }

    // Update is called once per frame
    private void ChangeImage(Texture2D tex)
    {
        picture.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 1);
    }
}