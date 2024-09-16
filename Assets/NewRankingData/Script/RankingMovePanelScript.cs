using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingMovePanelScript: RankingProcess
{
    [SerializeField, Header("�����L���O�\���p�̊Ŕ�")]
    private RectTransform _rankingPanel;
    [SerializeField, Header("�Ŕ�����Ă��鎞��")]
    private float _downTime;
    [SerializeField,Header("�Ŕ�����Ă���ʒu")]
    private float _downPos;
    [SerializeField, Header("�Ŕ��J���܂ł̎���")]
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
