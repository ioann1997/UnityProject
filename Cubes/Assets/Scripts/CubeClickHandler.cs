using UnityEngine;
using System.Collections.Generic;

public class CubeClickHandler : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private ExplosionHandler _explosionHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Raycaster _raycaster;
    
    private void Start()
    {
        _inputHandler.MouseLeftClick += OnMouseLeftClick;
        _raycaster.CubeHit += HandleCubeClicked;
    }
    
    private void OnDestroy()
    {
        _inputHandler.MouseLeftClick -= OnMouseLeftClick;
        _raycaster.CubeHit -= HandleCubeClicked;
    }
    
    private void OnMouseLeftClick()
    {
        _raycaster.PerformRaycast();
    }
    
    private void HandleCubeClicked(Cube clickedCube)
    {
        Vector3 currentPosition = clickedCube.transform.position;
        Vector3 currentScale = clickedCube.transform.localScale;
        float currentSplitChance = clickedCube.SplitChance;
        
        if (Random.Range(0f, 1f) < currentSplitChance)
        {
            List<Cube> newCubes = _cubeSpawner.SplitCube(
                currentPosition, 
                currentScale, 
                currentSplitChance
            );
                    
            _explosionHandler.CreateExplosion(currentPosition, newCubes);
        }
        else
        {
            _explosionHandler.CreateExhaustedCubeExplosion(currentPosition, currentScale);
        }
        
        Destroy(clickedCube.gameObject);
    }
}
