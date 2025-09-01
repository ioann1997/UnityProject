using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolSize = 50;
    [SerializeField] private Color _cubeColor = Color.blue;
    
    private ObjectPool<Cube> _objectPool;
    private Transform _poolParent;

    private void Awake()
    {
        _poolParent = new GameObject("CubePool").transform;
        _poolParent.SetParent(transform);
        InitializePool();
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
        
        // Сбрасываем состояние куба
        cube.ResetCube();
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

    public void ReturnCube(Cube cube)
    {
        if (cube != null)
        {
            _objectPool.Release(cube);
        }
    }

    private void OnDestroy()
    {
        if (_objectPool != null)
        {
            _objectPool.Dispose();
        }
    }
}
