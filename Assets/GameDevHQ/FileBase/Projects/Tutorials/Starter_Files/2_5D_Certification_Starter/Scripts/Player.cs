using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _lives;
    [SerializeField] private float _ladderClimbSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    [SerializeField] private int _toolCount;
    private Vector3 _direction;
    private CharacterController _controller;
    private Vector3 _velocity;
    private float _yVelocity;
    private Animator _anim;
    private bool _isJumping;
    private bool _isHanging;
    private bool _isClimbingLadder;
    private Ledge _activeLedge;
    private Ladder _activeLadder;
    
    private static readonly int GrabLedge = Animator.StringToHash("GrabLedge");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jump = Animator.StringToHash("Jumping");
    private static readonly int ClimbUp = Animator.StringToHash("ClimbUp");
    private static readonly int ClimbLadderStart = Animator.StringToHash("ClimbLadder");
    private static readonly int EndClimbingLadder = Animator.StringToHash("EndClimbingLadder");


    // Start is called before the first frame update
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("Player does not have a controller component");
        }

        _anim = GetComponentInChildren<Animator>();
        _toolCount = 0;
    }

   
    private void Update()
    {
        if (_controller.enabled && !_isClimbingLadder) HandleMovePlayer();
        
        if (_isClimbingLadder) HandleLadderClimbMovement();

        if (_isHanging && Input.GetKeyDown(KeyCode.E))
        {
            _anim.SetTrigger(ClimbUp);
            _isHanging = false;
        }
    }

    private void HandleLadderClimbMovement()
    {
        if (!_controller.enabled) return;
        var vInput = Input.GetAxis("Vertical");
        _direction = new Vector3(0, vInput, 0);
        _anim.SetFloat(Speed, Mathf.Abs(vInput));
        _controller.Move(_direction * (_ladderClimbSpeed * Time.deltaTime));
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
                var playerTransform = transform;
                var facing = playerTransform.localEulerAngles;
                facing.y = _direction.z > 0 ? 0 : 180;
                playerTransform.eulerAngles = facing;
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

    public void LedgeGrab(Ledge currentLedge)
    {
        _activeLedge = currentLedge;
        _controller.enabled = false;
        transform.position = _activeLedge.GetHandSnapPoint();
        _isJumping = false;
        _anim.SetBool(Jump, _isJumping);
        _anim.SetFloat(Speed, 0.0f);
        _isHanging = true;
        _anim.SetBool(GrabLedge, _isHanging);
    }
    
    public void ClimbEdgeComplete()
    {
        transform.position = _activeLedge.GetStandPos();
        _anim.SetBool(GrabLedge, false);
        _controller.enabled = true;
    }
    public void CollectTool()
    {
        _toolCount++;
        UI_Manager.Instance.UpdateToolsCountUI(_toolCount);
    }

    public void StartClimbLadder(Ladder currentLadder)
    {
        _isClimbingLadder = true;
        _activeLadder = currentLadder;
        transform.position = _activeLadder.GetSnapPoint();
        _isJumping = false;
        _anim.SetBool(Jump, _isJumping);
        _anim.SetFloat(Speed, 0.0f);
        _anim.SetTrigger(ClimbLadderStart);
    }
    public void TriggerEndClimbLadder()
    {
        _controller.enabled = false;
        _anim.SetBool(EndClimbingLadder, _isClimbingLadder);
        _isClimbingLadder = false;
    }

    public void EndClimbLadder() 
    {
        _anim.SetBool(EndClimbingLadder, _isClimbingLadder);
        transform.position = _activeLadder.GetStandPos();
        _controller.enabled = true;
       _anim.SetFloat(Speed, 0.0f);
    }


}
