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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pauseGame() {
        this.pauseUI.SetActive(true);
        backgroundAudio.Pause();
    }

    public void resumeGame() {
        this.pauseUI.SetActive(false);
        backgroundAudio.Play();
    }

    void Awake() {
        // other instructions
        // subscribe to events
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
        // GameStart();
    }

    public void SetScore(int score) {
        string scoreText = "Score: " + score.ToString();
        this.gameScore.Value = score;

        this.scoreText.text = scoreText;
        this.gameOverScoreText.text = scoreText;
    }

    IEnumerator activateGameOver() {
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