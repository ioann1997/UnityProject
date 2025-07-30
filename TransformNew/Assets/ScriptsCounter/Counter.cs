using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Counter : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    private int _currentNumber;
    private Coroutine _counterCotoutine = null;
    private float delay = 0.5f;

    public int CurrentNumber => _currentNumber;
    public event Action<int> ValueChanged;

    private void Start()
    {
        _currentNumber = 0;

        if (_inputHandler != null)
        {
            _inputHandler.ButtonPressed += OnButtonPressed;
        }
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.ButtonPressed -= OnButtonPressed;
        }
    }

    private void OnButtonPressed()
    {
        if (_counterCotoutine == null)
        {
            _counterCotoutine = StartCoroutine(CountUp(delay));
        }

        else
        {
            StopCoroutine(_counterCotoutine);
            _counterCotoutine = null;
        }
    }

    private IEnumerator CountUp(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            _currentNumber++;
            ValueChanged?.Invoke(_currentNumber);
            yield return wait;
        }
    }

}
