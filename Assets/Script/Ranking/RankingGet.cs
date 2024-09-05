using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Progress;
using System.Linq;

public class RankingGet : MonoBehaviour
{

    [SerializeField, Header("ランキング表示テキスト")]
    private List<Text> _ranktext;

    //ランキングのデータ
    private List<PlayerData> _playerRank = new List<PlayerData>();

    void Start()
    {
        GetRanking();
    }

    public void GetRanking()
    {
        //これはforeachでのカウント用
        int n = 0;

        //リンクで降順に並び変えて代入
        var order_list2 = RankingData.playerDatas.OrderByDescending(d => d._score);

        foreach (var item in order_list2)
        {
            _ranktext[n].text = n + 1 + "位は" + item._name + " " + item._score + "点";
            n++;
            if (n >= _ranktext.Count)
            {
                break;
            }
        }
    }
}
