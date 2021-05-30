using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _lives;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    private Vector3 _direction;
    private CharacterController _controller;
    private Vector3 _velocity;
    private float _yVelocity;
    private Rigidbody _rigidbody;
    private Animator _anim;
    private bool _isJumping;
    private bool _isHanging;
    private Ledge _activeLedge;
    private static readonly int GrabLedge = Animator.StringToHash("GrabLedge");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jump = Animator.StringToHash("Jumping");
    private static readonly int ClimbUp = Animator.StringToHash("ClimbUp");


    // Start is called before the first frame update
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("Player does not have a controller component");
        }

        _anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (_controller.enabled)
        {
            HandleMovePlayer();
        }

        if (_isHanging && Input.GetKeyDown(KeyCode.E))
        {
            _anim.SetTrigger(ClimbUp);
            _isHanging = false;
        }
    }

    private void HandleMovePlayer()
    {

        if (_controller.isGrounded)
        {
            _direction = new Vector3(0,0,Input.GetAxisRaw("Horizontal") * _speed);
            _anim.SetFloat(Speed, Mathf.Abs(_direction.z));

            if (_isJumping)
            {
                _isJumping = false;
                _anim.SetBool(Jump, _isJumping);
            }

            if (_direction.z != 0f)
            {
                var facing = transform.localEulerAngles;
                facing.y = _direction.z > 0 ? 0 : 180;
                transform.eulerAngles = facing;
            }
            
            if (Input.GetButtonDown("Jump"))
            {
                _direction.y += _jumpHeight;
                _isJumping = true;
               _anim.SetBool(Jump, _isJumping);
               
            }
        }
        _direction.y -= _gravity * Time.deltaTime;
        _controller.Move(_direction * Time.deltaTime);
    }

    public void LedgeGrab(Vector3 snapPoint, Ledge currentLedge)
    {
        _controller.enabled = false;
        transform.position = snapPoint;
        _isJumping = false;
        _anim.SetBool(Jump, _isJumping);
        _anim.SetFloat(Speed, 0.0f);
        _isHanging = true;
        _anim.SetBool(GrabLedge, _isHanging);
        _activeLedge = currentLedge;
    }

    public void ClimbEdgeComplete()
    {
        transform.position = _activeLedge.GetStandPos();
        _anim.SetBool(GrabLedge, false);
        _controller.enabled = true;
    }

}
