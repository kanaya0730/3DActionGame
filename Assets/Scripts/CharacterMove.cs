using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using UnityEngine.UI;
public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    private Button _normalAttack;

    [SerializeField] 
    private Button _skillAttack;

    [SerializeField]
    private Button _specialAttack;
    
    public void Start()
    {
        _normalAttack.onClick.AsObservable()
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromSeconds(1))
            .Subscribe(_ => NormalAttack());
        
        _skillAttack.onClick.AsObservable()
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromSeconds(1))
            .Subscribe(_ => SkillAttack());
        
        _specialAttack.onClick.AsObservable()
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromSeconds(1))
            .Subscribe(_ => SpecialAttack());
    }


    public void NormalAttack()
    {
        Debug.Log("通常攻撃");
    }
    
    public void SkillAttack()
    {
        Debug.Log("スキル攻撃");
    }
    
    public void SpecialAttack()
    {
        Debug.Log("特殊攻撃");
    }
    
}
