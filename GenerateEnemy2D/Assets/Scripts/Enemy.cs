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

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direction * Time.fixedDeltaTime * _speed);
    }

    private IEnumerator CountdownLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);

        LifeTimeExpired?.Invoke(this);
    }
}
