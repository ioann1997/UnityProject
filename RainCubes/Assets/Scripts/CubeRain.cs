using UnityEngine;

public class CubeRain : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CubeSpawner _cubeSpawner;

    [Header("Game Settings")]
    [SerializeField] private bool _autoStartSpawning = true;

    private void Start()
    {
        if (_autoStartSpawning)
        {
            StartCubeRain();
        }
    }

    private void StartCubeRain()
    {
         _cubeSpawner.StartSpawningCubes();
    }
}
