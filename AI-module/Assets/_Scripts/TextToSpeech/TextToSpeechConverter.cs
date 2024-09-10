using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Serialization;
using System.Globalization;

public class TextToSpeechConverter
{
    private const string URL = "1";
    private const string API_KEY = "1";

    private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new OpenAI.CustomNamingStrategy()
        },
        MissingMemberHandling = MissingMemberHandling.Error,
        Culture = CultureInfo.InvariantCulture
    };

    public static async Task<AudioClip> GetTextVoiceover(string textToVoice, VoiceSettings voiceSettings)
    {
        var postData = new AudioRequest
        {
            Text = textToVoice,
            VoiceSettings = voiceSettings
        };
        string jsonPayload = JsonConvert.SerializeObject(postData, jsonSerializerSettings);
        byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(payloadBytes);
        DownloadHandlerAudioClip downloadHandler = new DownloadHandlerAudioClip(URL, AudioType.MPEG);
        AudioClip audioClip;
        Debug.Log(jsonPayload);
        using (var request = new UnityWebRequest(URL, "POST"))
        {
            request.uploadHandler = uploadHandler;
            request.downloadHandler = downloadHandler;

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("xi-api-key", API_KEY);
            request.SetRequestHeader("Accept", "audio/mpeg");
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading audio: " + request.error);
            }
            audioClip = downloadHandler.audioClip;
        }
        Debug.Log("aaa");
        return audioClip;
    }
}