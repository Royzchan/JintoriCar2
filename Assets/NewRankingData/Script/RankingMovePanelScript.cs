using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingMovePanelScript: RankingProcess
{
    [SerializeField, Header("ランキング表示用の看板")]
    private RectTransform _rankingPanel;
    [SerializeField, Header("看板が下りてくる時間")]
    private float _downTime;
    [SerializeField,Header("看板が下りてくる位置")]
    private float _downPos;
    [SerializeField, Header("看板が開くまでの時間")]
    private float _openTime;
    public override bool UpdateProcess()
    {
        if (!_startRun)
        {
            StartCoroutine(MovePanel());
            _startRun = true;
            
        }

        return _endRun;
    }

    IEnumerator MovePanel()
    {
        _rankingPanel.DOAnchorPosY(_downPos, _downTime).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(_downTime);
        //_rankingPanel.DOAnchorMax(new Vector2(0.5f,0.5f),_openTime).SetEase(Ease.InBack);
        //yield return new WaitForSeconds(_openTime + 0.3f);
        _endRun = true;
    }
}
