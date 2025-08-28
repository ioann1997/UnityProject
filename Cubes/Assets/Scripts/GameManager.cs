using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private ExplosionHandler _explosionHandler;
    [SerializeField] private InputHandler _inputHandler;
    
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        // Singleton pattern для единственного экземпляра
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Если компоненты не назначены, находим их автоматически
        if (_cubeSpawner == null)
        {
            _cubeSpawner = FindObjectOfType<CubeSpawner>();
        }
        
        if (_explosionHandler == null)
        {
            _explosionHandler = FindObjectOfType<ExplosionHandler>();
        }
        
        if (_inputHandler == null)
        {
            _inputHandler = FindObjectOfType<InputHandler>();
        }
    }
    
    private void Start()
    {
        // Подписываемся на события InputHandler
        if (_inputHandler != null)
        {
            _inputHandler.OnCubeClicked += HandleCubeClicked;
            Debug.Log("GameManager: Подключен к InputHandler");
        }
        else
        {
            Debug.LogError("GameManager: InputHandler не найден!");
        }
    }
    
    private void OnDestroy()
    {
        // Отписываемся от событий
        if (_inputHandler != null)
        {
            _inputHandler.OnCubeClicked -= HandleCubeClicked;
        }
    }
    
    private void HandleCubeClicked(GameObject clickedCube)
    {
        Cube cubeComponent = clickedCube.GetComponent<Cube>();
        if (cubeComponent == null) return;
        
        // Проверяем шанс разделения
        if (Random.Range(0f, 1f) < cubeComponent.SplitChance)
        {
            // Разделяем куб через спавнер
            if (_cubeSpawner != null)
            {
                Vector3 currentPosition = clickedCube.transform.position;
                Vector3 currentScale = clickedCube.transform.localScale;
                float currentSplitChance = cubeComponent.SplitChance;
                
                // Создаем новые кубы через спавнер
                List<GameObject> newCubes = _cubeSpawner.SplitCube(
                    currentPosition, 
                    currentScale, 
                    currentSplitChance
                );
                
                // Создаем взрыв через взрыватель
                if (_explosionHandler != null)
                {
                    _explosionHandler.CreateExplosion(currentPosition, newCubes);
                }
            }
        }
        
        // Уничтожаем оригинальный куб
        Destroy(clickedCube);
    }
    
    // Публичные методы для управления игрой
    public void StartGame()
    {
        if (_cubeSpawner != null)
        {
            // Игра начинается автоматически при старте CubeSpawner
            Debug.Log("Игра началась!");
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Игра приостановлена");
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Игра возобновлена");
    }
    
    public void RestartGame()
    {
        // Очищаем все кубы
        if (_cubeSpawner != null)
        {
            _cubeSpawner.ClearAllCubes();
        }
        
        // Сбрасываем время
        Time.timeScale = 1f;
        
        Debug.Log("Игра перезапущена");
    }
    
    // Метод для получения статистики
    public int GetCurrentCubeCount()
    {
        if (_cubeSpawner != null)
        {
            return _cubeSpawner.CurrentCubeCount;
        }
        return 0;
    }
}
