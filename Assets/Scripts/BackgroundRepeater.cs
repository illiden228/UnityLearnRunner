using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    [SerializeField] private List<Background> _backgrounds;
    private Vector3 _startPosition;
    private Vector3 _nextPosition;
    private Background _currentBackground;
    private int _currentBackgroundIndex = 0;
    private int _backBackgroundIndex = 0;
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _player.Started += OnPlayerStart;
        foreach (var back in _backgrounds)
        {
            _player.Died += back.OnPlayerDied;
            _player.Dash += back.OnPlayerDash;
            _player.DashEnded += back.OnPlayerDashEnded;
        }

        _startPosition = _backgrounds[0].transform.position;
        _nextPosition = _backgrounds[_backgrounds.Count - 1].transform.position;
        NextBackground();
    }

    private void OnDisable()
    {
        _player.Started -= OnPlayerStart;
        foreach (var back in _backgrounds)
        {
            _player.Died -= back.OnPlayerDied;
            _player.Dash -= back.OnPlayerDash;
            _player.DashEnded -= back.OnPlayerDashEnded;
        }
    }

    void Update()
    {
        if(_currentBackground.transform.position.x >= _startPosition.x - 0.3f)
        {
            DisplaceBackground();
        }
    }

    private void DisplaceBackground()
    {
        _backgrounds[_backBackgroundIndex].transform.position  = _nextPosition;
        NextBackground();
    }

    private void NextBackground()
    {
        _backBackgroundIndex = _currentBackgroundIndex;
        _currentBackgroundIndex++;

        if(_currentBackgroundIndex == _backgrounds.Count)
            _currentBackgroundIndex = 0;

        _currentBackground = _backgrounds[_currentBackgroundIndex];
    }

    private void OnPlayerStart()
    {
        foreach (var back in _backgrounds)
        {
            back.Starting();
        }
    }
}
