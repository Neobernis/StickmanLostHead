using System;
using UnityEngine;

public class RigidBodySleep : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>()?.Sleep();
    }
}
