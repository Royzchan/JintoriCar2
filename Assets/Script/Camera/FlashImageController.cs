using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImageController : MonoBehaviour
{
    Image _image;

    // �_�Ŏ���[s]
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
        // �����������o�߂�����
        _time += Time.deltaTime;

        // ����cycle�ŌJ��Ԃ��g�̃A���t�@�l�v�Z
        var alpha = Mathf.Cos((float)(2 * Mathf.PI * _time / _cycle)) * 0.5f + 0.5f;

        // ��������time�ɂ�����A���t�@�l�𔽉f
        var color = _image.color;
        color.a = alpha;
        _image.color = color;
    }
}
