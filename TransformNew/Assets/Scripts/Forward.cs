using UnityEngine;
using UnityEngine.UIElements;

public class Forward : MonoBehaviour
{

    [SerializeField] private int _speed;

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
