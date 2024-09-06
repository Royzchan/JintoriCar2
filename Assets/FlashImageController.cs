using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImageController : MonoBehaviour
{
    Image _image;

    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;

    private double _time;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // 内部時刻を経過させる
        _time += Time.deltaTime;

        // 周期cycleで繰り返す波のアルファ値計算
        var alpha = Mathf.Cos((float)(2 * Mathf.PI * _time / _cycle)) * 0.5f + 0.5f;

        // 内部時刻timeにおけるアルファ値を反映
        var color = _image.color;
        color.a = alpha;
        _image.color = color;
    }
}
