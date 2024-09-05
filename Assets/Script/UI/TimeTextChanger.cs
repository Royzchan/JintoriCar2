using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class TimeTextChanger : MonoBehaviour
{

    [SerializeField, Header("�^�C�}�[�p�e�L�X�g")]
    public Text _timeText;
    [SerializeField, Header("�J�E���g�_�E���p�e�L�X�g")]
    public Text _countText;

    /*���������p*/
    private GameManager _gm;

    void Start()
    {
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        //�Q�[�����������琧�����Ԃ̃e�L�X�g���X�V
        if(_gm.IsPlaying)
        {
            _timeText.text = _gm.TimeLimit.ToString("####");
            //�J�E���g�_�E���͏����Ă���
            _countText.text = "";
        }
        //�J�E���g�_�E������������
        else
        {
            //�J�E���g�_�E���̃e�L�X�g���X�V
            _countText.text = _gm.CountDown.ToString("####");
        }
    }
}
