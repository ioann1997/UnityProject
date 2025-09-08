using UnityEngine;
using UnityEngine.Pool;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _poolSize = 10;

    [Header("Target Settings")]
    [SerializeField] private Target _target;

    private ObjectPool<Enemy> _objectPool;

    private void Awake()
    {
        InitializePool();
    }

    public void SpawnSingleEnemy()
    {
        if (_objectPool != null)
        {
            _objectPool.Get();
        }
    }

    private void InitializePool()
    {
        _objectPool = new ObjectPool<Enemy>(
            createFunc: CreateEnemy,
            actionOnGet: OnEnemyGet,
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolSize,
            maxSize: _poolSize
        );
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab);
        enemy.transform.position = transform.position; 

        return enemy;
    }

    private void OnEnemyGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.transform.position = transform.position;

        enemy.SetTarget(_target);

        enemy.LifeTimeExpired += OnEnemyLifeTimeExpired;
    }

    private void OnEnemyLifeTimeExpired(Enemy enemy)
    {
        enemy.LifeTimeExpired -= OnEnemyLifeTimeExpired;

        _objectPool.Release(enemy);
    }
}