using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float easing;
    [SerializeField] private Vector2 minXY = Vector2.zero;

    public static GameObject POI;


    private Camera _camera;
    private float _cameraPositionZ;

    private void Awake()
    {
        _cameraPositionZ = transform.position.z;
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        if (POI is null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            if (POI.CompareTag(Constants.Projectile))
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = _cameraPositionZ;

        transform.position = destination;
        
        _camera.orthographicSize = destination.y + 10;
    }
}