using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(ColorChanger))]
public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private ColorChanger _colorChanger;
    private float _lifeTime;
    private float _minlifeTime = 2f;
    private float _maxlifeTime = 5f;
    private bool _hasLandedOnPlatform = false;

    public event System.Action<Cube> OnLifeTimeExpired;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void Reset()
    {
        _hasLandedOnPlatform = false;
        _lifeTime = 0f;
        _renderer.material.color = Color.blue;

        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platformComponent))
        {
            if (_hasLandedOnPlatform == false)
            {
                OnLandedOnPlatform();
            }
        }
    }

    private void OnLandedOnPlatform()
    {
        _hasLandedOnPlatform = true;

        _colorChanger.SetRandomColor(_renderer);

        _lifeTime = Random.Range(_minlifeTime, _maxlifeTime);
        
        StartCoroutine(LifeTimeCountdown());        
    }

    private IEnumerator LifeTimeCountdown()
    {
        yield return new WaitForSeconds(_lifeTime);
        
        OnLifeTimeExpired?.Invoke(this);
    }
}