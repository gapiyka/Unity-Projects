using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class CanvasScript : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Button yourButton;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text AllCoinsText;
    [SerializeField] private TextMesh scoreTabloText;
    [SerializeField] private Text topScoreText;

    public int score = 0;
    public int topScore;

    private string path; 
    private string pathCoin;
    private int earnCoin;
    private int coins;
    void Start()
    {
        path = Application.dataPath + "/Score.txt";
        pathCoin = Application.dataPath + "/Coins.txt";
        CreateText();
        topScore = Convert.ToInt32(File.ReadAllText(path));
        coins = Convert.ToInt32(File.ReadAllText(pathCoin));
        scoreText.text = "Score: " + score + " m";
        scoreTabloText.text = "Top Score: \n" + Math.Round((float)topScore/1000, 1) + " km";
        topScoreText.text = "Top Score: " + Math.Round((float)topScore / 1000, 1) + " km";
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        AllCoinsText.text = Convert.ToString(coins);//added
    }

    void Update()
    {
        score = (int)-player.transform.position.x; 
        scoreText.text = "Score: " + score + " m";

        coinsText.text = Convert.ToString(earnCoin);

        AllCoinsText.text = Convert.ToString(coins);
    }

    void CreateText()
    {
        if(!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }
        if (!File.Exists(pathCoin))
        {
            File.WriteAllText(pathCoin, "0");
        }
    }

    public void FillText()
    {
        if (score > topScore)
        {
            File.WriteAllText(path, Convert.ToString(score));
        }
    }
    public void ReNewCoin(int ammount)
    {
        coins = ammount;
    }
    public void PlusCoin()
    {
        earnCoin++;
        coins++;
    }
    public void SumCoin()
    {
        File.WriteAllText(pathCoin, Convert.ToString(coins));
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene(0);
    }

    public int Coins()
    {
        return coins;
    }
}
