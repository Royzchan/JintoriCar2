using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonEffect : MonoBehaviour
{
    GameStart _gameStart;

    ParticleSystem _startButtonEffect;

    bool _isPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        _gameStart = GameObject.Find("GameStart").GetComponent<GameStart>();
        _startButtonEffect = GameObject.Find("StartEffect").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStart.GetSceneChange() && _isPlay)
        {
            _startButtonEffect.Play();
            _isPlay = false;
        }
    }
}
