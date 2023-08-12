using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject cloudsphere;

    public int numSpheresMin = 6;
    public int numSpheresMax = 10;

    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);

    public float scaleYMin = 2f;

    public List<GameObject> spheres;

    private void Start()
    {
        spheres = new List<GameObject>();

        CreateCloud();
    }

    private void CreateCloud()
    {
        int numSpheres = Random.Range(numSpheresMin, numSpheresMax);

        for (int i = 0; i < numSpheres; i++)
        {
            GameObject sphere = Instantiate(cloudsphere, transform);
            spheres.Add(sphere);

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            sphere.transform.localPosition = offset;

            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            sphere.transform.localScale = scale;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
    }

    private void Restart()
    {
        foreach (var sphere in spheres)
            Destroy(sphere);

        CreateCloud();
    }
}