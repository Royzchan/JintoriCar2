using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RankingProcess : MonoBehaviour
{
    protected bool _endRun = false;
    protected bool _startRun = false;
    public abstract bool UpdateProcess();
    #region UpdateProcess�̎g����
   //�p�����
   //public override bool UpdateProcess()
   //��錾���ď����̊J�n����
   //if(!_startRun)
   //{
   // _startRun = true;
   //}
   //���L�q����Start�֐���������ɂ��Ă�������
   //�����I����ɂ�_endRun��true�ɂ���Ύ��̏����ֈڍs�ł��܂�
    #endregion

}
