using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private string playerName;
    private bool m_Started = false;
    private int m_Points;
    private int recordPoints;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        playerName = Manager.playerName;
        Load();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > recordPoints)
        {
            BestScoreText.text = $"Best Score : {playerName} : {m_Points}";
        }
    }

    public void GameOver()
    {
        if (m_Points > recordPoints)
        {
            recordPoints = m_Points;
            Save();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int score;
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(new SaveData
        {
            playerName = playerName,
            score = m_Points
        });

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void Load()
    {
        FileInfo f = new FileInfo(Application.persistentDataPath + "/savefile.json");

        if (f.Exists)
        {
            string json = File.ReadAllText(f.FullName);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            string recordPlayer = data.playerName;
            recordPoints = data.score;
            BestScoreText.text = $"Best Score : {recordPlayer} : {recordPoints}";
        }
        else
        {
            BestScoreText.text = "Best Score : None";
        }
    }

}
