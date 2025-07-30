using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab; // Префаб куба для создания новых
    [SerializeField] private float _minScale = 0.5f; // Минимальный размер (половина от исходного)
    [SerializeField] private float _currentSplitChance = 1f; // Минимальный размер (половина от исходного)

    private int _minCreateCube = 2;
    private int _maxCreateCube = 6;
    private List<GameObject> _createdCubes = new List<GameObject>();
    private float _explosionForce = 100f;
    private float _explosionRadius = 50f;

    private void OnMouseDown()
    {
        if (Random.Range(0, 1f) < _currentSplitChance)
        {
            int cubeCount = Random.Range(_minCreateCube, _maxCreateCube + 1);

            CreateNewCubes(cubeCount);

            ApplyExplosionForce();
        }

        Destroy(gameObject);
    }

    private void CreateNewCubes(int count)
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentScale = transform.localScale;

        for (int i = 0; i < count; i++)
        {
            // Создаем новый куб
            GameObject newCube = Instantiate(_cubePrefab, currentPosition, Quaternion.identity);

            // Уменьшаем размер в два раза
            Vector3 newScale = currentScale * _minScale;
            newCube.transform.localScale = newScale;


            Renderer renderer = newCube.GetComponent<Renderer>();
            renderer.material.color = GetRandomColor();

            // Случайно перемещаем куб вокруг исходной позиции
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
            newCube.transform.position = currentPosition + randomOffset;

            CubeController cubeController = newCube.GetComponent<CubeController>();

            cubeController._currentSplitChance = _currentSplitChance *= 0.5f;

            _createdCubes.Add(newCube);
        }
    }

    private void ApplyExplosionForce()
    {
        Vector3 explosionCenter = transform.position;

        foreach (GameObject cube in _createdCubes)
        {
            Vector3 direction = (cube.transform.position - explosionCenter).normalized;
            float distance = Vector3.Distance(cube.transform.position, explosionCenter);

            float forceMultiplier = 1f - (distance / _explosionRadius);
            forceMultiplier = Mathf.Clamp01(forceMultiplier);

            Rigidbody rb = cube.GetComponent<Rigidbody>();
            rb.AddForce(direction * _explosionForce *  forceMultiplier, ForceMode.Impulse);
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

   
