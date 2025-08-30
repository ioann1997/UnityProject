using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [Header("Cube Properties")]
    [SerializeField] private float _splitChance = 1f; 
    [SerializeField] private float _scaleMultiplier = 0.5f; 

    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private ColorChanger _colorChanger;

    public Rigidbody Rigidbody
    {
        get { return _rigidbody; }
    }

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
        _colorChanger = GetComponent<ColorChanger>();
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
        _colorChanger = gameObject.AddComponent<ColorChanger>();
        _colorChanger.SetRandomColor(_renderer);
    }
}