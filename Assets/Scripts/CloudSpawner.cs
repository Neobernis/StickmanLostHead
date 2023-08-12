using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudSpawner : MonoBehaviour
{
    public int numberClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPositionMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPositionMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1f;
    public float cloudScaleMax = 3f;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] _cloudInstances;

    private void Awake()
    {
        _cloudInstances = new GameObject[numberClouds];
        GameObject anchor = GameObject.Find(Constants.CloudAnchor);

        GameObject cloud;
        for (int i = 0; i < numberClouds; i++)
        {
            cloud = Instantiate(cloudPrefab, anchor.transform);
            Vector3 cloudPosition = Vector3.zero;
            cloudPosition.x = Random.Range(cloudPositionMin.x, cloudPositionMax.x);
            cloudPosition.y = Random.Range(cloudPositionMin.y, cloudPositionMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            cloudPosition.y = Mathf.Lerp(cloudPositionMin.y, cloudPositionMax.y, scaleU);
            cloudPosition.z = 100 - 90 * scaleU;

            cloud.transform.position = cloudPosition;
            cloud.transform.localScale = Vector3.one * scaleVal;

            _cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        foreach (var cloud in _cloudInstances)
        {
            float scaleValue = cloud.transform.localScale.x;
            Vector3 cloudPosition = cloud.transform.position;

            cloudPosition.x -= scaleValue * Time.deltaTime * cloudSpeedMult;

            if (cloudPosition.x <= cloudPositionMin.x) 
                cloudPosition.x = cloudPositionMax.x;

            cloud.transform.position = cloudPosition;
        }
    }
}
