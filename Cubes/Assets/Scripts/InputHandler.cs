using UnityEngine;

public class InputHandler : MonoBehaviour
{
    const int LeftMouseButton = 0;
    
    public event System.Action MouseLeftClick;
    
    private void Update()
    {
        HandleMouseInput();
    }
    
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            MouseLeftClick?.Invoke();
        }
    }
}
