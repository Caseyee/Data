using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject NewRecordText;
    public Text BestScoreText;
    public DataSaver dataSaver;
    public InputField PlayersNameInput;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        dataSaver = GameObject.Find("DataSaver").GetComponent<DataSaver>();

        dataSaver.LoadData();

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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

        BestScoreText.text = $"Best Score: {dataSaver.bestScore} Name: {dataSaver.playerName}";
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
    }

    public void GameOver()
    {
        if (m_Points <= dataSaver.bestScore)
        {
            m_GameOver = true;
            GameOverText.SetActive(true);
        }
        else
        {
            PlayersNameInput.gameObject.SetActive(true);
            NewRecordText.gameObject.SetActive(true);
        }
    }

    public void SaveData()
    {
        dataSaver.playerName = PlayersNameInput.text;
        dataSaver.bestScore = m_Points;
        dataSaver.SaveInfo();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
