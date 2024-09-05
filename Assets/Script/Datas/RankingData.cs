using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingData : MonoBehaviour
{
    public static List<PlayerData> playerDatas = new List<PlayerData>();

    //プレイヤーのマテリアルの情報とプレイヤーのスコアをセットで保存
    public static Dictionary<int, Material> _playerScore = new Dictionary<int, Material>();
    //何人でプレイしているかを保存
    public static int _playerNum = 0;
}
