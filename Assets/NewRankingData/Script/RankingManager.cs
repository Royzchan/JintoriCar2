using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField, Header("�V�[���ؑ�")]
    ChangeScene _cs;
    [SerializeField,Header("�����L���O�̃A�j���[�V�������̏���")]
    private RankingProcess[] _processes;
    [SerializeField]
    Animator[] _carAnimators;
    [SerializeField, Header("�����L���O�p�Ԏ�̃f�[�^")]
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

        //�������ڕ��J��Ԃ�
        if(_processNum < _processes.Length)
        {
            if (_processes[_processNum] != null)
            {
                if (_processes[_processNum].UpdateProcess())
                {
                    //true�Ȃ珈����i�߂�
                    _processNum++;
                }
            }
            //null�Ȃ珈����i�߂�
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
