using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    const int LeftButtonMouse = 0;

    [SerializeField] private Animator _timerAnimator;
    [SerializeField] private AnimationClip _timerClip;

    public event Action ButtonPressed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftButtonMouse))
        {
            if (_timerAnimator != null && _timerClip != null)
            {
                _timerAnimator.Play(_timerClip.name);
            }

            ButtonPressed?.Invoke();
        }
    }
}