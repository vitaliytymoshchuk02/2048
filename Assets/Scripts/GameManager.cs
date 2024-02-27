using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Gameplay gameplay;
    public GameObject gameOverCanvas;
    public GameObject finishCanvas;
    public GameObject exit;
    public GameObject confirmExit;
    public GameObject confirmNewGame;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    public static GameManager Instance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bestScoreText.text = LoadBestScore().ToString();
    }
    public void Restart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        StartCoroutine(Fade(gameOverCanvas.GetComponent<CanvasGroup>(), 1f, 1f, 0.5f));
    }

    public void Finish()
    {
        finishCanvas.SetActive(true);
        gameplay.enabled = false;
        StartCoroutine(Fade(finishCanvas.GetComponent<CanvasGroup>(), 1f, 1f, 0.5f));
    }

    public void FadeIn(GameObject go)
    {
        go.SetActive(true);
        StartCoroutine(Fade(go.GetComponent<CanvasGroup>(), 1f, 0f, 0.1f));
    }

    public void FadeOut(GameObject go)
    {
        StartCoroutine(Fade(go.GetComponent<CanvasGroup>(), 0f, 0f, 0.1f));
        go.SetActive(false);
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }
    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        SaveBestScore();
    }

    public void SaveBestScore()
    {
        int bestScore = LoadBestScore();

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScore", score);
        }
    }

    public int LoadBestScore()
    {
        return PlayerPrefs.GetInt("bestScore", 0);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
                Application.Quit();
    }
}
