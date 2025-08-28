using UnityEngine;
using UnityEngine.UIElements;

public class ColorChanger : MonoBehaviour
{
    private void Start()
    {
    }
    public void SetRandomColor(Renderer renderer)
    {
        if (renderer != null)
        {
            renderer.material.color = Random.ColorHSV();
        }
    }
}
