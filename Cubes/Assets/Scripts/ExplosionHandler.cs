using UnityEngine;
using System.Collections.Generic;

public class ExplosionHandler : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float _baseExplosionForce = 20f; 
    [SerializeField] private float _baseExplosionRadius = 5f;
    [SerializeField] private float _forceMultiplier = 2f;
    [SerializeField] private float _radiusMultiplier = 1.5f;
    [SerializeField] private LayerMask _affectedLayers = -1;
    
    public void CreateExplosion(Vector3 explosionCenter, List<Cube> affectedCubes)
    {
        foreach (Cube cube in affectedCubes)
        {
            if (cube == null) 
                continue;
            
            Rigidbody rigidBody = cube.Rigidbody;

            rigidBody.AddExplosionForce(_baseExplosionForce, explosionCenter, _baseExplosionRadius, 0f, ForceMode.Impulse);
            
        }
    }

    public void CreateExhaustedCubeExplosion(Vector3 explosionCenter, Vector3 cubeScale)
    {
        float cubeSize = Mathf.Max(cubeScale.x, cubeScale.y, cubeScale.z);
        float explosionRadius = _baseExplosionRadius * _radiusMultiplier / cubeSize;
        float explosionForce = _baseExplosionForce * _forceMultiplier / cubeSize;
        
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, explosionRadius, _affectedLayers);
        
        foreach (Collider collider in colliders)
        {
            if (collider.transform.position == explosionCenter) continue;
            
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();

            float distance = Vector3.Distance(explosionCenter, collider.transform.position);
            float distanceMultiplier = 1f - (distance / explosionRadius);
            distanceMultiplier = Mathf.Clamp01(distanceMultiplier);
                
            float finalForce = explosionForce * distanceMultiplier;

            rigidBody.AddExplosionForce(finalForce, explosionCenter, explosionRadius, 0f, ForceMode.Impulse);
        }
    }
}
