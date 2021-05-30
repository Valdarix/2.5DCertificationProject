using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] private Vector3 _snapPoint;

    [SerializeField]
    private Vector3 _standPos;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag($"Ledge_Grab_Checker")) return;

        var player = other.transform.parent.GetComponent<Player>();

        if (player != null)
        {
            player.LedgeGrab(_snapPoint, this);
            
        }
    }

    public Vector3 GetStandPos() => _standPos;
}
