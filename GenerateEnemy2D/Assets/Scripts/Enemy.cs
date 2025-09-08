using System.Collections;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _lifeTime = 6f;
    
    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    
    private Vector2 _direction;
    private Target _target;
    private Rigidbody2D _rigidbody2D;

    public event Action<Enemy> LifeTimeExpired;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(CountdownLifeTime());
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = _target.CurrentPosition;
        Vector2 currentPosition = _rigidbody2D.position;
        _direction = (targetPosition - currentPosition).normalized;
        
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direction * Time.fixedDeltaTime * _speed);
    }

    public void SetTarget(Target target)
    {
        _target = target;
    }

    private IEnumerator CountdownLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);

        LifeTimeExpired?.Invoke(this);
    }
}
