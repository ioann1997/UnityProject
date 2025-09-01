using UnityEngine;
using UnityEngine.UIElements;

public class ColorChanger : MonoBehaviour
{
    public void SetRandomColor(Renderer renderer)
    {
        if (renderer != null)
        {
            renderer.material.color = Random.ColorHSV();
        }
    }
}
