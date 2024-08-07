using System;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int _initialPlayerLives = 3;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentPlayerLives;
    private float _currentScore = 0f;

    private void Awake()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _currentPlayerLives = _initialPlayerLives;
        UpdateLives();
        UpdateScore();

    }

    public void ProcessPlayerDeath()
    {
        if (_currentPlayerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersists();
        SceneLoader.LoadFirstScene();
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        _currentPlayerLives--;
        UpdateLives();
        SceneLoader.ReloadScene();
    }

    private void UpdateLives()
    {
        _livesText.text = _currentPlayerLives.ToString();
    }

    public void AddScore(float coinCost)
    {
        if (coinCost > 0)
        {
            _currentScore += coinCost;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        _scoreText.text = _currentScore.ToString();
    }
}
