using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Obstacle : MoveBack
{
    private float _destroyPositionX;
    public event UnityAction<Obstacle> Destroyed;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage();
        }
    }

    public void Init(float destroyPositionX)
    {
        _destroyPositionX = destroyPositionX;
    }

    private void Update() 
    {
        Move();

        if(transform.position.x > _destroyPositionX)
        {
            Destroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
