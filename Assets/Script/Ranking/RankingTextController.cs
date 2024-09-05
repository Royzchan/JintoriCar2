using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingTextController : MonoBehaviour
{
    [SerializeField, Header("ランキングのテキストを入れる")]
    private GameObject[] _rankText;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _rankText.Count(); i++)
        {
            if (i < RankingData._playerNum)
            {
                _rankText[i].SetActive(true);
            }
            else
            {
                _rankText[i].SetActive(false);
            }
        }
    }
}
