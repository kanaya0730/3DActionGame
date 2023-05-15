using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class JumpState : IPlayerState
{
    private float _jumpTime = 1.5f;
    
    public async void OnStart(PlayerController playerController)
    {
        playerController.Anim.Play("Jumping@loop");
        await UniTask.Delay(TimeSpan.FromSeconds(_jumpTime));
        playerController.JumpInputEnd();
        playerController.ChangeState(PlayerController.PlayerStateId.Stand);
    }

    public void OnUpData(PlayerController playerController)
    {
        
    }

    public string GetText() => "ジャンプした";
}
