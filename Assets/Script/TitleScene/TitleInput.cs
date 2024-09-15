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
                // �{�^���������ꂽ���̏���
                break;

            case InputActionPhase.Canceled:
                _downStartButton = false;
                // �{�^���������ꂽ���̏���
                break;
        }
    }
}
