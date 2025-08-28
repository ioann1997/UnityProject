using UnityEngine;
using System.Collections.Generic;

public class ExplosionHandler : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float _explosionForce = 10f; 
    [SerializeField] private float _explosionRadius = 50f;
    [SerializeField] private LayerMask _affectedLayers = -1;
    
    public void CreateExplosion(Vector3 explosionCenter, List<Cube> affectedCubes)
    {
        foreach (Cube cube in affectedCubes)
        {
            if (cube == null) continue;
            
            Rigidbody rigidBody = cube.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(_explosionForce, explosionCenter, _explosionRadius, 0f, ForceMode.Impulse);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
