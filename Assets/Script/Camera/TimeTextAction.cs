using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextAction : MonoBehaviour
{
    [SerializeField, Header("タイマー用テキスト")]
    public Text _timeText;

    [SerializeField, Header("残り何秒で開始するか")]
    int _actionTime;

    private GameManager _gm;

    //前回の時間を保存
    float _oldTime;

    // Start is called before the first frame update
    void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム中だったら制限時間のテキストを更新
        if (_gm.IsPlaying)
        {
            //第二引数は1以下
            StartCoroutine(LastCroutine(Mathf.Ceil(_gm.TimeLimit), 0.5f));
        }
    }


    //残り〇秒での演出
    IEnumerator LastCroutine(float nowTime, float t)
    {
        if (_timeText == null) yield break;
        //5秒以下で実行
        if (nowTime > _actionTime) yield break;
        //前回と時間が違ったら実行
        if (nowTime == _oldTime) yield break;

        //演出の時間を計測
        float _t = 0;
        //今回の時間を上書き
        _oldTime = nowTime;

        //演出の最大時間を現在の演出時間が超えるまでwhile
        while (_t < t)
        {
            //演出時間カウント
            _t += Time.deltaTime;

            //最大演出時間の前半で拡大
            if (_t < t / 2)
            {
                _timeText.rectTransform.localScale = Vector3.Lerp(_timeText.rectTransform.localScale, new Vector3(1.5f, 1.5f, 1f), t / 2);
                yield return null;
            }
            //後半で縮小
            else if (_t >= t / 2)
            {
                _timeText.rectTransform.localScale = Vector3.Lerp(_timeText.rectTransform.localScale, Vector3.one, t / 2);
                yield return null;
            }
        }
    }
}
