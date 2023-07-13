using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        // DontDestroyOnLoad(this);
    }
    #endregion
    public GameObject mainMenuScreen;
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public int Lives { get; set; }
    public int AvailibleLives = 3;
    public bool IsMainMenu { get; set; }
    public bool IsGameStarted { get; set; }
    public static event Action<int> OnLifeLost;

    private void Start()
    {
        IsMainMenu = true;
        this.Lives = this.AvailibleLives;
        Screen.SetResolution(1920, 1080, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }
    public void NewGameClicked()
    {
        IsMainMenu = false;
        mainMenuScreen.SetActive(false);
        AudioManager.Instance.BtnAudioPlay();
    }
    public void ExitClicked()
    {
        AudioManager.Instance.BtnAudioPlay();
        Application.Quit();
    }
    public void RestartGame()
    {
        StartCoroutine("TryAgain");
    }
    IEnumerator TryAgain()
    {
        AudioManager.Instance.BtnAudioPlay();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ShowVictoryScreen()
    {
        AudioManager.Instance.VictoryAudioPlay();
        victoryScreen.SetActive(true);
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            AudioManager.Instance.LevelUpAudioPlay();
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }
    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;
            if (this.Lives < 1)
            {
                AudioManager.Instance.GameOverAudioPlay();
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLifeLost?.Invoke(this.Lives);
                AudioManager.Instance.LostLifeAudioPlay();
                // reset balls
                BallsManager.Instance.ResetBalls();
                // stop the game
                IsGameStarted = false;
                // reload the level
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }
    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
}
