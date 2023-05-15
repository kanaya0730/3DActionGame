using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandState : IPlayerState
{
    public void OnStart(PlayerController playerController)
    {
        playerController.Anim.Play("Standing@loop");
    }

    public void OnUpData(PlayerController playerController)
    {
        if (playerController.MoveInput != Vector2.zero)
        {
            playerController.ChangeState(PlayerController.PlayerStateId.Walk);
        }

        if (playerController.JumpInput)
        {
            Debug.Log("3");
            playerController.ChangeState(PlayerController.PlayerStateId.Jump);
        }
        
        if (playerController.RunInput)
        {
            playerController.ChangeState(PlayerController.PlayerStateId.Run);
        }
        
        if (playerController.AttackInput)
        {
            playerController.ChangeState(PlayerController.PlayerStateId.Attack);
        }
    }

    public string GetText() => "立った";
}
