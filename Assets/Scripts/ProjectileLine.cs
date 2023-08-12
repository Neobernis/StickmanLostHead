using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine Single;

    public float minDistance = 0.1f;

    private LineRenderer _line;
    private GameObject _poi;

    private List<Vector3> _points;

    public Vector3 LastPoint
    {
        get
        {
            if (_points is null)
            {
                return Vector3.zero;
            }

            return _points[^1];
        }
    }

    public GameObject Poi
    {
        get => _poi;
        set
        {
            _poi = value;
            if (_poi != null)
            {
                _line.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    private void Awake()
    {
        Single = this;

        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        if (Poi is null)
        {
            if (FollowCamera.POI is not null)
            {
                if (FollowCamera.POI.CompareTag("Projectile"))
                {
                    Poi = FollowCamera.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        AddPoint();

        if (FollowCamera.POI is null)
        {
            Poi = null;
        }
    }

    private void AddPoint()
    {
        Vector3 point = _poi.transform.position;

        if (_points.Count > 0 && (point - LastPoint).magnitude < minDistance)
        {
            return;
        }

        if (_points.Count == 0)
        {
            Vector3 launchPosDiff = point - Slingshot.LaunchPos;
            _points.Add(point + launchPosDiff);
            _points.Add(point);
            _line.positionCount = 2;

            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);

            _line.enabled = true;
        }
        else
        {
            _points.Add(point);
            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, LastPoint);
            _line.enabled = true;
        }
    }

    public void Clear()
    {
        _poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }
}