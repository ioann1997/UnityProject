using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Counter : MonoBehaviour
{
    private float _currentNumber;

    public float CurrentNumber => _currentNumber;

    public event Action<int> ValueChanged;

    private void Start()
    {
        _currentNumber = 0;
    }
}
