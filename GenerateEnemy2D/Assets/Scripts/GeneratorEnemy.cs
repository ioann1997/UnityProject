using UnityEngine;
using System.Collections;

public class GeneratorEnemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpawnerEnemy[] _spawnerEnemies;

    [Header("Game Settings")]
    [SerializeField] private bool _autoStartSpawning = true;
    [SerializeField] private float _spawnInterval = 2f;

    private WaitForSeconds _spawnWait;

    private void Start()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);
        
        if (_autoStartSpawning)
        {
            StartCoroutine(GenerateEnemyRoutine());
        }
    }

    private IEnumerator GenerateEnemyRoutine()
    {
        while (enabled)
        {
            SpawnerEnemy randomSpawner = GetRandomSpawner();
            randomSpawner.SpawnSingleEnemy();

            yield return _spawnWait;
        }
    }

    private SpawnerEnemy GetRandomSpawner()
    {
        int randomIndex = Random.Range(0, _spawnerEnemies.Length);
        return _spawnerEnemies[randomIndex];
    }
}
