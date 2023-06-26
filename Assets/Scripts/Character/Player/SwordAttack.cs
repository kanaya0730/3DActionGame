using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField]
    private float _atk;

    private void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject.GetComponent<IDamagable>();

        if (target != null)
        {
            other.gameObject.GetComponent<IDamagable>().AddDamage(_atk);
        }
    }
}
