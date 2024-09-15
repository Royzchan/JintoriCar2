using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    private ChangeScene _changeScene;

    void Start()
    {
        _changeScene = GetComponent<ChangeScene>();
    }

    void Update()
    {

    }
}
