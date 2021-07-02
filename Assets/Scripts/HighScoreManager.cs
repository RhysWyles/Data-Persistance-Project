using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    #region Singleton
    public static HighScoreManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScores();
    }
    #endregion

    public string currentPlayerName;

    public string playerName;
    public int highScore;

    [SerializeField]
    HighScores highScores = new HighScores();

    private void Start()
    {
       
    }

    public void NewScore(int newScore, string name)
    {
        if (highScores != null)
        {
            for (int i = 0; i < highScores.highScores.Length; i++)
            {
                if (newScore > highScores.highScores[i].highScore)
                {
                    Score oldHighScore = highScores.highScores[i];
                    NewScore(oldHighScore.highScore, oldHighScore.playerName);
                    highScores.highScores[i].highScore = newScore;
                    highScores.highScores[i].playerName = currentPlayerName;
                    SaveHighScores();
                    Debug.Log("New High Score");
                    break;
                }
            }
        }
        else
        {
            highScores.highScores[0].playerName = HighScoreManager.instance.currentPlayerName;
            highScores.highScores[0].highScore = newScore;
        }
    }

    public void SaveHighScores()
    {
        string json = JsonUtility.ToJson(highScores);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScores data = JsonUtility.FromJson<HighScores>(json);

            highScores = data;
        }
    }

    [System.Serializable]
    class Score
    {
        public string playerName;
        public int highScore;
    }

    [System.Serializable]
    class HighScores
    {
        public Score[] highScores = new Score[5];
    }

}
