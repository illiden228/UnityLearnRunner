using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Obstacle> _obstacles;
    [SerializeField] private float _timeBetweenSpawm;
    [SerializeField] private float _obstacleDestroyPositionX;
    private Player _player;
    private float _timer;
    private float _randomTimeBetweenSpawn;
    private bool _isAlive = false;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _player.Died += OnPlayerDied;
        _player.Started += OnPlayerStarted;
        _randomTimeBetweenSpawn = _timeBetweenSpawm;
    }

    private void Update()
    {
        if(_isAlive)
        {
            _timer += Time.deltaTime;
            if (_timer > _randomTimeBetweenSpawn)
            {
                Spawn();
                _randomTimeBetweenSpawn = _timeBetweenSpawm + Random.Range(-0.5f, 0.5f);
                _timer = 0;
            }
        }
    }

    private void OnDisable()
    {
        _player.Died -= OnPlayerDied;
        _player.Started -= OnPlayerStarted;
    }

    private void Spawn()
    {
        GameObject randomObstacle = _obstacles[Random.Range(0, _obstacles.Count)].gameObject;
        Obstacle newObstacle = Instantiate(randomObstacle, transform.position, Quaternion.identity).GetComponent<Obstacle>();
        newObstacle.Init(_obstacleDestroyPositionX);
        newObstacle.Destroyed += OnObstacleDestroyed;
        newObstacle.Starting();
        _player.Died += newObstacle.OnPlayerDied;
        _player.Dash += newObstacle.OnPlayerDash;
        _player.DashEnded += newObstacle.OnPlayerDashEnded;
    }

    private void OnObstacleDestroyed(Obstacle obstacle)
    {
        obstacle.Destroyed -= OnObstacleDestroyed;
        _player.Died -= obstacle.OnPlayerDied;
        _player.Dash -= obstacle.OnPlayerDash;
        _player.DashEnded -= obstacle.OnPlayerDashEnded;
    }

    private void OnPlayerStarted()
    {
        _isAlive = true;
    }

    private void OnPlayerDied()
    {
        _isAlive = false;
    }
}
