using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleCount : MonoBehaviour
{
    [SerializeField]
    Image _circleCountImage;

    [SerializeField]
    Text _circleCountText;

    [SerializeField, Header("残り何秒で開始するか")]
    int _actionTime;

    private GameManager _gm;

    bool _active = false;

    float countDownElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム中だったら制限時間のテキストを更新
        if (_gm.IsPlaying)
        {
            if (_gm.TimeLimit.ToString("####") == "") return;
            if (int.Parse(_gm.TimeLimit.ToString("####")) > _actionTime) return;
            if (!_active)
            {
                _circleCountImage.gameObject.SetActive(true);
                _circleCountText.gameObject.SetActive(true);
            }
            _circleCountText.text = _gm.TimeLimit.ToString("####");
            countDownElapsedTime += Time.deltaTime;
            _circleCountImage.fillAmount = countDownElapsedTime % 1.0f;
        }
    }
}
