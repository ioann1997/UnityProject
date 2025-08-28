using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("Cube Properties")]
    [SerializeField] private float _splitChance = 1f; // шанс разделения куба
    [SerializeField] private float _scaleMultiplier = 0.5f; // множитель размера при разделении
    
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    
    // Свойства с контролируемым доступом
    public float SplitChance 
    { 
        get => _splitChance; 
        private set => _splitChance = Mathf.Clamp01(value); 
    }
    
    public float ScaleMultiplier 
    { 
        get => _scaleMultiplier; 
        private set => _scaleMultiplier = Mathf.Clamp01(value); 
    }
    
    public Vector3 Scale 
    { 
        get => transform.localScale; 
        private set => transform.localScale = value; 
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }
    
    
    public void Initialize(Vector3 scale)
    {
        Scale = scale;
    }
    
    public void Initialize(float splitChance, float scaleMultiplier, Vector3 scale)
    {
        SplitChance = splitChance;
        ScaleMultiplier = scaleMultiplier;
        Scale = scale;
    }
    
    public void SetRandomColor()
    {
        if (_renderer != null)
        {
            _renderer.material.color = Random.ColorHSV();
        }
    }
    
    public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        if (_rigidbody != null)
        {
            _rigidbody.AddForce(force, forceMode);
        }
    }
}
