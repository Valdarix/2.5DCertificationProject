using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] private GameObject _snapPoint;
    [SerializeField] private GameObject _standPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag($"Ledge_Grab_Checker")) return;

        var player = other.transform.parent.GetComponent<Player>();

        if (player != null)
        {
            player.LedgeGrab(this);
        }
    }

    public Vector3 GetStandPos() => _standPos.transform.position;
    public Vector3 GetHandSnapPoint() => _snapPoint.transform.position;
}
