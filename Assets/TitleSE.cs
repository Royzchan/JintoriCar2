using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSE : MonoBehaviour
{
    GameStart _gameStart;

    AudioSource _audioSource;

    bool _isPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        _gameStart = GameObject.Find("GameStart").GetComponent<GameStart>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStart.GetSceneChange() && _isPlay)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
            _isPlay = false;
        }
    }
}
