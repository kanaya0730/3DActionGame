using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void OnStart(PlayerController playerController);
    void OnUpData(PlayerController playerController);
    
    string GetText();
}
