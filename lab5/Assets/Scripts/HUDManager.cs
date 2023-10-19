using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class HUDManager: MonoBehaviour {
    public GameObject gameOverUI;
    public GameObject normalUI;
    public GameObject pauseUI;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject highscoreText;
    public IntVariable gameScore;

    public AudioSource backgroundAudio;
    
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        string scoreText = "Score: " + this.gameScore.Value.ToString();
        this.scoreText.text = scoreText;
        this.gameOverScoreText.text = scoreText;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pauseGame()
    {
        Debug.Log("PAUSE");
        this.pauseUI.SetActive(true);
        this.backgroundAudio.Pause();
        
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void resumeGame()
    {
        this.pauseUI.SetActive(false);
        // Debug.Log("BACKGROUND-PLAY");
        this.backgroundAudio.UnPause();
        // Debug.Log("BACKGROUND-PLAY-END");
        
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    void Awake()
    {
        this.GameStart();
        // other instructions
        // subscribe to events
        /*
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
        */
        // GameStart();
    }

    public void IncrementScore(int increment)
    {
        Debug.Log("INCREMENT " + increment);
        this.gameScore.SetValue(increment + this.gameScore.Value);
        string scoreText = "Score: " + this.gameScore.Value.ToString();
        this.scoreText.text = scoreText;
        this.gameOverScoreText.text = scoreText;
        Debug.Log("INCREMENT_END " + this.gameScore.Value);
    }

    public void SetScore(int score) {
        string scoreText = "Score: " + score.ToString();
        this.gameScore.Value = score;

        this.scoreText.text = scoreText;
        this.gameOverScoreText.text = scoreText;
    }

    IEnumerator activateGameOver()
    {
        Debug.Log("ACTIVATE_GAME_OVER");
        gameOverUI.SetActive(true);
        normalUI.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0.0f;
    }

    public void GameStart() {
        // hide gameover panel
        gameOverUI.SetActive(false);
        normalUI.SetActive(true);
        this.resumeGame();

        backgroundAudio.Stop();
        backgroundAudio.Play();
    }

    public void GameRestart()
    {
        this.GameStart();
        this.gameScore.SetValue(0);
        this.IncrementScore(0);
    }

    public void GameOver() {
        gameOverUI.SetActive(true);
        normalUI.SetActive(false);

        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = (
            "TOP- " + gameScore.previousHighestValue.ToString("D6")
        );

        // show
        highscoreText.SetActive(true);
    }
}