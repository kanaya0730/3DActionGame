using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public IPlayerState CurrentPlayerState => _stateData[_currentPlayerStateId];

    public PlayerAction InputAction => _inputActions;

    public Animator Anim => _anim;
    public Rigidbody Rb => _rb;
    
    public float Speed => _speed;
    public float AttackWaitTime => _attackWaitTime;
    public float SkillWaitTIme => _skillWaitTime;
    public Vector2 MoveInput => _moveInput;
    
    
    public ParticleSystem ParticleAttack => _particleAttack;
    public ParticleSystem[] SkillEfect => _skillEfect;

    public bool JumpInput => _jumpInput;
    public bool AttackInput => _attackInput;
    public bool RunInput => _runInput;
    
    
    
    private PlayerAction _inputActions;
    private Animator _anim;
    private Rigidbody _rb;
    
    [SerializeField]
    private float _speed = 3f;

    private Vector2 _moveInput;
    
    [SerializeField] 
    private float _attackWaitTime;

    [SerializeField] 
    private float _skillWaitTime;
    
    private bool _jumpInput;
    private bool _attackInput;
    private bool _runInput;
    
    [SerializeField]
    [Header("攻撃エフェクト")]
    private ParticleSystem _particleAttack;
    
    [SerializeField]
    [Header("効果エフェクト")]
    private ParticleSystem[] _skillEfect;

    private PlayerStateId _currentPlayerStateId;
    
    public Dictionary<PlayerStateId, IPlayerState> _stateData = new();

    public enum PlayerStateId
    {
        None,
        Stand,
        Walk,
        Run,
        Jump,
        Attack,
        PowerUpSkill,
        SpeedUpSkill,
        
    }
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        
        _inputActions = new PlayerAction();
        _inputActions.Enable();
        
        _inputActions.Player.Attack.performed += context => Attack(context,1);
        _inputActions.Player.Jump.performed += context => Jump(context);
        _inputActions.Player.Skill0.performed += context => PowerUpSkill(context,3);
        _inputActions.Player.Skill1.performed += context => SpeedUpSkill(context,3);

        _currentPlayerStateId = PlayerStateId.None;
        _stateData.Add(PlayerStateId.Walk,new WalkState());
        _stateData.Add(PlayerStateId.Run, new RunState());
        _stateData.Add(PlayerStateId.Jump, new JumpState());
        _stateData.Add(PlayerStateId.Stand,new StandState());
        _stateData.Add(PlayerStateId.Attack,new AttackState());
        ChangeState(PlayerStateId.Stand);
        
        this.FixedUpdateAsObservable()
            .Subscribe(_ => OnMove())
            .AddTo(this);
        
        this.UpdateAsObservable()
            .Subscribe(_ => CountTime())
            .AddTo(this);
        
    }
    
    public void ChangeState(PlayerStateId nextState)
    {
        if (_currentPlayerStateId == nextState)
        {
            return;
        }

        _currentPlayerStateId = nextState;

        CurrentPlayerState.OnStart(this);
    }

    
    private void OnMove()   
    {
        _moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        /*Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * _moveInput.y + Camera.main.transform.right * _moveInput.x;
 
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        _rb.velocity = moveForward * _speed + new Vector3(0, _rb.velocity.y, 0);
        
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) 
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }*/

        CurrentPlayerState.OnUpData(this);
        Debug.Log(_currentPlayerStateId);
        Debug.Log(_jumpInput);
    }

    public void Attack( InputAction.CallbackContext context, float time)
    {
        /*if (_attackWaitTime <= 0)
        {
            _attackInput = context.ReadValueAsButton();
            _particleAttack.Play();
            _attackWaitTime = time;
        }*/
        _attackInput = context.ReadValueAsButton();
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValueAsButton();
    }

    public void PowerUpSkill(InputAction.CallbackContext context, float time)
    {
        if (_skillWaitTime <= 0)
        {
            _skillEfect[1].Play();
            _skillWaitTime = time;
        }
    }
    
    public void SpeedUpSkill(InputAction.CallbackContext context, float time)
    {
        if (_skillWaitTime <= 0)
        {
            _skillEfect[2].Play();
            _speed = 6.0f;
            _skillWaitTime = time;
        }
    }

    public void CountTime()
    {
        //攻撃クールタイム計算
        if (_attackWaitTime <= 0)
        {
            _attackWaitTime = 0;
        }
        else
        {
            _attackWaitTime -= Time.deltaTime;
        }

        //スキルクールタイム計算
        if (_skillWaitTime <= 0)
        {
            _skillWaitTime = 0;
        }
        else
        {
            _skillWaitTime -= Time.deltaTime;
        }
    }
    
    public void JumpInputEnd()
    {
        _jumpInput = false;
    }
    
    public void AttackInputEnd()
    {
        _attackInput = false;
    }
}