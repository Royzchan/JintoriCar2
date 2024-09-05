using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Progress;
using System.Linq;

public class RankingGet : MonoBehaviour
{

    [SerializeField, Header("�����L���O�\���e�L�X�g")]
    private List<Text> _ranktext;

    //�����L���O�̃f�[�^
    private List<PlayerData> _playerRank = new List<PlayerData>();

    void Start()
    {
        GetRanking();
    }

    public void GetRanking()
    {
        //�����foreach�ł̃J�E���g�p
        int n = 0;

        //�����N�ō~���ɕ��ѕς��đ��
        var order_list2 = RankingData.playerDatas.OrderByDescending(d => d._score);

        foreach (var item in order_list2)
        {
            _ranktext[n].text = n + 1 + "�ʂ�" + item._name + " " + item._score + "�_";
            n++;
            if (n >= _ranktext.Count)
            {
                break;
            }
        }
    }
}
