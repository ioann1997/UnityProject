using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private LayerMask _clickableLayers = -1; // слои, на которые можно кликать
    [SerializeField] private Camera _mainCamera; // основная камера для raycast
    
    public static InputHandler Instance { get; private set; }
    
    // События для других компонентов
    public System.Action<GameObject> OnCubeClicked;
    public System.Action<Vector3> OnWorldPositionClicked;
    
    private void Awake()
    {
        // Singleton pattern для единственного экземпляра
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Если камера не назначена, находим основную
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }
    
    private void Update()
    {
        HandleMouseInput();
    }
    
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
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
            
            // Проверяем, является ли объект кубом
            Cube cube = clickedObject.GetComponent<Cube>();
            if (cube != null)
            {
                // Уведомляем о клике по кубу
                OnCubeClicked?.Invoke(clickedObject);
            }
            
            // Уведомляем о клике по позиции в мире
            OnWorldPositionClicked?.Invoke(hit.point);
        }
    }
}
