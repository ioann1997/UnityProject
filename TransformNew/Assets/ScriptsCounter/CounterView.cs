using System;
using TMPro;
using UnityEngine;

public class CounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private Counter _counter;

    private void Start()
    {
        if (_counter != null)
        {
            _counter.ValueChanged += OnValueChanged;
        }
    }

    private void OnDestroy()
    {
        if (_counter != null)
        {
            _counter.ValueChanged -= OnValueChanged;
        }
    }

    private void OnValueChanged(int count)
    {
        _counterText.text = count.ToString();
    }
}
