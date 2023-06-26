using System;
using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.IO;


public class FirstLogin : MonoBehaviour
{
    [SerializeField]
    [Header("パスワード")]
    private string _customKey;
    
    [SerializeField]
    private LoginKeyData _loginKeyData;
    
    private bool _shouldCreateAccount;

    void OnEnable()
    {
        PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
    }
    void Start()
    {
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }

    //ログイン成功
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("ログイン成功");
        Debug.Log("アカウントが存在するかどうか確認");
        _shouldCreateAccount = result.NewlyCreated;
        
        if (!_shouldCreateAccount)
        {
            LoginKey loginKey = JsonSaveManager<LoginKey>.Load(_customKey);

            if (loginKey == null)
            {
                Debug.Log("アカウントが存在しないのでパスワードとキーを作成");
                loginKey = new LoginKey
                {
                    _loginKeyData = GenerateCustomID(),
                    _playFabID = result.PlayFabId,
                };
                
                _loginKeyData.SetValue(loginKey);
            }
            else
            {
                Debug.Log("既にこのパスワードのキーが存在する");
                _loginKeyData.SetValue(loginKey);
            }
        }
        else
        {
            Debug.Log("アカウントが存在するので正しいパスワードを入力して下さい");
        }
    }
    
    /// <summary>アプリケーション終了時に呼び出す</summary>
    private void OnApplicationQuit() 
    {
        OverWriteSaveData();
    }
    
    //保存
    private void OverWriteSaveData()
    {
        // if (!_shouldCreateAccount)
        // {
        //     return;
        // }
        
        LoginKey loginKey = new LoginKey()
        {
            _loginKeyData = _loginKeyData.CustomID,
            _playFabID = _loginKeyData.PlayFabID,
        };

        JsonSaveManager<LoginKey>.Save(loginKey, _customKey);
    }

    private void OnDisable()
    {
        PlayFabAuthService.OnLoginSuccess -= OnLoginSuccess;
    }
    
    private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";
    
    /// <summary>カスタムID生成</summary>
    /// <returns>生成したID</returns>
    private string GenerateCustomID()
    {
        var idLength = 32;//IDの長さ
        StringBuilder stringBuilder = new StringBuilder(idLength);
        var random = new System.Random();
    
        for (var i = 0; i < idLength; i++)
        {
            stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
        }

        return stringBuilder.ToString();
    }
}
