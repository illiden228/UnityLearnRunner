using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private float _dashSpeedModyfier = 1.5f;
    [SerializeField] private float _growthScore = 1;
    [SerializeField] private float _startSpeed = 10f;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private ParticleSystem _runEffect;
    [SerializeField] private ParticleSystem _boomEffect;
    [SerializeField] private AudioClip _jumpAudioClip;
    [SerializeField] private AudioClip _crashAudioClip;
    private int _currentHealth;
    private float _score;
    private Animator _animator;
    private PlayerMover _mover;
    private AudioSource _audioSource;
    private bool _isAlive = true;
    private bool _isStarted = false;

    public event UnityAction<int> HealthChanged;
    public event UnityAction<int> ScoreChanged;
    public event UnityAction Died;
    public event UnityAction<float> Dash;
    public event UnityAction DashEnded;
    public event UnityAction Started;

    private void Start()
    {
        _currentHealth = _health;
        HealthChanged?.Invoke(_currentHealth);
        _mover = GetComponent<PlayerMover>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _mover.OnGround += OnPlayerGround;
        Died += _mover.OnPlayerDied;

        _runEffect.Play();
        _animator.SetBool(SimpleCharacterAnimator.Static_b, true);
        _animator.SetFloat(SimpleCharacterAnimator.Speed_f, 0.3f);
    }

    private void Update()
    {
        if(!_isStarted)
        {
            if (transform.position != _startPosition.position)
                transform.position = Vector3.MoveTowards(transform.position, _startPosition.position, _startSpeed * Time.deltaTime);
            else
            {
                _isStarted = true;
                Started?.Invoke();
            }
        }

        if(_isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _runEffect.Stop();
                PlayClip(_jumpAudioClip);
                _animator.SetTrigger(SimpleCharacterAnimator.Jump_trig);
                _animator.SetBool(SimpleCharacterAnimator.Jump_b, true);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _animator.SetFloat(SimpleCharacterAnimator.Speed_f, 0.8f);
                var main = _runEffect.main;
                main.simulationSpeed *= _dashSpeedModyfier;
                _growthScore *= _dashSpeedModyfier;
                Dash?.Invoke(_dashSpeedModyfier);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _animator.SetFloat(SimpleCharacterAnimator.Speed_f, 0.3f);
                var main = _runEffect.main;
                main.simulationSpeed /= _dashSpeedModyfier;
                _growthScore /= _dashSpeedModyfier;
                DashEnded?.Invoke();
            }
        }

        if(_isStarted)
        {
            _score += _growthScore;
            ScoreChanged?.Invoke((int) _score);
        }
    }

    private void OnDisable()
    {
        Died -= _mover.OnPlayerDied;
        _mover.OnGround -= OnPlayerGround;
    }

    private void OnPlayerGround()
    {
        _animator.SetBool(SimpleCharacterAnimator.Jump_b, false);
        _runEffect.Play();
    }

    public void TakeDamage()
    {
        _currentHealth--;
        if (_currentHealth < 1)
        {
            Death();
        }
        HealthChanged?.Invoke(_currentHealth);
    }

    public void Death()
    {
        _isAlive = false;
        PlayClip(_crashAudioClip);
        _runEffect.Stop();
        _boomEffect.Play();
        _animator.SetBool(SimpleCharacterAnimator.Death_b, true);
        Died?.Invoke();
    }

    private void PlayClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
