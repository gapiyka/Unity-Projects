using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Text quoteText;
    [SerializeField] private TextAsset textFile;
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        LoadRandomQuote();
    }

    void LoadRandomQuote()
    {
        string[] FileQuotes = textFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        int randIndex = UnityEngine.Random.Range(0, FileQuotes.Length);
        quoteText.text = FileQuotes[randIndex];
    }
}
