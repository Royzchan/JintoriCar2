using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleInput : MonoBehaviour
{

    private bool _downStartButton = false;

    public bool DownStartButton { get { return _downStartButton; } }

    public void OnStartButton(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _downStartButton = true;
                // ボタンが押された時の処理
                break;

            case InputActionPhase.Canceled:
                _downStartButton = false;
                // ボタンが離された時の処理
                break;
        }
    }
}
