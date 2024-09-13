using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField, Header("シーン切替")]
    ChangeScene _cs;
    [SerializeField,Header("ランキングのアニメーション等の処理")]
    private RankingProcess[] _processes;
    [SerializeField]
    Animator[] _carAnimators;
    [SerializeField, Header("ランキング用車種のデータ")]
    GameObject[] _carPrefabs;
    public List<GameObject> _carObjs;
    int _processNum = 0;
    private void Start()
    {
        
        if(RankingData.playerDatas.Count != 0)
        {
            for(int i = 0;i < RankingData.playerDatas.Count;++i)
            {
                var playerData = RankingData.GetRankData(i);
                GameObject g = Instantiate(_carPrefabs[playerData._playerModelNum]);
                Animator anim = g.AddComponent<Animator>();
                anim.runtimeAnimatorController = _carAnimators[i].runtimeAnimatorController;
                _carObjs.Add(g);
            }
        }
        if(_carObjs.Count != 0)
        {
            _processes[0] = _carObjs[0].AddComponent<RankingCarScript>();
        }
        
    }
    // Update is called once per frame
    void Update()
    {

        //処理項目分繰り返す
        if(_processNum < _processes.Length)
        {
            if (_processes[_processNum] != null)
            {
                if (_processes[_processNum].UpdateProcess())
                {
                    //trueなら処理を進める
                    _processNum++;
                }
            }
            //nullなら処理を進める
            else
            {
                _processNum++;
            }
        }
        else
        {
            _cs.enabled = true;
        }
    }
}
