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
    [SerializeField] private int _poolSize = 3;

    private ObjectPool<Cube> _objectPool;
    private WaitForSeconds _spawnWait;
    
    private void Awake()
    {
        InitializePool();
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    public void StartSpawningCubes()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            _objectPool.Get();
           
            yield return _spawnWait;
        }
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
            maxSize: _poolSize
        );
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);

        return cube;
    }

    private void OnCubeGet(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.transform.position = GetRandomSpawnPosition();
        
        cube.LifeTimeExpired += OnCubeLifeTimeExpired;
    }

    private void OnCubeRelease(Cube cube)
    {
        cube.LifeTimeExpired -= OnCubeLifeTimeExpired;       
        cube.gameObject.SetActive(false);

        cube.Reset();
    }

    private void OnCubeDestroy(Cube cube)
    {
        if (cube != null)
        {
            Destroy(cube.gameObject);
        }
    }

    private void OnCubeLifeTimeExpired(Cube cube)
    {
        cube.LifeTimeExpired -= OnCubeLifeTimeExpired;

        _objectPool.Release(cube);
    }
}