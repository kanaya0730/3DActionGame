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
            playerController.ChangeState(playerController.WalkInput
                ? PlayerController.PlayerStateId.Walk
                : PlayerController.PlayerStateId.Run);
        }

        if (playerController.JumpInput)
            playerController.ChangeState(PlayerController.PlayerStateId.JumpToTop);

        if (playerController.AttackInput)
            playerController.ChangeState(PlayerController.PlayerStateId.Attack);
        
        if (playerController.PowerAttackInput)
            playerController.ChangeState(PlayerController.PlayerStateId.PowerAttack);

        if (playerController.SkillInput)
            playerController.ChangeState(PlayerController.PlayerStateId.Skill);
    }

    public string GetText() => "立った";
}
