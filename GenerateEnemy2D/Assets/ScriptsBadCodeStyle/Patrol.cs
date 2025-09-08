using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _allPlacespoint;

    private Transform[] _arrayPlaces;
    private int _currentPlaceIndex;

    private void Update()
    {
        Transform currentPoint = _arrayPlaces[_currentPlaceIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, _moveSpeed * Time.deltaTime);

        if (transform.position == currentPoint.position)
            NextPlaceTakerLogic();
    }
    private void NextPlaceTakerLogic()
    {
        _currentPlaceIndex++;

        if (_currentPlaceIndex == _arrayPlaces.Length)
            _currentPlaceIndex = 0;

        Vector3 nextPoint = _arrayPlaces[_currentPlaceIndex].position;
        transform.forward = nextPoint - transform.position;
    }
}