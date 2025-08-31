using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask _clickableLayers = -1;
    [SerializeField] private Camera _mainCamera;
    
    public event System.Action<Cube> CubeHit;
    
    public void PerformRaycast()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickableLayers))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            if (hitObject.TryGetComponent<Cube>(out Cube cube))
            {
                CubeHit?.Invoke(cube);
            }
        }
    }
}
