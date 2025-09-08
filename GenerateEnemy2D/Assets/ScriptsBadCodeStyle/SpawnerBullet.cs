using System.Collections;
using UnityEngine;

public class SpawnerBullet : MonoBehaviour
{

    [SerializeField] private float _shootingSpeed;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _shootingInterval;
    
    private Transform ObjectToShoot;
    private WaitForSeconds _shootingWait;

    private void Start()
    {
        _shootingWait = new WaitForSeconds(_shootingInterval);
        StartCoroutine(ShootingWorker());
    }

    private IEnumerator ShootingWorker()
    {
        while (enabled)
        {
            Vector3 direction = (ObjectToShoot.position - transform.position).normalized;
            Bullet newBullet = Instantiate(_bulletPrefab, transform.position + direction, Quaternion.identity);

            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            bulletRigidbody.transform.up = direction;
            bulletRigidbody.AddForce(direction * _shootingSpeed, ForceMode.VelocityChange);

            yield return _shootingWait;
        }
    }
}