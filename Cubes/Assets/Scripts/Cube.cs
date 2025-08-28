using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("Cube Properties")]
    [SerializeField] private float _splitChance = 1f; // шанс разделения куба
    [SerializeField] private float _scaleMultiplier = 0.5f; // множитель размера при разделении
    
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    
    public float SplitChance => _splitChance;
    public float ScaleMultiplier => _scaleMultiplier;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }
    
    public void SetSplitChance(float newChance)
    {
        _splitChance = Mathf.Clamp01(newChance);
    }
    
    public void SetScaleMultiplier(float newMultiplier)
    {
        _scaleMultiplier = Mathf.Clamp01(newMultiplier);
    }
    
    public void SetRandomColor()
    {
        if (_renderer != null)
        {
            _renderer.material.color = GetRandomColor();
        }
    }
    
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    
    public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        if (_rigidbody != null)
        {
            _rigidbody.AddForce(force, forceMode);
        }
    }
    
    private Color GetRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            1f
        );
    }
}
