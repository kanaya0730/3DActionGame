using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParamData",
    menuName = "ScriptableObjects/EnemyParamAsset")]
public class EnemyParamData : ScriptableObject
{
    [SerializeField]
    [Header("敵のステータス")]
    private List<EnemyParam> _enemyParams = new();
}

[System.Serializable]
public class EnemyParam
{
    [SerializeField]
    [Header("名前")]
    private string _enemyName;
    
    [SerializeField]
    [Header("体力")]
    private float _enemyHp;
    
    [SerializeField]
    [Header("攻撃力")]
    private float _enemyAtk;
    
    [SerializeField]
    [Header("防御力")]
    private float _enemyDef;
    
    [SerializeField]
    [Header("獲得できる経験値")]
    private float _enemyGold;
    
    [SerializeField]
    [Header("獲得できる経験値")]
    private float _enemyExp;
}
