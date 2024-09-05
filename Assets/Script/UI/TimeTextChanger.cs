using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class TimeTextChanger : MonoBehaviour
{

    [SerializeField, Header("タイマー用テキスト")]
    public Text _timeText;
    [SerializeField, Header("カウントダウン用テキスト")]
    public Text _countText;

    /*内部処理用*/
    private GameManager _gm;

    void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        //ゲーム中だったら制限時間のテキストを更新
        if(_gm.IsPlaying)
        {
            _timeText.text = _gm.TimeLimit.ToString("####");
            //カウントダウンは消しておく
            _countText.text = "";
        }
        //カウントダウン中だったら
        else
        {
            //カウントダウンのテキストを更新
            _countText.text = _gm.CountDown.ToString("####");
        }
    }
}
