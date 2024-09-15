using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("�`�v���C���[�֌W�`")]

    [SerializeField, Header("�v���C�l��")]
    private int _playerNum = 2;

    [Header("�����͎l�l�o�^���Ă���")]
    [SerializeField, Header("�v���C���[�}�e���A��������")]
    private Material[] _playerMats;

    [SerializeField, Header("�v���C���[�}�e���A���ɑΉ����閼�O������")]
    private string[] _playerNames;

    [Header("�`���Ԋ֌W�`")]

    [SerializeField, Header("�J�E���g�_�E���̎���")]
    private float _countDown = 3.0f;

    [SerializeField, Header("�Q�[���̐�������")]
    private float _timeLimit = 180f;

    [Header("�`�A�C�e���֌W�`")]

    [SerializeField, Header("�A�C�e���擾���̉񕜊���"), Range(0.1f, 1.0f)]
    private float _inkHealValue_Item = 0.3f;

    [SerializeField, Header("�A�C�e���̃X�|�[������")]
    private float _itemSpawnTime = 5.0f;

    [SerializeField, Header("�Q�[���㔼�̃A�C�e���̃X�|�[������")]
    private float _lastItemSpawnTime = 8.0f;

    [SerializeField, Header("���A�C�e�������Ă邩")]
    private int _maxHaveItem = 3;

    [SerializeField, Header("�A�C�e���̌��ʎ���")]
    private float _itemLimit = 3.0f;

    [SerializeField, Header("���e�̃v���t�@�u")]
    private GameObject _bomb;

    [SerializeField, Header("�X�^���̎���")]
    private float _stunTime = 5.0f;

    [Header("�`�V�X�e���֘A�`")]
    [SerializeField, Header("�v���C���[�̉�����")]
    private float _playerAccelerationValue = 5.0f;

    [SerializeField, Header("�C���N�̌�����")]
    private float _playerInkDecValue = 3.0f;

    [SerializeField, Header("�A�C�e���擾���̃C���N�̑�����")]
    private float _getItemInkUpValue = 50.0f;

    [SerializeField,Header("�㔼�̃X�J�C�{�b�N�X")]
    private Material _lastSkyBox;

    [Header("�`���y�֌W�`")]
    [SerializeField, Header("�ʏ펞��BGM��AudioSource")]
    private AudioSource _normalAudio;

    [SerializeField, Header("�I�Վ���BGM��AudioSource")]
    private AudioSource _lastAudio;
    //BGM���ς�������ǂ���
    private bool _changeBGM = false;

    [SerializeField, Header("���x�A�b�v����SE")]
    private AudioClip _speedUpSE;

    [SerializeField, Header("���e��������SE")]
    private AudioClip _throwBombSE;

    [Header("�`�}�b�v�֌W�`")]
    [SerializeField, Header("�ŏ��̃}�b�v�^�C���̐F")]
    private Material _defaultMapTileColor;

    [SerializeField, Header("�Ԃ̃v���C���[�̃}�e���A��")]
    private Material _redPlayerMat;

    [SerializeField, Header("�̃v���C���[�̃}�e���A��")]
    private Material _bluePlayerMat;

    [SerializeField, Header("�΂̃v���C���[�̃}�e���A��")]
    private Material _greenPlayerMat;

    [SerializeField, Header("���̃v���C���[�̃}�e���A��")]
    private Material _purplePlayerMat;

    [SerializeField, Header("�}�b�v�̘g")]
    private GameObject _mapFence;


    //�p�^�C���̖�����ۑ�
    //�������͐��𐔂���p
    private int _defaultTileNum = 0;
    private int _redTileNum = 0;
    private int _blueTileNum = 0;
    private int _greenTileNum = 0;
    private int _purpleTileNum = 0;

    /*���������p*/
    //�Q�[�������ǂ���
    private bool _isPlaying = false;

    //�X�R�A�Z�b�g�ς݂�
    private bool _endScoreSet = false;

    //�v���C���[��ۑ����Ă����ꏊ
    //�S�̂̃J�����֌W�̓Q�[���}�l�[�W���[��
    List<CarController> _carControllers = new List<CarController>();

    //�}�b�v�̃^�C����ۑ�����ϐ�
    GameObject[] _mapTiles;

    //�}�b�v�^�C���̃����_���[
    List<MeshRenderer> _mapTilesRender = new List<MeshRenderer>();

    [SerializeField, Header("�I����UI")]
    private FinishUI _finishUI;

    #region �Q�b�^�[
    public bool IsPlaying { get { return _isPlaying; } }

    public float TimeLimit { get { return _timeLimit; } }

    public float CountDown { get { return _countDown; } }

    public AudioClip ThrowBombSE { get { return _throwBombSE; } }

    public AudioClip SpeedUpSE { get { return _speedUpSE; } }

    public int MaxHaveItem { get { return _maxHaveItem; } }

    public float ItemLimit { get { return _itemLimit; } }

    public float InkHealValue_Item { get { return _inkHealValue_Item; } }

    public float ItemSpawnTime { get { return _itemSpawnTime; } }

    public GameObject Bomb { get { return _bomb; } }

    public float StunTime { get { return _stunTime; } }

    public float PlayerAccelerationValue { get { return _playerAccelerationValue; } }

    public float PlayerInkDecValue { get { return _playerInkDecValue; } }

    public float GetItemInkUpValue { get { return _getItemInkUpValue; } }

    public int MapTileNum() { return _mapTiles.Length; }

    public int DefaultTileNum { get { return _defaultTileNum; } }

    public int RedTileNum { get { return _redTileNum; } }

    public int BlueTileNum { get { return _blueTileNum; } }

    public int GreenTileNum { get { return _greenTileNum; } }

    public int PurpleTileNum { get { return _purpleTileNum; } }


    #endregion

    void Start()
    {

        //�}�b�v�̃^�C�����ŏ��Ɏ擾���Ă���
        _mapTiles = GameObject.FindGameObjectsWithTag("MapTile");

        //�}�b�v�̃^�C���̃}�e���A�����擾
        foreach (GameObject obj in _mapTiles)
        {
            //���b�V�������_�\���̂�o�^���Ă���1
            _mapTilesRender.Add(obj.GetComponent<MeshRenderer>());
        }
    }

    void Update()
    {
        //�Q�[�������ᖳ�������ꍇ
        if (!_isPlaying)
        {
            //�J�E���g�_�E���������čs��
            _countDown -= Time.deltaTime;
            //�J�E���g�_�E����0�ɂȂ�����
            if (_countDown <= 0)
            {
                _countDown = 0;
                //�Q�[���̃v���C�����ǂ�����true�ɂ���
                _isPlaying = true;
                _normalAudio.Play();
            }
        }
        //�Q�[�����������ꍇ
        else
        {
            if (_timeLimit >= 0)
            {
                //�������Ԃ������Ă���
                _timeLimit -= Time.deltaTime;
            }

            //�c�莞�Ԃ�20�b�؂�����
            if (_timeLimit <= 20)
            {
                //BGM���ύX����Ă��Ȃ�������
                if (!_changeBGM)
                {
                    //�ʏ펞��BGM���~�߂�
                    _normalAudio.Stop();
                    //���X�g�X�p�[�g��BGM�𗬂�
                    _lastAudio.Play();
                    //BGM��ς����t���O���I����
                    _changeBGM = true;
                    //�A�C�e���̃��X�|�[�����Ԃ�L�΂�
                    _itemSpawnTime = _lastItemSpawnTime;

                    //���[����
                    RenderSettings.skybox = _lastSkyBox;
                    //�g������
                    _mapFence.SetActive(false);
                }
            }

            //�������Ԃ�0�ɂȂ�����
            if (_timeLimit <= 0)
            {
                if (_finishUI.FinishAction())
                {
                    //�X�R�A�̃Z�b�g���܂���������
                    if (!_endScoreSet)
                    {
                        //�X�R�A���Z�b�g����
                        RankingSet();
                        //�����j���O�V�[���Ɉړ�
                        ChangeScene.ChengeNextScene("RankingScene", 0.1f);
                        _endScoreSet = true;
                    }
                }
            }

            //�Q�[�����̓p�l���𐔂�������
            CheckMapTilesColor();
        }
    }

    //���݂̃p�l���̖������m�F
    private void CheckMapTilesColor()
    {
        //���ꂼ��̃p�l���̖��������Z�b�g
        _defaultTileNum = 0;
        _redTileNum = 0;
        _blueTileNum = 0;
        _greenTileNum = 0;
        _purpleTileNum = 0;
        //�����mapTile�̃����_���[�̃}�e���A�����m�F
        foreach (MeshRenderer renderer in _mapTilesRender)
        {
            //�F���ς���Ă��Ȃ������ꍇ
            if (renderer.material.color == _defaultMapTileColor.color)
            {
                _defaultTileNum++;
            }
            //�Ԃ������ꍇ
            if (renderer.material.color == _redPlayerMat.color)
            {
                _redTileNum++;
            }
            //�������ꍇ
            if (renderer.material.color == _bluePlayerMat.color)
            {
                _blueTileNum++;
            }
            //�΂������ꍇ
            if (renderer.material.color == _greenPlayerMat.color)
            {
                _greenTileNum++;
            }
            //���������ꍇ
            if (renderer.material.color == _purplePlayerMat.color)
            {
                _purpleTileNum++;
            }
        }
    }

    //�����L���O���Z�b�g
    public void RankingSet()
    {
        //�����L���O�f�[�^�̕��ɉ��l�v���C�����Z�b�g
        RankingData._playerNum = _playerNum;

        //�v���C���[�����N���A
        RankingData.playerDatas.Clear();

        //�z���MapTail�����I�u�W�F�N�g������
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("MapTile");

        //���Ԗڂ̃v���C���[�����m�F
        int playerCount = 0;

        //�}�e���A���𐔂���
        foreach (CarController mat in _carControllers)
        {
            //�}�e���A���̐����v���C���[�̃}�e���A�����ɐ�����
            int count = 0;

            foreach (GameObject obj in _mapTiles)
            {
                Material objMat = obj.GetComponent<MeshRenderer>().material;
                if (objMat.color == mat.MyColor.color)
                {
                    count++;
                }
            }

            //�����ŃZ�b�g
            RankingData.playerDatas.Add(new PlayerData(mat.Name, count, mat.MyColor, mat.ModelNum));
            playerCount++;
        }
    }

    //���X�g�ɃJ�[�R���g���[���[��o�^
    public void SetCarController(CarController controller)
    {
        _carControllers.Add(controller);
    }

    //�A�C�e���̑S���̃J�����̉�]
    public void Item_CameraRotate_All(CarController controller)
    {
        foreach (CarController car in _carControllers)
        {
            //�Ăяo�����{�l����Ȃ������ꍇ
            if (car != controller)
            {
                car.HitCameraRotate();
            }
        }
    }

    //�A�C�e���̑S���X�^��
    public void Item_Stun_All(CarController controller)
    {
        foreach (CarController car in _carControllers)
        {
            //�Ăяo�����{�l����Ȃ������ꍇ
            if (car != controller)
            {
                car.HitStun();
            }
        }
    }
}
