using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    private ChangeScene _changeScene;

    #region input関係

    public void OnStartButton(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                _changeScene.DebugChange();
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                break;
        }
    }

    #endregion

    void Start()
    {
        _changeScene = GetComponent<ChangeScene>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _changeScene.DebugChange();
        }
    }
}
