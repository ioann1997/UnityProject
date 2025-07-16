using UnityEngine;

public class ScaleIncrease : MonoBehaviour
{
    [SerializeField] private float _scaleChange;

    private void Update()
    {
        transform.localScale += new Vector3(_scaleChange, _scaleChange, _scaleChange) ;
    }
}
