using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIControler : MonoBehaviour
{
    private Image _timerImage;

    private GameManager _gm;

    private float _timeLimit;

    void Start()
    {
        _gm = FindAnyObjectByType<GameManager>();
        _timerImage = GetComponent<Image>();
        _timeLimit = _gm.TimeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        _timerImage.fillAmount = _gm.TimeLimit / _timeLimit;
    }
}
