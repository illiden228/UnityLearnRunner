using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBack : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 30;
    protected bool IsAlive = false;
    private float _speedModyfier = 1;

    protected void Move()
    {
        if(IsAlive)
            transform.Translate(Vector3.right * _moveSpeed * _speedModyfier * Time.deltaTime);
    }

    public void Starting()
    {
        IsAlive = true;
    }

    public void OnPlayerDied()
    {
        IsAlive = false;
    }

    public void OnPlayerDash(float speedModyfier)
    {
        _speedModyfier = speedModyfier;
    }

    public void OnPlayerDashEnded()
    {
        _speedModyfier = 1;
    }
}
