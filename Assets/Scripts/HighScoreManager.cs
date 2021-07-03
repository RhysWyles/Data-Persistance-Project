using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public CanvasRenderer highScorePanel;
    public GameObject highScoreRow;

    [SerializeField]
    HighScores highScores = new HighScores();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Scene Loaded: " + scene.name);
        if (currentScene == 0)
        {
            if (highScorePanel == null)
                FindHighScorePanel();

            CreateHighScoreRows();
        }
    }

    void FindHighScorePanel()
    {
        //Under the Canvas there are two panels - Menu & HighScore need to find HighScore one
        CanvasRenderer[] CanvasPanels = new CanvasRenderer[7];
        CanvasPanels = FindObjectOfType<Canvas>().GetComponentsInChildren<CanvasRenderer>();

        for(int i = 0; i < CanvasPanels.Length; i++)
        {
            if (CanvasPanels[i].CompareTag("HighScoreTable"))
            {
                highScorePanel = CanvasPanels[i];
                Debug.Log("Found Canvas Panel with tag High Score Table");
            }
        }
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
            highScores.highScores[0].playerName = name;
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

    public void CreateHighScoreRows()
    {
        for (int i = 0; i < highScores.highScores.Length; i++)
        {
            GameObject newHighScoreRow;
            Vector3 highScorePanelPosition = highScorePanel.transform.position;
            Vector3 newRowPosition = highScorePanelPosition + new Vector3(0, (-30 * i) + 40, 0);

            newHighScoreRow = Instantiate(highScoreRow, newRowPosition, Quaternion.identity, highScorePanel.transform);

            Text[] rowTextFields = newHighScoreRow.GetComponentsInChildren<Text>();

            rowTextFields[0].text = (i + 1).ToString();
            rowTextFields[1].text = highScores.highScores[i].highScore.ToString();
            rowTextFields[2].text = highScores.highScores[i].playerName;
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
