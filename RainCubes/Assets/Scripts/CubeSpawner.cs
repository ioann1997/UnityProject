using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10f, 18f, 10f);
    [SerializeField] private float _spawnHeight = 18f;

    [Header("Pool Reference")]
    [SerializeField] private CubePool _cubePool;

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            SpawnCube();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnCube()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();

        Cube cube = _cubePool.GetCube();
        cube.transform.position = randomPosition;
        cube.transform.rotation = Quaternion.identity;

        cube.SetCubePool(_cubePool);
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
    public void StartSpawningCubes()
    {
        StartCoroutine(SpawnRoutine());
    }
}