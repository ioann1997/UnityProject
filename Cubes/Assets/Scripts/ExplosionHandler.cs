using UnityEngine;
using System.Collections.Generic;

public class ExplosionHandler : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float _explosionForce = 10f; 
    [SerializeField] private float _explosionRadius = 50f;
    [SerializeField] private LayerMask _affectedLayers = -1; // слои, на которые действует взрыв
    
    public void CreateExplosion(Vector3 explosionCenter, List<GameObject> affectedObjects)
    {
        foreach (GameObject obj in affectedObjects)
        {
            if (obj == null) continue;
            
            // Проверяем, находится ли объект в нужном слое
            if (((1 << obj.layer) & _affectedLayers) == 0) continue;
            
            // Вычисляем направление взрыва
            Vector3 direction = (obj.transform.position - explosionCenter).normalized;
            float distance = Vector3.Distance(obj.transform.position, explosionCenter);
            
            // Вычисляем силу взрыва в зависимости от расстояния
            float forceMultiplier = 1f - (distance / _explosionRadius);
            forceMultiplier = Mathf.Clamp01(forceMultiplier);
            
            // Применяем силу к объекту
            Cube cube = obj.GetComponent<Cube>();
            if (cube != null)
            {
                Vector3 explosionForce = direction * _explosionForce * forceMultiplier;
                cube.AddForce(explosionForce, ForceMode.Impulse);
            }
        }
    }
    
    public void CreateExplosionAtPosition(Vector3 explosionCenter)
    {
        // Находим все объекты в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, _explosionRadius, _affectedLayers);
        
        foreach (Collider collider in colliders)
        {
            if (collider == null) continue;
            
            // Вычисляем направление взрыва
            Vector3 direction = (collider.transform.position - explosionCenter).normalized;
            float distance = Vector3.Distance(collider.transform.position, explosionCenter);
            
            // Вычисляем силу взрыва в зависимости от расстояния
            float forceMultiplier = 1f - (distance / _explosionRadius);
            forceMultiplier = Mathf.Clamp01(forceMultiplier);
            
            // Применяем силу к объекту
            Cube cube = collider.GetComponent<Cube>();
            if (cube != null)
            {
                Vector3 explosionForce = direction * _explosionForce * forceMultiplier;
                cube.AddForce(explosionForce, ForceMode.Impulse);
            }
        }
    }
    
    // Визуализация радиуса взрыва в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
