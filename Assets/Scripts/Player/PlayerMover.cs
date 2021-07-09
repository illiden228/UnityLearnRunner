using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityScale;
    [SerializeField] private int _jumps = 2;
    private Rigidbody _playerRigidbody;
    private Vector3 _initialGravity;
    private bool _isOnGround = true;
    private bool _isAlive = true;
    private int _jumpsLeft;
    public event UnityAction OnGround;
    
    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _jumpsLeft = _jumps;
        _initialGravity = Physics.gravity;
        Physics.gravity *= _gravityScale;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && (_isOnGround || _jumpsLeft != 0) && _isAlive)
        {
            _playerRigidbody.velocity = Vector3.zero;
            _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _jumpsLeft--;
            _isOnGround = false;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isOnGround = true;
        _jumpsLeft = _jumps;
        OnGround?.Invoke();
    }

    public void OnPlayerDied()
    {
        _isAlive = false;
        Physics.gravity = _initialGravity;
    }
}
