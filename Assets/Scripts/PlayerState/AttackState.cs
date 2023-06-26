using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AttackState : IPlayerState
{
    public async void OnStart(PlayerController playerController)
    {
        playerController.Anim.Play("SwordAttack");
        await UniTask.Delay(TimeSpan.FromSeconds(1.2f)); 
        playerController.AttackInputEnd();
    }

    public void OnUpData(PlayerController playerController)
    {
        if (playerController.AttackInput) return;
        
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
