using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(0)]
public class GameManager: Singleton<GameManager> {
    public IntVariable gameScore;

    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent gamePause;
    public UnityEvent gameResume;

    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    private int score = 0;
    private bool paused = false;

    void Start()
    {
        // reset score
        gameScore.Value = 0;

        // increase score by 1
        // gameScore.ApplyChange(1);

        // invoke score change event with current score to update HUD
        scoreChange.Invoke(gameScore.Value);

        gameStart.Invoke();
        Time.timeScale = 1.0f;

        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadMainMenu() {
        SceneManager.LoadSceneAsync(
            "MainMenu", LoadSceneMode.Single
        );
    }

    public void SceneSetup(Scene current, Scene next) {
        gameStart.Invoke();
        SetScore(score);
    }

    public void PauseGame () {
        Time.timeScale = 0.0f;
        this.paused = true;
        gamePause.Invoke();
    }

    public void ResumeGame () {
        Time.timeScale = 1.0f;
        this.paused = true;
        gameResume.Invoke();
    }

    public void GameRestart() {
        // reset score
        score = 0;
        SetScore(score);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment) {
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score) {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        // Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}