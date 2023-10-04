using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager: MonoBehaviour {
    public GameObject gameOverUI;
    public GameObject normalUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore(int score) {
        string scoreText = "Score: " + score.ToString();
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
    }

    public void GameOver() {
        gameOverUI.SetActive(true);
        normalUI.SetActive(false);
    }
}