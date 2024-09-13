using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CarController : MonoBehaviour
{
    //���W�b�h�{�f�B
    Rigidbody _rb;
    //�Q�[���}�l�[�W���[
    private GameManager _gm;

    #region �Ԃ̊�b�X�e�[�^�X
    [SerializeField, Header("�Ԃ̊�{�I�ȉ�����")]
    private float _defaultSpeed = 500f;

    [SerializeField, Header("�Ԃ̍ő�X�s�[�h")]
    private float _defaultMaxSpeed = 20f;

    [SerializeField, Header("�Ԃ̉�]�l")]
    private float _defaultRotateValue = 1.0f;

    [SerializeField, Header("�J�����̉�]�̒l")]
    private float _defaultCameraRotateSpeed = 2.0f;

    [SerializeField, Header("�ŏ��̍ő�C���N��")]
    private float _defaultMaxInk = 100f;

    [SerializeField, Header("�Ԃ̋����ł���C���N�ʂ̍ő�l")]
    private float _powerUpMaxInk = 450;

    [SerializeField, Header("�C���N�̉񕜑��x")]
    private float _defaultInkHealSpeed = 2f;

    //�������Ŏg���̂ł��������g��
    private float _speed;//���x

    private float _maxSpeed;//�ő呬�x

    private float _cameraRotateSpeed;//�J�����̉�]���x

    private float _maxInk;//�Ԃ̃C���N�̃^���N�̍ő�l

    private float _ink;//���݂̃C���N�̗�

    private float _inkHealSpeed;//�C���N�̉񕜑��x

    private Material _myColor;//�����̐F

    private GameObject _useModel;//�g���Ă��郂�f��

    private int _modelNum;//�g���Ă��郂�f�������Ԗڂ�

    private string _name;//���O
    #endregion


    //�J�����ɃM�~�b�N���������Ă��邩�ǂ���
    private bool _onCameraGimmick = false;

    //�J�����̃M�~�b�N�̌��ʎ��Ԃ̃J�E���g
    private float _cameraGimmickCounter = 0f;

    [Header("-�������牺�͂��܂肢����Ȃ������ǂ�����-")]
    [SerializeField, Header("�J�����R���g���[���[")]
    //�J�����R���g���[���[
    private CameraController _cameraController;

    [SerializeField, Header("�J�[�g�̃��f��������")]
    private GameObject[] _cartModels;

    [SerializeField, Header("���G�t�F�N�g������")]
    private GameObject[] _smokeEffects;

    [SerializeField, Header("�J�[�g�̃^�C���̐F������")]
    private Material[] _cartTileColors;

    #region �A�C�e���֌W�̕ϐ�
    //�������Ă���A�C�e���̃��X�g
    private List<ItemType> _haveItems = new List<ItemType>();

    //����A�C�e�����g���邩
    private int _useItemuNum = 3;

    public int UseItemNum { get { return _useItemuNum; } }

    //�X�^���ɓ������Ă邩�ǂ���
    private bool _hitStun = false;

    //�X�^���̃^�C�}�[
    private float _stunTimer = 0f;

    #endregion

    //�J�����֌W
    private bool _moveFront = false;

    [SerializeField, Header("�O���[�̃V�F�[�_�[")]
    private GrayscaleEffect _grayscaleEffect;

    //���y�֌W
    private AudioSource _audioSource;

    #region �Q�b�^�[

    public float DefMaxInk { get { return _defaultMaxInk; } }

    public float MaxInk { get { return _maxInk; } }

    public float Ink { get { return _ink; } }

    public Material MyColor { get { return _myColor; } }

    public GameObject UseModel { get { return _useModel; } }

    public int ModelNum { get { return _modelNum; } }

    public string Name { get { return _name; } }

    public bool MoveFront { get { return _moveFront; } }
    #endregion

    #region Input�V�X�e���̒l�ۑ��p�̕ϐ�
    //���̓���(�ړ��Ɏg��)
    private float _inputMoveX;

    //�c�̓���(�ړ��Ɏg��)
    private float _inputMoveY;

    //�X�v���[���˂�������Ă��邩
    private bool _downDrawColor = false;

    //���_�ړ��Ɏg��
    private float _lookValue;

    private PlayerInput _playerInput;
    #endregion

    #region PlayerInputSystem�̒l�擾�֐�
    //�����PlayerInput�̒l���擾
    //�ړ��֌W�̒l�̎擾
    public void OnMove(InputAction.CallbackContext context)
    {
        // MoveAction�̓��͒l���擾
        var axis = context.ReadValue<Vector2>();

        //���͂��ꂽ�l�̕ۑ�
        _inputMoveY = axis.y;
        _inputMoveX = axis.x;
    }

    public void OnDrawColor(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // �{�^���������ꂽ���̏���
                _downDrawColor = true;
                break;

            case InputActionPhase.Canceled:
                // �{�^���������ꂽ���̏���
                _downDrawColor = false;
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // MoveAction�̓��͒l���擾
        var value = context.ReadValue<Vector2>();

        //���͂��ꂽ�l�̕ۑ�
        _lookValue = -value.x;
    }

    //�U������̒l�̎擾
    public void OnAttack(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;
        UseItem();
    }

    #endregion

    void Start()
    {
        //���W�b�h�{�f�B��o�^
        _rb = GetComponent<Rigidbody>();

        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();

        //AudioSource�擾
        _audioSource = GetComponent<AudioSource>();

        //���������X�g�ɓo�^
        _gm.SetCarController(this);

        //�ő�HP���Z�b�g
        _maxInk = _defaultMaxInk;

        //���݂�HP�ɍő�HP��ۑ�
        _ink = _defaultMaxInk;

        //�C���N�̉񕜑��x���Z�b�g
        _inkHealSpeed = _defaultInkHealSpeed;

        //�X�s�[�h���Z�b�g
        _speed = _defaultSpeed;

        //�J�����̊��x���Z�b�g
        _cameraRotateSpeed = _defaultCameraRotateSpeed;

        //�ő呬�x���Z�b�g
        _maxSpeed = _defaultMaxSpeed;

        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        //�J�����̃M�~�b�N����
        //�M�~�b�N�ɂ������Ă�����
        if (_onCameraGimmick)
        {
            _cameraGimmickCounter += Time.deltaTime;
            if (_gm.ItemLimit <= _cameraGimmickCounter)
            {
                //���������񂵂Ă��Ƃɖ߂�
                CameraRotate();
                //�J�����̃M�~�b�N�H����Ă锻����I�t�ɂ���
                _onCameraGimmick = false;
                //�J�E���^�[���Z�b�g
                _cameraGimmickCounter = 0;
            }
        }

        //�X�^���ɓ������Ă�����
        if (_hitStun)
        {
            //�X�^���̎��Ԃ������Ă���
            _stunTimer -= Time.deltaTime;
            //���Ԃ�0�ɂȂ�����
            if (_stunTimer < 0)
            {
                //���Ԃ�0�ɂ���
                _stunTimer = 0;
                //��ʂ̃O���[��߂�
                _grayscaleEffect.GreyScale();
                //�X�^����߂�
                _hitStun = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //�ړ������͂�����
        //�Q�[�����������Ă�����
        if (_gm.IsPlaying)
        {
            //�X�^�����Ă��Ȃ������ꍇ
            if (!_hitStun)
            {
                //�ړ�
                MoveLength(_inputMoveY);
                //�J�����̃M�~�b�N�ɓ������Ă��Ȃ�������
                if (!_onCameraGimmick)
                {
                    //�ʏ�̌����ŉ�]
                    Turn(_inputMoveX);
                }
                //�J�����̃M�~�b�N�ɓ������Ă����ꍇ
                else
                {
                    //���씽�]
                    Turn(-_inputMoveX);
                }
            }

            //�J���������X�ɖ߂�
            _cameraController.InitPosReturn(_moveFront);
            //�J�����̉�]
            _cameraController.CameraOperation(_lookValue);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�}�b�v�^�C���ɓ��������ꍇ
        if (other.gameObject.CompareTag("MapTile"))
        {
            var renderer = other.gameObject.GetComponent<Renderer>();
            //�Q�[�������A�X�v���[���˒��̂�
            if (_downDrawColor && _gm.IsPlaying)
            {
                if (_ink > 0)
                {
                    //�F��h��
                    renderer.material = _myColor;
                    //���̃G�t�F�N�g���Z�b�g
                    _smokeEffects[_modelNum].SetActive(true);
                    //��U���ŃC���N
                    _ink -= Time.deltaTime * _gm.PlayerInkDecValue;
                    if (_ink <= 0)
                    {
                        _ink = 0;
                        _smokeEffects[_modelNum].SetActive(false);
                    }
                }

            }
            else
            {
                //���̃G�t�F�N�g����
                _smokeEffects[_modelNum].SetActive(false);
            }
        }
    }

    //�v���C���[�̏����̒l���Z�b�g
    //���f���ƐF
    public void SetFirst(CartType type, CartColor color)
    {
        //�J�[�g���f��������
        for (int i = 0; i < _cartModels.Count(); i++)
        {
            //�^�C�v�̔ԍ�����v������A�N�e�B�u��
            if (i == (int)type)
            {
                _cartModels[i].SetActive(true);
                _useModel = _cartModels[i];//�g���Ă��郂�f�����Z�b�g
                _modelNum = i;//���Ԗڂ̃��f�����g���Ă邩��ۑ�
                _name = color.ToString();//���O�ɐF���Z�b�g
            }
            else
            {
                _cartModels[i].SetActive(false);
            }
        }

        //�J���[���Z�b�g
        _myColor = _cartTileColors[(int)color];
    }

    //�ǂ̃A�C�e���������Ă��邩��n��
    public ItemType Get1stItem()
    {
        //�A�C�e���������Ă��Ȃ������ꍇ
        if (_haveItems.Count == 0)
        {
            return ItemType.None;
        }
        else
        {
            return _haveItems[0];
        }
    }

    public bool Have1StItem()
    {
        //�A�C�e���������Ă��Ȃ������ꍇ
        if (_haveItems.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public ItemType Get2ndItem()
    {
        //�����Ă���A�C�e����1�ȉ��������ꍇ
        if (_haveItems.Count <= 1)
        {
            return ItemType.None;
        }
        else
        {
            return _haveItems[1];
        }
    }

    //�A�C�e���擾�̊֐�
    public void GetItem(ItemType type)
    {
        //�����Ă���A�C�e���̐������Ă鐔�̏���ȉ���������
        if (_haveItems.Count < _gm.MaxHaveItem)
        {
            //�A�C�e���������Ă��Ȃ������ꍇ
            if (_haveItems.Count == 0)
            {
                if (type == ItemType.Attack)
                {
                    _useItemuNum = 3;
                }
                else
                {
                    _useItemuNum = 1;
                }
            }
            //�����Ă���A�C�e����ǉ�����
            _haveItems.Add(type);
            //�C���N�̍ő�l���グ��
            InkMaxUp();
        }
    }

    //�A�C�e�����g���֐�
    private void UseItem()
    {

        //�X�^��������Ȃ���
        if(!_hitStun)
        {
            //�A�C�e�����X�g�ɃA�C�e���������Ă�����
            if (_haveItems.Count > 0)
            {
                switch (_haveItems[0])
                {
                    case ItemType.Attack://�U��(����)
                                         //��������
                        Acceleration();
                        _useItemuNum -= 1;
                        if (_useItemuNum <= 0)
                        {
                            //�g�����A�C�e��������
                            _haveItems.RemoveAt(0);
                            //��������ɃA�C�e�����c���Ă�����
                            if (_haveItems.Count > 0)
                            {
                                if (_haveItems[0] == ItemType.Attack)
                                {
                                    _useItemuNum = 3;
                                }
                                else
                                {
                                    _useItemuNum = 1;
                                }
                            }
                        }
                        break;

                    case ItemType.Bomb://���e
                        ThrowBomb();
                        //�g�����A�C�e��������
                        _haveItems.RemoveAt(0);
                        //��������ɃA�C�e�����c���Ă�����
                        if (_haveItems.Count > 0)
                        {
                            if (_haveItems[0] == ItemType.Attack)
                            {
                                _useItemuNum = 3;
                            }
                            else
                            {
                                _useItemuNum = 1;
                            }
                        }
                        break;

                    case ItemType.Camera_Rotate://�J����(��])
                        _gm.Item_CameraRotate_All(this);
                        //�g�����A�C�e��������
                        _haveItems.RemoveAt(0);
                        //��������ɃA�C�e�����c���Ă�����
                        if (_haveItems.Count > 0)
                        {
                            if (_haveItems[0] == ItemType.Attack)
                            {
                                _useItemuNum = 3;
                            }
                            else
                            {
                                _useItemuNum = 1;
                            }
                        }
                        break;

                    case ItemType.Stun://�X�^��
                        _gm.Item_Stun_All(this);
                        //�g�����A�C�e��������
                        _haveItems.RemoveAt(0);
                        //��������ɃA�C�e�����c���Ă�����
                        if (_haveItems.Count > 0)
                        {
                            if (_haveItems[0] == ItemType.Attack)
                            {
                                _useItemuNum = 3;
                            }
                            else
                            {
                                _useItemuNum = 1;
                            }
                        }
                        break;
                }
            }
        }
    }

    //�A�C�e���擾���̃C���N��
    public void InkHeal()
    {
        //�ő�̃C���N�ʂ��牽������
        _ink += _maxInk * _gm.InkHealValue_Item;
        //�C���N���ő�l�𒴂�����
        if (_ink >= _maxInk)
        {
            //�C���N���ő�l��
            _ink = _maxInk;
        }
    }

    //���e�𓊂���
    private void ThrowBomb()
    {
        //����炷
        _audioSource.PlayOneShot(_gm.ThrowBombSE);
        //���e�𐶐�
        Instantiate(_gm.Bomb, this.transform.position, this.transform.rotation).GetComponent<BombController>().SetPlayerColor(this, _myColor);
    }

    //�J������]�̍U����H������ꍇ
    public void HitCameraRotate()
    {
        //�J�����̃M�~�b�N��H����Ă��Ȃ�������
        if (!_onCameraGimmick)
        {
            //�M�~�b�N��H����Ă��锻���true��
            _onCameraGimmick = true;
            //�J��������
            CameraRotate();
        }
    }

    //�c�̈ړ�
    private void MoveLength(float value)
    {
        _rb.AddForce(transform.forward * (_speed + 500f) * value * Time.deltaTime, ForceMode.Acceleration);

        //�@�ړ����x�̊m�F
        float horizontalSpeed = (float)Math.Sqrt(Math.Pow(_rb.velocity.x, 2) + Math.Pow(_rb.velocity.z, 2));

        // ���͂��O�����x���o�Ă����ꍇ
        if (value >= 0 && horizontalSpeed > 0)
        {
            //�@�O�ɓ����Ă��锻���true��
            _moveFront = true;
        }
        //�@����ȊO�͑O�ɐi��ł��锻���false��
        else
        {
            _moveFront = false;
        }
    }

    //��]
    private void Turn(float value)
    {
        transform.Rotate(new Vector3(0f, _defaultRotateValue * value, 0f));
    }

    //����
    private void Acceleration()
    {
        //����炷
        _audioSource.PlayOneShot(_gm.SpeedUpSE);
        //���W�b�h�{�f�B�ŉ���
        _rb.AddForce(transform.forward * _gm.PlayerAccelerationValue, ForceMode.Impulse);
    }

    //�J��������]
    //�A�C�e���g�p���ɂ킩��₷�����邽��
    public void CameraRotate()
    {
        _cameraController.CameraRotate();
    }

    //�C���N�̍ő�l���X�V
    public void InkMaxUp()
    {
        _maxInk += _gm.GetItemInkUpValue;
        //�ő�l�𒴂����ꍇ
        if (_maxInk >= _powerUpMaxInk)
        {
            //�ő�l�ɂ���
            _maxInk = _powerUpMaxInk;
        }
    }

    //�X�^����H�������
    public void HitStun()
    {
        //�X�^���ɓ������Ă��Ȃ������ꍇ
        if (!_hitStun)
        {
            //�X�^�����Ă锻����I����
            _hitStun = true;
            //�X�^���̎��Ԃ��Z�b�g
            _stunTimer = _gm.StunTime;
            //��ʂ��O���[��
            _grayscaleEffect.GreyScale();
        }
    }
}
