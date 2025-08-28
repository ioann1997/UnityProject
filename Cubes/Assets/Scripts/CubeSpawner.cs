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

    [Header("Initial Spawn Settings")]
    [SerializeField] private int _countStartCube = 3;

    private float _randomOffsetRange = 1f;
    private float _randomSpawnAreaSize = 0.5f;
    private float _maxRotationAngle = 360f;

    private bool _isSpawning = false;
    private List<Cube> _spawnedCubes = new List<Cube>();

    private void Start()
    {
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

        Cube cube = Instantiate(_cubePrefab, randomPosition, GetRandomRotation());

        float randomScale = Random.Range(_minScale, _maxScale);
        cube.Initialize(Vector3.one * randomScale);

        cube.SetRandomColor();

        _spawnedCubes.Add(cube);
    }

    public List<Cube> SpawnSplitCubes(Vector3 position, Vector3 originalScale, float originalSplitChance, int count)
    {
        List<Cube> cubes = new List<Cube>();

        for (int i = 0; i < count; i++)
        {
            Cube cube = Instantiate(_cubePrefab, position, Quaternion.identity);

            Vector3 newScale = originalScale * cube.ScaleMultiplier;
            float newSplitChance = originalSplitChance * _splitChanceReduction;
            cube.Initialize(newSplitChance, cube.ScaleMultiplier, newScale);

            cube.SetRandomColor();

            Vector3 randomOffset = new Vector3(
                Random.Range(-_randomOffsetRange, _randomOffsetRange),
                Random.Range(-_randomOffsetRange, _randomOffsetRange),
                Random.Range(-_randomOffsetRange, _randomOffsetRange)
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

        return SpawnSplitCubes(position, originalScale, originalSplitChance, cubeCount);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = transform.position;
        Vector3 randomOffset = new Vector3(
            Random.Range(-_spawnAreaSize.x * _randomSpawnAreaSize, _spawnAreaSize.x * _randomSpawnAreaSize),
            Random.Range(-_spawnAreaSize.y * _randomSpawnAreaSize, _spawnAreaSize.y * _randomSpawnAreaSize),
            Random.Range(-_spawnAreaSize.z * _randomSpawnAreaSize, _spawnAreaSize.z * _randomSpawnAreaSize)
        );

        return center + randomOffset;
    }

    private Quaternion GetRandomRotation()
    {
        if (_randomizeRotation)
        {
            return Quaternion.Euler(
                Random.Range(0f, _maxRotationAngle),
                Random.Range(0f, _maxRotationAngle),
                Random.Range(0f, _maxRotationAngle)
            );
        }

        return Quaternion.identity;
    }
}