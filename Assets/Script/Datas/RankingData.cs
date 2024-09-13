using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingData : MonoBehaviour
{
    public static List<PlayerData> playerDatas = new List<PlayerData>();

    //プレイヤーのマテリアルの情報とプレイヤーのスコアをセットで保存
    public static Dictionary<int, Material> _playerScore = new Dictionary<int, Material>();
    //何人でプレイしているかを保存
    public static int _playerNum = 0;

    public static PlayerData GetRankData(int rank)
    {
        var data = playerDatas.OrderByDescending(d => d._score).ToArray()[rank];
        return data;
    }
}
