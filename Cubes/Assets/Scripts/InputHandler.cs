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
        HandleKeyboardInput();
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
    
    private void HandleKeyboardInput()
    {
        // Обработка клавиатуры
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Пробел - создать куб вручную
            CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
            if (spawner != null)
            {
                spawner.SpawnCubeManually();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            // C - очистить все кубы
            CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
            if (spawner != null)
            {
                spawner.ClearAllCubes();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Escape - выход из игры
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
    // Метод для проверки, находится ли позиция в пределах экрана
    public bool IsPositionOnScreen(Vector3 worldPosition)
    {
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(worldPosition);
        return screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
               screenPoint.y >= 0 && screenPoint.y <= Screen.height &&
               screenPoint.z > 0;
    }
    
    // Метод для получения позиции мыши в мировых координатах
    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        
        return Vector3.zero;
    }
}
