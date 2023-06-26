using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TopToGroundState : IPlayerState
{
    private float _animTIme = 1.8f;

    public async void OnStart(PlayerController playerController)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_animTIme));
        playerController.JumpInputEnd();
        playerController.ChangeState(PlayerController.PlayerStateId.Stand);
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
        
    }

    public string GetText() => "着地した";
}
