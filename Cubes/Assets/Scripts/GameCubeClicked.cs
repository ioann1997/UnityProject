using UnityEngine;
using System.Collections.Generic;

public class GameCubeClicked : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private ExplosionHandler _explosionHandler;
    [SerializeField] private InputHandler _inputHandler;
    
    private void Start()
    {
            _inputHandler.OnCubeClicked += HandleCubeClicked;
    }
    
    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnCubeClicked -= HandleCubeClicked;
        }
    }
    
    private void HandleCubeClicked(GameObject clickedCube)
    {
        Cube cubeComponent = clickedCube.GetComponent<Cube>();
        if (cubeComponent == null) return;
        
        if (Random.Range(0f, 1f) < cubeComponent.SplitChance)
        {
            if (_cubeSpawner != null)
            {
                Vector3 currentPosition = clickedCube.transform.position;
                Vector3 currentScale = clickedCube.transform.localScale;
                float currentSplitChance = cubeComponent.SplitChance;
                
                List<Cube> newCubes = _cubeSpawner.SplitCube(
                    currentPosition, 
                    currentScale, 
                    currentSplitChance
                );
                
                if (_explosionHandler != null)
                {
                    _explosionHandler.CreateExplosion(currentPosition, newCubes);
                }
            }
        }
        
        Destroy(clickedCube);
    }
}
