using OpenAI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace NeuralNetworksContenet
{
    public class ImageGenerator
    {
        private static OpenAIApi openai = new OpenAIApi();

        //public async Task<Texture2D> GetObjectImageByInterest()
        //{
        //}
        private static string folderPath = Application.dataPath + "/Resources/Images/";

        public static UnityEvent<Texture2D> OnImageGenerated = new UnityEvent<Texture2D>();

        public static async Task<DataTypes.ImageInformation> GetBackgroundImage(string description)
        {
            Texture2D texture = new Texture2D(2, 2);
            string fileName = "";

            Debug.Log("Начал генерировать");
            CreateImageRequest createImageRequest = new CreateImageRequest()
            {
                Model = OpenAI.DALLEVersion.v3,
                Prompt = $"3D render, digital art, disney style without anthropomorphism: {description}",
                //Prompt = $"{subject}, center view, white background, digital art, 3d render, no letters, no shadows ",
                Size = ImageSize.Size1792H
            };
            Debug.Log(createImageRequest.Prompt);
            var response = await openai.CreateImage(createImageRequest);
            if (response.Error != null)
            {
                response = await openai.CreateImage(createImageRequest);
            }
            if (response.Data != null && response.Data.Count > 0)
            {
                using (var request = new UnityWebRequest(response.Data[0].Url))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    request.SendWebRequest();

                    while (!request.isDone) await Task.Yield();

                    texture.LoadImage(request.downloadHandler.data);
                    OnImageGenerated?.Invoke(texture);

                    byte[] bytes = texture.EncodeToPNG();
                    foreach (ImageData imageData in response.Data)
                    {
                        fileName = imageData.RevisedPrompt.Substring(0, 100);
                        fileName = MDFiveEncoder.Encode(fileName);
                        Debug.Log(fileName);

                        File.WriteAllBytes(folderPath + $"{fileName}" + ".png", bytes);
                    }
                    Debug.Log("Картинка готова");
                }
            }
            else
            {
                Debug.LogWarning("No image was created from this prompt.");
            }

            return new DataTypes.ImageInformation(texture, fileName, createImageRequest.Size);
        }

        public static async Task<DataTypes.ImageInformation> GetObjectImage(string description)
        {
            Texture2D texture = new Texture2D(2, 2);
            string fileName = "";

            Debug.Log("Начал генерировать");

            //var response = await openai.CreateImage(new CreateImageRequest
            //{
            //    //Prompt = $"{description}",
            //    Model = OpenAI.DALLEVersion.v3,
            //    Prompt = $"3D render of {description} in disney Style on similarity white background, digital art, without letters, single object or person.",
            //    //Prompt = $"{subject}, center view, white background, digital art, 3d render, no letters, no shadows ",
            //    Size = ImageSize.Size1024
            //});

            CreateImageRequest createImageRequest = new CreateImageRequest()
            {
                Model = OpenAI.DALLEVersion.v3,
                //Prompt = $"I NEED to test how the tool works with extremely simple prompts. DO NOT add any detail, just use it AS-IS:" +
                //$"{description}",
                Prompt = $"3D image of {description} on a white background, digital art, without letters, single object or person.",
                //Prompt = $"3D render of {description} in disney Style without anthropomorphism.",
                Size = ImageSize.Size1024
            };
            var response = await openai.CreateImage(createImageRequest);
            if (response.Data != null && response.Data.Count > 0)
            {
                using (var request = new UnityWebRequest(response.Data[0].Url))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    request.SendWebRequest();

                    while (!request.isDone) await Task.Yield();

                    texture.LoadImage(request.downloadHandler.data);
                    OnImageGenerated?.Invoke(texture);

                    byte[] bytes = texture.EncodeToPNG();
                    foreach (ImageData imageData in response.Data)
                    {
                        fileName = imageData.RevisedPrompt.Substring(0, 100);
                        fileName = MDFiveEncoder.Encode(fileName);
                        Debug.Log(fileName);
                        File.WriteAllBytes(folderPath + $"{fileName}" + ".png", bytes);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No image was created from this prompt.");
            }

            return new DataTypes.ImageInformation(texture, fileName, createImageRequest.Size);
        }
    }
}