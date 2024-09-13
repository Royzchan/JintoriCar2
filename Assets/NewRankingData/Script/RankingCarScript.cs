using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingCarScript : RankingProcess
{
    Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        
    }
    public override bool UpdateProcess()
    {
        if(!_startRun)
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(AnimationCar());
            _startRun = true;
        }
        
        return _endRun;
    }

    IEnumerator AnimationCar()
    {
        //アニメーションの時間を取得
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        //その時間分待機
        yield return new WaitForSeconds(state.length);
        //終了
        _endRun = true;
    }
}
