using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10f, 18f, 10f);
    [SerializeField] private float _spawnHeight = 18f;

    [Header("Pool Settings")]
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolSize = 50;
    [SerializeField] private Color _cubeColor = Color.blue;

    private ObjectPool<Cube> _objectPool;
    private Transform _poolParent;

    private WaitForSeconds _spawnWait;
    
    private void Awake()
    {
        _poolParent = new GameObject("CubePool").transform;
        _poolParent.SetParent(transform);
        InitializePool();
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            SpawnCube();
            yield return _spawnWait;
        }
    }

    private void SpawnCube()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();

        Cube cube = GetCube();
        cube.transform.position = randomPosition;
        cube.transform.rotation = Quaternion.identity;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = transform.position;
        Vector3 randomOffset = new Vector3(
            Random.Range(-_spawnAreaSize.x, _spawnAreaSize.x),
            _spawnHeight,
            Random.Range(-_spawnAreaSize.z, _spawnAreaSize.z)
        );

        return center + randomOffset;
    }

    private void InitializePool()
    {
        _objectPool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: OnCubeGet,
            actionOnRelease: OnCubeRelease,
            actionOnDestroy: OnCubeDestroy,
            collectionCheck: true,
            defaultCapacity: _poolSize,
            maxSize: _poolSize * 2
        );
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab, _poolParent);
        cube.gameObject.SetActive(false);

        return cube;
    }

    private void OnCubeGet(Cube cube)
    {
        cube.gameObject.SetActive(true);
    }

    private void OnCubeRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.transform.SetParent(_poolParent);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;

        cube.Reset();
    }

    private void OnCubeDestroy(Cube cube)
    {
        if (cube != null)
        {
            Destroy(cube.gameObject);
        }
    }

    public Cube GetCube()
    {
        return _objectPool.Get();
    }

    public void StartSpawningCubes()
    {
        StartCoroutine(SpawnRoutine());
    }
}