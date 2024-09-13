using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingData : MonoBehaviour
{
    public static List<PlayerData> playerDatas = new List<PlayerData>();

    //�v���C���[�̃}�e���A���̏��ƃv���C���[�̃X�R�A���Z�b�g�ŕۑ�
    public static Dictionary<int, Material> _playerScore = new Dictionary<int, Material>();
    //���l�Ńv���C���Ă��邩��ۑ�
    public static int _playerNum = 0;

    public static PlayerData GetRankData(int rank)
    {
        var data = playerDatas.OrderByDescending(d => d._score).ToArray()[rank];
        return data;
    }
}
