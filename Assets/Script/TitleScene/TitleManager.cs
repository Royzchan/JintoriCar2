using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    private ChangeScene _changeScene;

    public GameStart _gameStart;

    [SerializeField, Header("タイトルシーンのインプット")]
    private GameObject _titleinput;

    void Start()
    {
        _changeScene = GetComponent<ChangeScene>();
        _gameStart.Set1TiteInput(Instantiate(_titleinput).GetComponent<TitleInput>());
        _gameStart.Set2TiteInput(Instantiate(_titleinput).GetComponent<TitleInput>());
    }

    void Update()
    {

    }
}
