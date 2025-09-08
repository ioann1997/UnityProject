using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InstantiateBulletsShooting : MonoBehaviour
{

    [SerializeField] private float _shootingSpeed;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootingInterval;
    
    public Transform ObjectToShoot;

    private void Start()
    {
        StartCoroutine(ShootingWorker());
    }
    private IEnumerator ShootingWorker()
    {
        while (enabled)
        {
            Vector3 direction = (ObjectToShoot.position - transform.position).normalized;
            GameObject newBullet = Instantiate(_bulletPrefab, transform.position + direction, Quaternion.identity);

            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            bulletRigidbody.transform.up = direction;
            bulletRigidbody.AddForce(direction * _shootingSpeed, ForceMode.VelocityChange);

            yield return new WaitForSeconds(_shootingInterval);
        }
    }
}