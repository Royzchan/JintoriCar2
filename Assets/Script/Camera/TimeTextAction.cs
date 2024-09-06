using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextAction : MonoBehaviour
{
    [SerializeField, Header("�^�C�}�[�p�e�L�X�g")]
    public Text _timeText;

    [SerializeField]
    Color _actionColor;

    [SerializeField, Header("�c�艽�b�ŊJ�n���邩")]
    int _actionTime;

    private GameManager _gm;

    //�O��̎��Ԃ�ۑ�
    float _oldTime;

    Color _textColorFirst;

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();
        _textColorFirst = _timeText.color;
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[�����������琧�����Ԃ̃e�L�X�g���X�V
        if (_gm.IsPlaying)
        {
            //��������1�ȉ�
            StartCoroutine(LastCroutine(_gm.TimeLimit.ToString("####"), 0.5f));
        }
    }


    //�c��Z�b�ł̉��o
    IEnumerator LastCroutine(string nowTime, float t)
    {
        if (_timeText == null) yield break;
        if (nowTime == "") yield break;
        float _nowTime = float.Parse(nowTime);
        //5�b�ȉ��Ŏ��s
        if (_nowTime > _actionTime) yield break;
        //�O��Ǝ��Ԃ����������s
        if (_nowTime == _oldTime) yield break;

        //���o�̎��Ԃ��v��
        float _t = 0;
        //����̎��Ԃ��㏑��
        _oldTime = _nowTime;

        //_timeText.color = _actionColor;

        //���o�̍ő厞�Ԃ����݂̉��o���Ԃ�������܂�while
        while (_t < t)
        {
            //���o���ԃJ�E���g
            _t += Time.deltaTime;

            //�ő剉�o���Ԃ̑O���Ŋg��
            if (_t < t / 2)
            {
                _timeText.rectTransform.localScale = Vector3.Lerp(_timeText.rectTransform.localScale, new Vector3(1.5f, 1.5f, 1f), t / 2);
                yield return null;
            }
            //�㔼�ŏk��
            else if (_t >= t / 2)
            {
                _timeText.rectTransform.localScale = Vector3.Lerp(_timeText.rectTransform.localScale, Vector3.one, t / 2);
                yield return null;
            }
        }

        //_timeText.color = _textColorFirst;
    }
}
