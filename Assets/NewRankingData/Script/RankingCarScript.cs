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
        //�A�j���[�V�����̎��Ԃ��擾
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        //���̎��ԕ��ҋ@
        yield return new WaitForSeconds(state.length);
        //�I��
        _endRun = true;
    }
}
