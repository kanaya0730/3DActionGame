using UnityEngine;

public class BuffState : IPlayerState
{
    public void OnStart(PlayerController playerController)
    {
        playerController.SkillEffect[playerController.SkillID].Play();
        playerController.SkillInputEnd();
    }

    public void OnUpData(PlayerController playerController)
    {
        if (playerController.MoveInput != Vector2.zero)
        {
            playerController.ChangeState(playerController.WalkInput
                ? PlayerController.PlayerStateId.Walk
                : PlayerController.PlayerStateId.Run);
        }
        
        if (playerController.MoveInput == Vector2.zero)
            playerController.ChangeState(PlayerController.PlayerStateId.Stand);
        
        if (playerController.JumpInput)
            playerController.ChangeState(PlayerController.PlayerStateId.JumpToTop);
    }

    public string GetText() => "スキルを使った";
}
