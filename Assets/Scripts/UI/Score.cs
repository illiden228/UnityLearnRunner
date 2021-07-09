using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text _mainScoreText;
    private Player _player;

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _player.ScoreChanged += OnPlayerScoreChanged;
    }

    private void OnDisable()
    {
        _player.ScoreChanged -= OnPlayerScoreChanged;
    }

    private void OnPlayerScoreChanged(int score)
    {
        _mainScoreText.text = score.ToString();
    }
}
