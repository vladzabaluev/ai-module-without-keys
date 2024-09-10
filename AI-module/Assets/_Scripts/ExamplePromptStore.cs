using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ExamplePromptStore : MonoBehaviour
{
    private string _task;
    [SerializeField] private InterestsContainer _interestsContainer;

    [SerializeField] private string taskFileName;

    private void Start()
    {
        TextAsset taskFile = Resources.Load<TextAsset>("TextFiles/TaskExamples/" + taskFileName);
        _task = taskFile.text;
    }

    public void CallContentGenerator()
    {
        //var result = await ContentGenerator.Instance.GetRewrittenTask(_task, _interestsContainer.GetRandomInterest(), 6);
    }
}