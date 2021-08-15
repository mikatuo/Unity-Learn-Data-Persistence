using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
	{
		UpdateBestScore();
		InitializeBricks();
	}

	void Update()
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

    void UpdateBestScore()
	{
        var gameState = GameState.Instance;
        if (gameState == null || gameState.BestScore == null)
            return;
        BestScoreText.text = $"Best Score : {gameState.BestScore.PlayerName} : {gameState.BestScore.ScorePoints}";
	}

	void InitializeBricks()
	{
		const float step = 0.6f;
		int perLine = Mathf.FloorToInt( 4.0f / step );

		int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
		for ( int i = 0; i < LineCount; ++i ) {
			for ( int x = 0; x < perLine; ++x ) {
				Vector3 position = new Vector3( -1.5f + step * x, 2.5f + i * 0.3f, 0 );
				var brick = Instantiate( BrickPrefab, position, Quaternion.identity );
				brick.PointValue = pointCountArray[ i ];
				brick.onDestroyed.AddListener( AddPoint );
			}
		}
	}

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (GameState.Instance != null)
            GameState.Instance.SetPoints(m_Points);
    }

    public void GameOver()
    {
        m_GameOver = true;
        var gameState = GameState.Instance;
        if (gameState != null) {
            gameState.MaybeAddPlayersScoreToBestScore();
            UpdateBestScore();
            gameState.Save();
        }
        GameOverText.SetActive(true);
    }
}
