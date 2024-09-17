using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCircleCount : MonoBehaviour
{
    [SerializeField]
    Image _circleCountImage;

    [SerializeField]
    Text _circleCountText;

    private GameManager _gm;

    // Start is called before the first frame update
    void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
        _circleCountImage.gameObject.SetActive(true);
        _circleCountText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //カウントダウン中だったら
        if (_gm.IsPlaying)
        {
            _circleCountImage.gameObject.SetActive(false);
            _circleCountText.gameObject.SetActive(false);
        }
        else
        {
            _circleCountImage.fillAmount = _gm.CountDown % 1.0f;
            _circleCountText.text = Mathf.Ceil(_gm.CountDown).ToString();
            
        }
    }
}
