using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _targetX = 5f;
    [SerializeField] private float _targetY = 0f;

    private Vector2 _currentPosition;
    private Vector2 _targetPosition;
    private Vector2 _startPosition;
    private bool _movingToTarget = true;

    private void Start()
    {
        _currentPosition = transform.position;
        _startPosition = _currentPosition;
        GenerateTargetPosition();
    }

    private void FixedUpdate()
    {
        Vector2 direction = (_targetPosition - _currentPosition).normalized;
        _currentPosition += direction * _moveSpeed * Time.fixedDeltaTime;
        transform.position = _currentPosition;

        Vector2 difference = _targetPosition - _currentPosition;

        if (difference.sqrMagnitude < 0.01f)
            {
            if (_movingToTarget)
            {
                _targetPosition = _startPosition;
                _movingToTarget = false;
            }
            else
            {
                GenerateTargetPosition();
                _movingToTarget = true;
            }
        }
    }

    public Vector2 CurrentPosition
    {
        get => transform.position;
        private set => transform.position = value;
    }

    private void GenerateTargetPosition()
    {
        _targetPosition = new Vector2(_currentPosition.x + _targetX, _currentPosition.y + _targetY);
    }
}

