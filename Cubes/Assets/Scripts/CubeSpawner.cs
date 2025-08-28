using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _cubePrefab; // префаб куба для создания
    [SerializeField] private float _spawnInterval = 2f; // интервал между созданием кубов
    [SerializeField] private int _maxCubesOnScene = 10; // максимальное количество кубов на сцене
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(18f, 18f, 18f); // размер области спавна (чуть меньше комнаты 20x20x20)
    
    [Header("Cube Properties")]
    [SerializeField] private float _minScale = 3f; // минимальный размер куба
    [SerializeField] private float _maxScale = 3f; // максимальный размер куба (фиксированный размер 3x3x3)
    [SerializeField] private bool _randomizeRotation = true; // случайный поворот куба
    [SerializeField] private float _splitChanceReduction = 0.5f; // уменьшение шанса разделения для новых кубов
    
    [Header("Split Settings")]
    [SerializeField] private int _minSplitCubes = 2; // минимальное количество кубов при разделении
    [SerializeField] private int _maxSplitCubes = 6; // максимальное количество кубов при разделении
    
    private bool _isSpawning = false;
    private int _currentCubeCount = 0;
    private List<GameObject> _spawnedCubes = new List<GameObject>();
    
    // Публичное свойство для получения количества кубов
    public int CurrentCubeCount => _currentCubeCount;
    
    private void Start()
    {
        // Проверяем настройки
        if (_cubePrefab == null)
        {
            Debug.LogError("CubeSpawner: Cube Prefab не назначен! Назначьте префаб куба в инспекторе.");
            return;
        }
        
        // Проверяем, что у префаба есть компонент Cube
        Cube cubeComponent = _cubePrefab.GetComponent<Cube>();
        if (cubeComponent == null)
        {
            Debug.LogError("CubeSpawner: У назначенного префаба нет компонента Cube! Проверьте префаб.");
            return;
        }
        
        Debug.Log($"CubeSpawner: Начинаем спавн. Интервал: {_spawnInterval}, Макс кубов: {_maxCubesOnScene}");
        Debug.Log($"CubeSpawner: Префаб куба: {_cubePrefab.name}");
        
        // Начинаем спавн кубов
        StartSpawning();
    }
    
    private void StartSpawning()
    {
        if (!_isSpawning)
        {
            _isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }
    
    private void StopSpawning()
    {
        _isSpawning = false;
        StopAllCoroutines();
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (_isSpawning)
        {
            if (_currentCubeCount < _maxCubesOnScene)
            {
                SpawnCube();
            }
            
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    
    private void SpawnCube()
    {
        // Генерируем случайную позицию в области спавна
        Vector3 randomPosition = GetRandomSpawnPosition();
        
        Debug.Log($"CubeSpawner: Создаем куб в позиции {randomPosition}");
        
        // Создаем куб
        GameObject newCube = Instantiate(_cubePrefab, randomPosition, GetRandomRotation());
        
        // Получаем компонент Cube
        Cube cubeComponent = newCube.GetComponent<Cube>();
        if (cubeComponent != null)
        {
            // Устанавливаем случайный размер
            float randomScale = Random.Range(_minScale, _maxScale);
            cubeComponent.SetScale(Vector3.one * randomScale);
            
            // Устанавливаем случайный цвет
            cubeComponent.SetRandomColor();
            
            Debug.Log($"CubeSpawner: Куб создан с размером {randomScale}");
        }
        else
        {
            Debug.LogWarning("CubeSpawner: У созданного куба нет компонента Cube!");
        }
        
        // Увеличиваем счетчик кубов
        _currentCubeCount++;
        _spawnedCubes.Add(newCube);
        
        Debug.Log($"CubeSpawner: Всего кубов на сцене: {_currentCubeCount}");
        
        // Подписываемся на уничтожение куба
        StartCoroutine(MonitorCubeDestruction(newCube));
    }
    
    // Метод для создания кубов при разделении
    public List<GameObject> SpawnSplitCubes(Vector3 position, Vector3 originalScale, float originalSplitChance, int count)
    {
        List<GameObject> newCubes = new List<GameObject>();
        
        for (int i = 0; i < count; i++)
        {
            // Создаем куб
            GameObject newCube = Instantiate(_cubePrefab, position, Quaternion.identity);
            
            // Получаем компонент Cube
            Cube cubeComponent = newCube.GetComponent<Cube>();
            if (cubeComponent != null)
            {
                // Устанавливаем размер меньше оригинального
                Vector3 newScale = originalScale * cubeComponent.ScaleMultiplier;
                cubeComponent.SetScale(newScale);
                
                // Уменьшаем шанс разделения
                float newSplitChance = originalSplitChance * _splitChanceReduction;
                cubeComponent.SetSplitChance(newSplitChance);
                
                // Устанавливаем случайный цвет
                cubeComponent.SetRandomColor();
            }
            
            // Добавляем случайное смещение
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
            newCube.transform.position = position + randomOffset;
            
            newCubes.Add(newCube);
            _currentCubeCount++;
            _spawnedCubes.Add(newCube);
            
            // Подписываемся на уничтожение куба
            StartCoroutine(MonitorCubeDestruction(newCube));
        }
        
        return newCubes;
    }
    
    // Метод для разделения куба (новая логика)
    public List<GameObject> SplitCube(Vector3 position, Vector3 originalScale, float originalSplitChance)
    {
        // Определяем случайное количество новых кубов
        // Random.Range для int: [min, max) - не включает max, поэтому используем max + 1
        int cubeCount = Random.Range(_minSplitCubes, _maxSplitCubes + 1);
        
        Debug.Log($"CubeSpawner: Разделяем куб на {cubeCount} кубов (диапазон: {_minSplitCubes}-{_maxSplitCubes})");
        
        // Создаем новые кубы
        return SpawnSplitCubes(position, originalScale, originalSplitChance, cubeCount);
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = transform.position;
        Vector3 randomOffset = new Vector3(
            Random.Range(-_spawnAreaSize.x * 0.5f, _spawnAreaSize.x * 0.5f),
            Random.Range(-_spawnAreaSize.y * 0.5f, _spawnAreaSize.y * 0.5f),
            Random.Range(-_spawnAreaSize.z * 0.5f, _spawnAreaSize.z * 0.5f)
        );
        
        return center + randomOffset;
    }
    
    private Quaternion GetRandomRotation()
    {
        if (_randomizeRotation)
        {
            return Quaternion.Euler(
                Random.Range(0f, 360f),
                Random.Range(0f, 360f),
                Random.Range(0f, 360f)
            );
        }
        
        return Quaternion.identity;
    }
    
    private IEnumerator MonitorCubeDestruction(GameObject cube)
    {
        // Ждем, пока куб не будет уничтожен
        yield return new WaitUntil(() => cube == null);
        
        // Уменьшаем счетчик кубов
        _currentCubeCount--;
        _spawnedCubes.Remove(cube);
    }
    
    // Метод для ручного создания куба
    public void SpawnCubeManually()
    {
        if (_currentCubeCount < _maxCubesOnScene)
        {
            SpawnCube();
        }
    }
    
    // Метод для очистки всех кубов на сцене
    public void ClearAllCubes()
    {
        foreach (GameObject cube in _spawnedCubes.ToArray())
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }
        _spawnedCubes.Clear();
        _currentCubeCount = 0;
    }
    
    // Визуализация области спавна в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _spawnAreaSize);
    }
    
    // Методы для управления спавном
    public void SetSpawnInterval(float newInterval)
    {
        _spawnInterval = Mathf.Max(0.1f, newInterval);
    }
    
    public void SetMaxCubes(int newMax)
    {
        _maxCubesOnScene = Mathf.Max(1, newMax);
    }
}
