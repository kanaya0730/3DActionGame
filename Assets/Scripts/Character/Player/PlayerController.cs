using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region property
    private IPlayerState CurrentPlayerState => _stateData[_currentStateId];
    public PlayerAction InputAction => _inputActions;
    public Animator Anim => _anim;
    public Rigidbody Rb => _rb;
    public Vector2 MoveInput => _moveInput;
    public ParticleSystem[] SkillEffect => skillEffect;
    public float Speed => _speed;
    public float AttackCoolTime => _attackCoolTime;
    public float SkillCoolTIme => _skillCoolTime;
    public bool JumpInput => _jumpInput;
    public bool AttackInput => _attackInput;
    public bool PowerAttackInput => _powerAttackInput;
    public bool WalkInput => _walkInput;
    public bool SkillInput => _skillInput;
    public bool IsGround => _isGround;
    public int SkillID => _skillID;
    #endregion
    
    private PlayerAction _inputActions;
    private Animator _anim;
    private Rigidbody _rb;
    
    private Vector2 _moveInput;
    
    private float _attackCoolTime;
    private float _skillCoolTime;
    
    private bool _jumpInput;
    private bool _attackInput;
    private bool _powerAttackInput;
    private bool _walkInput;
    private bool _skillInput;
    
    private PlayerStateId _currentStateId;
    
    private readonly Dictionary<PlayerStateId, IPlayerState> _stateData = new();
    
    private string _currentState;
    
    [SerializeField]
    private float _speed = 3f;
    
    [SerializeField]
    [Header("効果エフェクト")]
    private ParticleSystem[] skillEffect;

    [SerializeField]
    private bool _isGround;
    
    [SerializeField]
    private int _skillID = 0;
    
    public enum PlayerStateId
    {
        None,
        Stand,
        Walk,
        Run,
        JumpToTop,
        TopOfJump,
        TopToGround,
        Attack,
        PowerAttack,
        Skill,
    }
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _inputActions = new PlayerAction();
        _inputActions.Enable();
        
        _inputActions.Player.Attack.performed += context => Attack(context,1);
        _inputActions.Player.PowerAttack.performed += context => PowerAttack(context,1); 
        _inputActions.Player.Jump.performed += Jump;
        _inputActions.Player.Walk.performed += Run;
        _inputActions.Player.Skill.performed += context => Skill(context,3);
        _inputActions.Player.Plus.performed += context => SkillSelect(1);
        _inputActions.Player.Minus.performed += context => SkillSelect(-1);

        _currentStateId = PlayerStateId.None;
        _stateData.Add(PlayerStateId.Walk,new WalkState());
        _stateData.Add(PlayerStateId.Run, new RunState());
        _stateData.Add(PlayerStateId.JumpToTop, new JumpToTopState());
        _stateData.Add(PlayerStateId.TopOfJump, new TopOfJumpState());
        _stateData.Add(PlayerStateId.TopToGround, new TopToGroundState());
        _stateData.Add(PlayerStateId.Stand,new StandState());
        _stateData.Add(PlayerStateId.Attack,new AttackState());
        _stateData.Add(PlayerStateId.Skill,new BuffState());
        _stateData.Add(PlayerStateId.PowerAttack, new PowerAttackState());
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
        if (_currentStateId == nextState)
            return;

        _currentStateId = nextState;
        CurrentPlayerState.OnStart(this);
        
        Debug.Log(_currentStateId);
        Debug.Log(_currentState);
    }
    
    private void OnMove()
    {
        _moveInput = _inputActions.Player.Move.ReadValue<Vector2>();

        CurrentPlayerState.OnUpData(this);

        _currentState = CurrentPlayerState.GetText();
    }

    private void Attack( InputAction.CallbackContext context, float time)
    {
        if (!(_attackCoolTime <= 0)) return;
        
        _attackInput = context.ReadValueAsButton();
        _attackCoolTime = time;
    }

    private void PowerAttack(InputAction.CallbackContext context, float time)
    {
        if (!(_attackCoolTime <= 0)) return;
        
        _powerAttackInput = context.ReadValueAsButton();
        _attackCoolTime = time;
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValueAsButton();
        _isGround = false;
    }
    
    private void Run(InputAction.CallbackContext context)
    {
        _walkInput = context.ReadValueAsButton();
    }

    private void SkillSelect(int value)
    {
        _skillID += value;
        
        if (_skillID < 0)
        {
            _skillID = (skillEffect.Length - 1);
        }

        if (_skillID > skillEffect.Length)
        {
            _skillID = 0;
        }
    }
    
    private void Skill(InputAction.CallbackContext context, float coolTime)
    {
        if (!(_skillCoolTime <= 0)) return;
        _skillInput = context.ReadValueAsButton();
        _skillCoolTime = coolTime;
    }

    private void CountTime()
    {
        //攻撃クールタイム計算
        if (_attackCoolTime <= 0)
            _attackCoolTime = 0;
        else
            _attackCoolTime -= Time.deltaTime;

        //スキルクールタイム計算
        if (_skillCoolTime <= 0)
            _skillCoolTime = 0;
        else
            _skillCoolTime -= Time.deltaTime;
    }

    public void JumpInputEnd()
    {
        _jumpInput = false;
        Debug.Log("ジャンプした");
    }

    public void AttackInputEnd()
    {
        _attackInput = false;
        Debug.Log("攻撃した");
    }

    public void WalkInputEnd()
    {
        _walkInput = false;
        Debug.Log("歩き終わったした");
    }

    public void SkillInputEnd()
    {
        _skillInput = false;
        Debug.Log("スキルを発動した");
    }

    public void PowerAttackInputEnd()
    {
        _powerAttackInput = false;
        Debug.Log("強攻撃");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGround = true;
    }
}