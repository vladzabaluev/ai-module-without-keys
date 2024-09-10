using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader
{
    private const string russialURL = "1";
    private const string americanURL = "1";
    // private static Texture2D texture = new Texture2D(2, 2);

    public static async Task<Sprite> LoadImage(string URL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        var asyncOperation = www.SendWebRequest();
        Texture2D texture = new Texture2D(2, 2);

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        else
        {
            Debug.LogError("Failed to download image: " + www.error);
        }
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
        return sprite;
    }

    public static async Task<Sprite> LoadImageByName(string name, bool isRussian)
    {
        name = name.Trim();
        string URL = isRussian ? russialURL : americanURL;
        URL += name + ".png";

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);

        var asyncOperation = www.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        //place your code

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Получаем байты текстуры
            byte[] textureBytes = www.downloadHandler.data;

            // Создаем новую текстуру и загружаем в нее байты
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(textureBytes);

            // Создаем спрайт из текстуры
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
            return sprite;
        }
        else
        {
            Debug.LogError("Failed to load image. Error: " + www.error + "\n URL: " + www.url);
            return null;
        }
    }
}