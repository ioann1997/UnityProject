using UnityEngine;
using UnityEngine.UIElements;

public class Spin : MonoBehaviour
{

    [SerializeField] private int _speed;

   private void Update()
    {
        transform.Rotate(Vector3.up * _speed * Time.deltaTime);
    }
}
