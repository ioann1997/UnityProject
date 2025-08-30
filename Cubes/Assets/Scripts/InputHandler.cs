using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private LayerMask _clickableLayers = -1;
    [SerializeField] private Camera _mainCamera;
    
    public event System.Action<Cube> CubeClicked;
    public event System.Action<Vector3> WorldPositionClicked;
    
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
            
            if (clickedObject.TryGetComponent<Cube>(out Cube cube))
            {
                CubeClicked?.Invoke(cube);
            }
            
            WorldPositionClicked?.Invoke(hit.point);
        }
    }
}
