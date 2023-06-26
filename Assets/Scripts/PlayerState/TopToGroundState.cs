using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TopToGroundState : IPlayerState
{
    public async void OnStart(PlayerController playerController)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.58f));
        playerController.JumpInputEnd();
    }

    public void OnUpData(PlayerController playerController)
    { 
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * playerController.MoveInput.y + Camera.main.transform.right * playerController.MoveInput.x;
 
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        playerController.Rb.velocity = moveForward * playerController.Speed + new Vector3(0, playerController.Rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) 
        {
            playerController.transform.rotation = Quaternion.LookRotation(moveForward);
        }
        
        if(playerController.MoveInput == Vector2.zero && !playerController.JumpInput)
            playerController.ChangeState(PlayerController.PlayerStateId.Stand);

        if (playerController.MoveInput != Vector2.zero && !playerController.JumpInput)
        {
            playerController.ChangeState(playerController.WalkInput
                ? PlayerController.PlayerStateId.Walk
                : PlayerController.PlayerStateId.Run);
        }
        
    }

    public string GetText() => "着地した";
}
