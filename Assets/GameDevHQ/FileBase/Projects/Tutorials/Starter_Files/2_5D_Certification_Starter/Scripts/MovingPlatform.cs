using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private float _moveSpeed;
    private int _currentWaypoint;
    private bool _holding;
 
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_wayPoints == null) return;
        
        if (_holding) return;
        
        transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_currentWaypoint].transform.position,
            _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(_wayPoints[_currentWaypoint].transform.position, transform.position) <= 0)
        {
            _currentWaypoint++;
            StartCoroutine(HoldAtLocation());
        }

        if (_currentWaypoint != _wayPoints.Count) return;
        _wayPoints.Reverse();
        _currentWaypoint = 0;
    }

    private IEnumerator HoldAtLocation()
    {
        _holding = true;
        yield return new WaitForSeconds(5f);
        _holding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        other.transform.parent = null;
    }
}
