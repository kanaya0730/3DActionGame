using System;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class PowerAttackState : IPlayerState
{
    public async void OnStart(PlayerController playerController)
    {
        playerController.Anim.Play("Sword And Shield Slash");
        await UniTask.Delay(TimeSpan.FromSeconds(1.75f));
        playerController.PowerAttackInputEnd();
    }

    public void OnUpData(PlayerController playerController)
    {
        if (playerController.PowerAttackInput) return;
        
        if (playerController.MoveInput != Vector2.zero)
        {
            playerController.ChangeState(playerController.WalkInput
                ? PlayerController.PlayerStateId.Walk
                : PlayerController.PlayerStateId.Run);
        }
        else
            playerController.ChangeState(PlayerController.PlayerStateId.Stand);

        if (!playerController.JumpInput) return;
        playerController.ChangeState(playerController.IsGround
            ? PlayerController.PlayerStateId.JumpToTop
            : PlayerController.PlayerStateId.TopOfJump);
    }

    public string GetText() => "攻撃した";
}
