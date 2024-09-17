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
            if (_gm.TimeLimit > _actionTime) return;
            if (!_active)
            {
                _circleCountImage.gameObject.SetActive(true);
                _circleCountText.gameObject.SetActive(true);
                _active = true;
            }
            _circleCountText.text = Mathf.Ceil(_gm.TimeLimit).ToString();
            _circleCountImage.fillAmount = _gm.TimeLimit % 1.0f;
        }
    }
}
