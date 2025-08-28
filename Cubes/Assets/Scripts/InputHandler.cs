using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private LayerMask _clickableLayers = -1;
    [SerializeField] private Camera _mainCamera;
    
    public System.Action<GameObject> OnCubeClicked;
    public System.Action<Vector3> OnWorldPositionClicked;
    
    private void Update()
    {
        HandleMouseInput();
    }
    
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }
    
    private void HandleMouseClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickableLayers))
        {
            GameObject clickedObject = hit.collider.gameObject;
            
            Cube cube = clickedObject.GetComponent<Cube>();
            if (cube != null)
            {
                OnCubeClicked?.Invoke(clickedObject);
            }
            
            OnWorldPositionClicked?.Invoke(hit.point);
        }
    }
}
