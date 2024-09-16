using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RankingProcess : MonoBehaviour
{
    protected bool _endRun = false;
    protected bool _startRun = false;
    public abstract bool UpdateProcess();
    #region UpdateProcessの使い方
   //継承先で
   //public override bool UpdateProcess()
   //を宣言して処理の開始時に
   //if(!_startRun)
   //{
   // _startRun = true;
   //}
   //を記述してStart関数処理代わりにしてください
   //処理終了後には_endRunをtrueにすれば次の処理へ移行できます
    #endregion

}
