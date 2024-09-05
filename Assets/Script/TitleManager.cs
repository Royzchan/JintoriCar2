using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    private ChangeScene _changeScene;

    #region input�֌W

    public void OnStartButton(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // �{�^���������ꂽ���̏���
                _changeScene.DebugChange();
                break;

            case InputActionPhase.Canceled:
                // �{�^���������ꂽ���̏���
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
