using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    public delegate void ScoreUpdater(int scoreValue);
    public event ScoreUpdater ScoreUpdate;

    private TextMeshProUGUI scoreText;
    private int currentScore;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        currentScore = 0;
        ScoreUpdate += UpdateScore;
        UpdateScore(0);
    }

    private void OnEnable()
    {
        ScoreUpdate += UpdateScore;
    }
    private void OnDisable()
    {
        ScoreUpdate -= UpdateScore;
    }

    public void InvokeScoreUpdate(int ScoreValue)
    {
        ScoreUpdate?.Invoke(ScoreValue);
    }

    private void UpdateScore(int scoreValue)
    {
        currentScore += scoreValue;
        StaticData.score = currentScore;
        scoreText.text = currentScore.ToString();
    }
}
