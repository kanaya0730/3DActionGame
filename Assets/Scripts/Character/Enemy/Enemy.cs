using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float _hp = 100;

    public void AddDamage(float damage)
    {
        _hp -= damage;
        Debug.Log("add: " + damage + "hp: " + _hp);

        if (_hp <= 0)
        {
            Debug.Log("Enemyを倒した");
        }
    }
}