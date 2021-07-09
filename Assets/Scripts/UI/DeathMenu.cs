using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject _panelDeathMenu;
    [SerializeField] private TMP_Text _mainScoreText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private Button _restartButton;
    private Player _player;

    private void Start()
    {
        _panelDeathMenu.SetActive(false);
        _player = FindObjectOfType<Player>();
        _player.Died += OnPlayerDied;
        _restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnDisable()
    {
        _player.Died -= OnPlayerDied;
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);
    }

    private void OnPlayerDied()
    {
        _panelDeathMenu.SetActive(true);
        _mainScoreText.gameObject.SetActive(false);
        int score = int.Parse(_mainScoreText.text);
        int bestScore = score;
        _scoreText.text = _mainScoreText.text;
        if(PlayerPrefs.HasKey("Score"))
        {
            int previousScore = PlayerPrefs.GetInt("Score");
            if (previousScore > bestScore)
            {
                bestScore = previousScore;
                PlayerPrefs.SetInt("Score", bestScore);
            }
        }
        else
            PlayerPrefs.SetInt("Score", bestScore);

        PlayerPrefs.Save();
        _bestScoreText.text = bestScore.ToString();
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
