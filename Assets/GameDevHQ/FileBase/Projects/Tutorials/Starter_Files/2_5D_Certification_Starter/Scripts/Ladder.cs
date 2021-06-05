using System;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private GameObject _standPos;
    [SerializeField] private GameObject _snapPoint;
    private Player _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Ladder_Grab_Checker"))
        {

            _player = other.transform.parent.GetComponent<Player>();
            if (_player == null) return;
            _player.StartClimbLadder(this);
      
        }

        if (other.CompareTag($"Ledge_Grab_Checker"))
        {
            if (_player == null) return;
            _player.TriggerEndClimbLadder();
        }
    }

    public Vector3 GetSnapPoint() => _snapPoint.transform.position;
    public Vector3 GetStandPos() => _standPos.transform.position;
   
}
