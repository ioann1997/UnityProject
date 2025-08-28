using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(18f, 18f, 18f);
    
    [Header("Cube Properties")]
    [SerializeField] private float _minScale = 3f;
    [SerializeField] private float _maxScale = 3f;
    [SerializeField] private bool _randomizeRotation = true;
    [SerializeField] private float _splitChanceReduction = 0.5f;
    
    [Header("Split Settings")]
    [SerializeField] private int _minSplitCubes = 2;
    [SerializeField] private int _maxSplitCubes = 6;
    
    private bool _isSpawning = false;
    private int _countStartCube = 3;
    private List<Cube> _spawnedCubes = new List<Cube>();
    
    private void Start()
    {
        if (_cubePrefab == null)
        {
            Debug.LogError("CubeSpawner: Cube Prefab не назначен! Назначьте префаб куба в инспекторе.");
            return;
        }
        
        Cube cubeComponent = _cubePrefab.GetComponent<Cube>();
        if (cubeComponent == null)
        {
            Debug.LogError("CubeSpawner: У назначенного префаба нет компонента Cube! Проверьте префаб.");
            return;
        }
        
        Debug.Log($"CubeSpawner: Начинаем спавн. Интервал: {_spawnInterval}");
        Debug.Log($"CubeSpawner: Префаб куба: {_cubePrefab.name}");
        
        StartSpawning();
    }
    
    private void StartSpawning()
    {
        if (_isSpawning == false)
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
        for (int i = 0; i < _countStartCube; i++)
        {
                SpawnCube();
            
            yield return new WaitForSeconds(_spawnInterval);
        }
        StopSpawning();
    }
    
    private void SpawnCube()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();
        
        Debug.Log($"CubeSpawner: Создаем куб в позиции {randomPosition}");
        
        Cube cube = Instantiate(_cubePrefab, randomPosition, GetRandomRotation());

        float randomScale = Random.Range(_minScale, _maxScale);
        cube.Initialize(Vector3.one * randomScale);

        cube.SetRandomColor();
            
        Debug.Log($"CubeSpawner: Куб создан с размером {randomScale}");

        _spawnedCubes.Add(cube);
    }
    
    public List<Cube> SpawnSplitCubes(Vector3 position, Vector3 originalScale, float originalSplitChance, int count)
    {
        List<Cube> cubes = new List<Cube>();
        
        for (int i = 0; i < count; i++)
        {
            Cube cube = Instantiate(_cubePrefab, position, Quaternion.identity);
            
            Cube cubeComponent = cube.GetComponent<Cube>();
            if (cubeComponent != null)
            {
                Vector3 newScale = originalScale * cubeComponent.ScaleMultiplier;
                float newSplitChance = originalSplitChance * _splitChanceReduction;
                cubeComponent.Initialize(newSplitChance, cubeComponent.ScaleMultiplier, newScale);
                
                cubeComponent.SetRandomColor();
            }
            
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
            cube.transform.position = position + randomOffset;
            
            cubes.Add(cube);
            _spawnedCubes.Add(cube);
        }
        
        return cubes;
    }
    
    public List<Cube> SplitCube(Vector3 position, Vector3 originalScale, float originalSplitChance)
    {
        int cubeCount = Random.Range(_minSplitCubes, _maxSplitCubes + 1);
        
        Debug.Log($"CubeSpawner: Разделяем куб на {cubeCount} кубов (диапазон: {_minSplitCubes}-{_maxSplitCubes})");
        
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
    
    public void ClearAllCubes()
    {
        foreach (Cube cube in _spawnedCubes.ToArray())
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }
        _spawnedCubes.Clear();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _spawnAreaSize);
    }
}
