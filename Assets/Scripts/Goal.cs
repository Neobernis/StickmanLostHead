using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static bool goalMet = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(Constants.Projectile))
        {
            goalMet = true;
            
            var material = GetComponent<Renderer>().material;
            var color = material.color;
            color.a = 1;
            material.color = color;
        }
    }
}
