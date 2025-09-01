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

    public void StartCubeRain()
    {
        if (_cubeSpawner != null)
        {
            _cubeSpawner.StartSpawningCubes();
            Debug.Log("Дождь кубов запущен!");
        }
    }
}
