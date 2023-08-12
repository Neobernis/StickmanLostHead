using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    
    private GameObject _launchPoint;
    private Vector3 _launchPosition;
    private GameObject _projectile;
    private bool _aimingMode;
    private Camera _camera;

    private Rigidbody _projectileRigidbody;

    private static Slingshot _single;

    private void Awake()
    {
        _single = this;
        Transform launchPointTransform = transform.Find(Constants.LaunchPoint);
        _launchPoint = launchPointTransform.gameObject;
        _launchPoint.SetActive(false);
        _launchPosition = launchPointTransform.position;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_aimingMode == false)
            return;

        var mousePosition2D = Input.mousePosition;
        mousePosition2D.z = -_camera.transform.position.z;
        Vector3 mousePosition3D = _camera.ScreenToWorldPoint(mousePosition2D);

        Vector3 mouseDelta = mousePosition3D - _launchPosition;
        float maxMagnitude = GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projectilePosition = _launchPosition + mouseDelta;
        _projectile.transform.position = projectilePosition;

        if (Input.GetButtonUp(Constants.Fire1))
            Fire(mouseDelta);

        GameStateMachine.ShotFired();
        ProjectileLine.Single.Poi = _projectile;
    }

    public static Vector3 LaunchPos
    {
        get
        {
            if (_single is null)
                return Vector3.zero;
            return _single._launchPosition;
        }
    }

    private void OnMouseEnter()
    {
        _launchPoint.SetActive(true);
        Debug.Log("Slingshot:OnMouseEnter");
    }

    private void OnMouseExit()
    {
        _launchPoint.SetActive(false);
        Debug.Log("Slingshot:OnMouseExit");
    }

    private void OnMouseDown()
    {
        _aimingMode = true;
        _projectile = Instantiate(prefabProjectile, _launchPosition, Quaternion.identity);
        _projectileRigidbody = _projectile.GetComponent<Rigidbody>();
        _projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Fire(Vector3 mouseDelta)
    {
        ProjectileLine.Single.Clear();

        _aimingMode = false;
        _projectileRigidbody.isKinematic = false;
        _projectileRigidbody.velocity = -mouseDelta * velocityMult;
        FollowCamera.POI = _projectile;
        _projectile = null;
    }
}