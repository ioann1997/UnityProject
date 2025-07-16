using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private Animator _timerAnimator;
    [SerializeField] private AnimationClip _timerClip;

    private float _delay = 0.5f;
    private int _counter = 0;
    private Coroutine _counterCoroutine = null;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

        while (_counter >=0 )
        {
            DisplayCountUp(_counter);
            _counter++;
            yield return wait;
        }
    }

    private void DisplayCountUp(int count)
    {
        _counterText.text = count.ToString("");
    }
}
