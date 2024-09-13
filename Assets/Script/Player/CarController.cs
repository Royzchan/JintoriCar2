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
    //リジッドボディ
    Rigidbody _rb;
    //ゲームマネージャー
    private GameManager _gm;

    #region 車の基礎ステータス
    [SerializeField, Header("車の基本的な加速量")]
    private float _defaultSpeed = 500f;

    [SerializeField, Header("車の最大スピード")]
    private float _defaultMaxSpeed = 20f;

    [SerializeField, Header("車の回転値")]
    private float _defaultRotateValue = 1.0f;

    [SerializeField, Header("カメラの回転の値")]
    private float _defaultCameraRotateSpeed = 2.0f;

    [SerializeField, Header("最初の最大インク量")]
    private float _defaultMaxInk = 100f;

    [SerializeField, Header("車の強化できるインク量の最大値")]
    private float _powerUpMaxInk = 450;

    [SerializeField, Header("インクの回復速度")]
    private float _defaultInkHealSpeed = 2f;

    //減速等で使うのでこっちを使う
    private float _speed;//速度

    private float _maxSpeed;//最大速度

    private float _cameraRotateSpeed;//カメラの回転速度

    private float _maxInk;//車のインクのタンクの最大値

    private float _ink;//現在のインクの量

    private float _inkHealSpeed;//インクの回復速度

    private Material _myColor;//自分の色

    private GameObject _useModel;//使っているモデル

    private int _modelNum;//使っているモデルが何番目か

    private string _name;//名前
    #endregion


    //カメラにギミックがかかっているかどうか
    private bool _onCameraGimmick = false;

    //カメラのギミックの効果時間のカウント
    private float _cameraGimmickCounter = 0f;

    [Header("-ここから下はあまりいじらない方が良いかも-")]
    [SerializeField, Header("カメラコントローラー")]
    //カメラコントローラー
    private CameraController _cameraController;

    [SerializeField, Header("カートのモデルを入れる")]
    private GameObject[] _cartModels;

    [SerializeField, Header("煙エフェクトを入れる")]
    private GameObject[] _smokeEffects;

    [SerializeField, Header("カートのタイルの色を入れる")]
    private Material[] _cartTileColors;

    #region アイテム関係の変数
    //所持しているアイテムのリスト
    private List<ItemType> _haveItems = new List<ItemType>();

    //何回アイテムを使えるか
    private int _useItemuNum = 3;

    public int UseItemNum { get { return _useItemuNum; } }

    //スタンに当たってるかどうか
    private bool _hitStun = false;

    //スタンのタイマー
    private float _stunTimer = 0f;

    #endregion

    //カメラ関係
    private bool _moveFront = false;

    [SerializeField, Header("グレーのシェーダー")]
    private GrayscaleEffect _grayscaleEffect;

    //音楽関係
    private AudioSource _audioSource;

    #region ゲッター

    public float DefMaxInk { get { return _defaultMaxInk; } }

    public float MaxInk { get { return _maxInk; } }

    public float Ink { get { return _ink; } }

    public Material MyColor { get { return _myColor; } }

    public GameObject UseModel { get { return _useModel; } }

    public int ModelNum { get { return _modelNum; } }

    public string Name { get { return _name; } }

    public bool MoveFront { get { return _moveFront; } }
    #endregion

    #region Inputシステムの値保存用の変数
    //横の入力(移動に使う)
    private float _inputMoveX;

    //縦の入力(移動に使う)
    private float _inputMoveY;

    //スプレー噴射が押されているか
    private bool _downDrawColor = false;

    //視点移動に使う
    private float _lookValue;

    private PlayerInput _playerInput;
    #endregion

    #region PlayerInputSystemの値取得関数
    //これでPlayerInputの値を取得
    //移動関係の値の取得
    public void OnMove(InputAction.CallbackContext context)
    {
        // MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        //入力された値の保存
        _inputMoveY = axis.y;
        _inputMoveX = axis.x;
    }

    public void OnDrawColor(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                _downDrawColor = true;
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                _downDrawColor = false;
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // MoveActionの入力値を取得
        var value = context.ReadValue<Vector2>();

        //入力された値の保存
        _lookValue = -value.x;
    }

    //攻撃判定の値の取得
    public void OnAttack(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        UseItem();
    }

    #endregion

    void Start()
    {
        //リジッドボディを登録
        _rb = GetComponent<Rigidbody>();

        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();

        //AudioSource取得
        _audioSource = GetComponent<AudioSource>();

        //自分をリストに登録
        _gm.SetCarController(this);

        //最大HPをセット
        _maxInk = _defaultMaxInk;

        //現在のHPに最大HPを保存
        _ink = _defaultMaxInk;

        //インクの回復速度をセット
        _inkHealSpeed = _defaultInkHealSpeed;

        //スピードをセット
        _speed = _defaultSpeed;

        //カメラの感度をセット
        _cameraRotateSpeed = _defaultCameraRotateSpeed;

        //最大速度をセット
        _maxSpeed = _defaultMaxSpeed;

        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        //カメラのギミック解除
        //ギミックにかかっていたら
        if (_onCameraGimmick)
        {
            _cameraGimmickCounter += Time.deltaTime;
            if (_gm.ItemLimit <= _cameraGimmickCounter)
            {
                //もっかい回してもとに戻す
                CameraRotate();
                //カメラのギミック食らってる判定をオフにして
                _onCameraGimmick = false;
                //カウンターリセット
                _cameraGimmickCounter = 0;
            }
        }

        //スタンに当たっていたら
        if (_hitStun)
        {
            //スタンの時間を引いていく
            _stunTimer -= Time.deltaTime;
            //時間が0になったら
            if (_stunTimer < 0)
            {
                //時間を0にして
                _stunTimer = 0;
                //画面のグレーを戻す
                _grayscaleEffect.GreyScale();
                //スタンを戻す
                _hitStun = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //移動処理はこっち
        //ゲーム中かつ生きていたら
        if (_gm.IsPlaying)
        {
            //スタンしていなかった場合
            if (!_hitStun)
            {
                //移動
                MoveLength(_inputMoveY);
                //カメラのギミックに当たっていなかったら
                if (!_onCameraGimmick)
                {
                    //通常の向きで回転
                    Turn(_inputMoveX);
                }
                //カメラのギミックに当たっていた場合
                else
                {
                    //操作反転
                    Turn(-_inputMoveX);
                }
            }

            //カメラを徐々に戻す
            _cameraController.InitPosReturn(_moveFront);
            //カメラの回転
            _cameraController.CameraOperation(_lookValue);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //マップタイルに当たった場合
        if (other.gameObject.CompareTag("MapTile"))
        {
            var renderer = other.gameObject.GetComponent<Renderer>();
            //ゲーム中かつ、スプレー噴射中のみ
            if (_downDrawColor && _gm.IsPlaying)
            {
                if (_ink > 0)
                {
                    //色を塗る
                    renderer.material = _myColor;
                    //煙のエフェクトをセット
                    _smokeEffects[_modelNum].SetActive(true);
                    //一旦仮でインク
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
                //煙のエフェクト消す
                _smokeEffects[_modelNum].SetActive(false);
            }
        }
    }

    //プレイヤーの初期の値をセット
    //モデルと色
    public void SetFirst(CartType type, CartColor color)
    {
        //カートモデルを見る
        for (int i = 0; i < _cartModels.Count(); i++)
        {
            //タイプの番号が一致したらアクティブに
            if (i == (int)type)
            {
                _cartModels[i].SetActive(true);
                _useModel = _cartModels[i];//使っているモデルをセット
                _modelNum = i;//何番目のモデルを使ってるかを保存
                _name = color.ToString();//名前に色をセット
            }
            else
            {
                _cartModels[i].SetActive(false);
            }
        }

        //カラーをセット
        _myColor = _cartTileColors[(int)color];
    }

    //どのアイテムを持っているかを渡す
    public ItemType Get1stItem()
    {
        //アイテムを持っていなかった場合
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
        //アイテムを持っていなかった場合
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
        //持っているアイテムが1個以下だった場合
        if (_haveItems.Count <= 1)
        {
            return ItemType.None;
        }
        else
        {
            return _haveItems[1];
        }
    }

    //アイテム取得の関数
    public void GetItem(ItemType type)
    {
        //持っているアイテムの数が持てる数の上限以下だったら
        if (_haveItems.Count < _gm.MaxHaveItem)
        {
            //アイテムを持っていなかった場合
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
            //持っているアイテムを追加する
            _haveItems.Add(type);
            //インクの最大値を上げる
            InkMaxUp();
        }
    }

    //アイテムを使う関数
    private void UseItem()
    {

        //スタン中じゃなくて
        if(!_hitStun)
        {
            //アイテムリストにアイテムが入っていたら
            if (_haveItems.Count > 0)
            {
                switch (_haveItems[0])
                {
                    case ItemType.Attack://攻撃(減速)
                                         //加速する
                        Acceleration();
                        _useItemuNum -= 1;
                        if (_useItemuNum <= 0)
                        {
                            //使ったアイテムを消す
                            _haveItems.RemoveAt(0);
                            //消した後にアイテムが残っていたら
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

                    case ItemType.Bomb://爆弾
                        ThrowBomb();
                        //使ったアイテムを消す
                        _haveItems.RemoveAt(0);
                        //消した後にアイテムが残っていたら
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

                    case ItemType.Camera_Rotate://カメラ(回転)
                        _gm.Item_CameraRotate_All(this);
                        //使ったアイテムを消す
                        _haveItems.RemoveAt(0);
                        //消した後にアイテムが残っていたら
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

                    case ItemType.Stun://スタン
                        _gm.Item_Stun_All(this);
                        //使ったアイテムを消す
                        _haveItems.RemoveAt(0);
                        //消した後にアイテムが残っていたら
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

    //アイテム取得時のインク回復
    public void InkHeal()
    {
        //最大のインク量から何割か回復
        _ink += _maxInk * _gm.InkHealValue_Item;
        //インクが最大値を超えたら
        if (_ink >= _maxInk)
        {
            //インクを最大値に
            _ink = _maxInk;
        }
    }

    //爆弾を投げる
    private void ThrowBomb()
    {
        //音を鳴らす
        _audioSource.PlayOneShot(_gm.ThrowBombSE);
        //爆弾を生成
        Instantiate(_gm.Bomb, this.transform.position, this.transform.rotation).GetComponent<BombController>().SetPlayerColor(this, _myColor);
    }

    //カメラ回転の攻撃を食らった場合
    public void HitCameraRotate()
    {
        //カメラのギミックを食らっていなかったら
        if (!_onCameraGimmick)
        {
            //ギミックを食らっている判定をtrueに
            _onCameraGimmick = true;
            //カメラを回す
            CameraRotate();
        }
    }

    //縦の移動
    private void MoveLength(float value)
    {
        _rb.AddForce(transform.forward * (_speed + 500f) * value * Time.deltaTime, ForceMode.Acceleration);

        //　移動速度の確認
        float horizontalSpeed = (float)Math.Sqrt(Math.Pow(_rb.velocity.x, 2) + Math.Pow(_rb.velocity.z, 2));

        // 入力が前かつ速度が出ていた場合
        if (value >= 0 && horizontalSpeed > 0)
        {
            //　前に動いている判定をtrueに
            _moveFront = true;
        }
        //　それ以外は前に進んでいる判定をfalseに
        else
        {
            _moveFront = false;
        }
    }

    //回転
    private void Turn(float value)
    {
        transform.Rotate(new Vector3(0f, _defaultRotateValue * value, 0f));
    }

    //加速
    private void Acceleration()
    {
        //音を鳴らす
        _audioSource.PlayOneShot(_gm.SpeedUpSE);
        //リジッドボディで加速
        _rb.AddForce(transform.forward * _gm.PlayerAccelerationValue, ForceMode.Impulse);
    }

    //カメラを回転
    //アイテム使用時にわかりやすくするため
    public void CameraRotate()
    {
        _cameraController.CameraRotate();
    }

    //インクの最大値を更新
    public void InkMaxUp()
    {
        _maxInk += _gm.GetItemInkUpValue;
        //最大値を超えた場合
        if (_maxInk >= _powerUpMaxInk)
        {
            //最大値にする
            _maxInk = _powerUpMaxInk;
        }
    }

    //スタンを食らった時
    public void HitStun()
    {
        //スタンに当たっていなかった場合
        if (!_hitStun)
        {
            //スタンしてる判定をオンに
            _hitStun = true;
            //スタンの時間をセット
            _stunTimer = _gm.StunTime;
            //画面をグレーに
            _grayscaleEffect.GreyScale();
        }
    }
}
