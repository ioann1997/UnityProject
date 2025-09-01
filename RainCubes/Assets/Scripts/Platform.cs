using UnityEngine;


public class Platform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private bool _isPlatform = true;
    
    public bool IsPlatform => _isPlatform;
}
