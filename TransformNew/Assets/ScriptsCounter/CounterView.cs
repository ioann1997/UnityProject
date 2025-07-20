using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CounterView : MonoBehaviour
{
    const int LeftButtonMouse = 0;

    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private Animator _timerAnimator;
    [SerializeField] private AnimationClip _timerClip;

    private Counter _counter;
    private float _delay = 0.5f;
    private int _Currentcounter = 0;
    private Coroutine _counterCoroutine = null;


    private void OnEnabled()
    {
        _counter.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _counter.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(int count)
    {
        _counterText.text = count.ToString("");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftButtonMouse))
        {
            if(_timerAnimator != null)
            {
                _timerAnimator.Play(_timerClip.name);
            }

        if (_counterCoroutine == null)
            {
                _counterCoroutine = StartCoroutine(CountUp(_delay));
            }
            else
            {
                StopCoroutine(_counterCoroutine);
                _counterCoroutine = null;
            }
        }
    }

    private IEnumerator CountUp(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            OnValueChanged(_Currentcounter);
            _Currentcounter++;
            Debug.Log(_counter.CurrentNumber.ToString());
            yield return wait;
        }
    }
}
