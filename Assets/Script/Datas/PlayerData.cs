using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string _name;
    public int _score;
    public Material _playerMat;
    public GameObject _playerObj;
    public int _playerModelNum;

    public PlayerData(string name, int score,Material playerMat,int modelNum)
    {
        _name = name;
        _score = score;
        _playerMat = playerMat;
        _playerModelNum = modelNum;
    }
}
