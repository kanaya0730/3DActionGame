using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private PlayerAction _inputActions;
    
    private Animator _anim;
    private Rigidbody _rb;
    
    [SerializeField]
    private float _speed = 3f;

    [SerializeField] 
    private float _attackWaitTime;

    [SerializeField] 
    private float _skillWaitTime;

    [SerializeField]
    [Header("攻撃エフェクト")]
    private ParticleSystem _particleAttack;
    
    [SerializeField]
    [Header("効果エフェクト")]
    private ParticleSystem[] _particle;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        
        _inputActions = new PlayerAction();
        _inputActions.Enable();
        
        _inputActions.Player.Attack.performed += context => Attack(1);
        _inputActions.Player.Jump.performed += context => Jump();
        _inputActions.Player.PowerUpSkill.performed += context => PowerUpSkill(3);
        _inputActions.Player.SpeedUpSkill.performed += context => SpeedUpSkill(3);
        
        this.FixedUpdateAsObservable()
            .Subscribe(_ => OnMove())
            .AddTo(this);
        
        this.UpdateAsObservable()
            .Subscribe(_ => CountTime())
            .AddTo(this);
    }
    
    private void OnMove()
    {
        var direction = _inputActions.Player.Move.ReadValue<Vector2>();
        
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * direction.y + Camera.main.transform.right * direction.x;
 
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        _rb.velocity = moveForward * _speed + new Vector3(0, _rb.velocity.y, 0);
 
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) 
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
            _anim.SetBool("Run", true);
        }

        else
        {
            _anim.SetBool("Run", false);
        }
        
    }

    public void Attack(float time)
    {
        if (_attackWaitTime <= 0)
        {
            _particleAttack.Play();
            _attackWaitTime = time;
        }
    }
    
    public void Jump()
    {
        _anim.SetTrigger("Jump");
    }

    public void PowerUpSkill(float time)
    {
        if (_skillWaitTime <= 0)
        {
            _particle[1].Play();
            _skillWaitTime = time;
        }
    }
    
    public void SpeedUpSkill(float time)
    {
        if (_skillWaitTime <= 0)
        {
            _particle[2].Play();
            _speed = 6.0f;
            _skillWaitTime = time;
        }
    }

    public void CountTime()
    {
        if (_attackWaitTime <= 0)
        {
            _attackWaitTime = 0;
        }
        else
        {
            _attackWaitTime -= Time.deltaTime;
        }

        if (_skillWaitTime <= 0)
        {
            _skillWaitTime = 0;
        }
        else
        {
            _skillWaitTime -= Time.deltaTime;
        }
    }
}