using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Transform _allPlacespoint;
    private Transform[] _arrayPlaces;
    private int _currentPlaceIndex;

    private void Start()
    {
        _arrayPlaces = new Transform[_allPlacespoint.childCount];

        for (int i = 0; i < _allPlacespoint.childCount; i++)
            _arrayPlaces[i] = _allPlacespoint.GetChild(i);
    }
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