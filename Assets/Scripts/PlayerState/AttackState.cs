using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    public void OnStart(PlayerController playerController)
    {
        playerController.ParticleAttack.Play();
    }

    public void OnUpData(PlayerController playerController)
    {
        
    }

    public string GetText() => "攻撃した";
}
