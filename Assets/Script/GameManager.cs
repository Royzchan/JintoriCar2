using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("～プレイヤー関係～")]

    [SerializeField, Header("プレイ人数")]
    private int _playerNum = 2;

    [Header("ここは四人登録しておく")]
    [SerializeField, Header("プレイヤーマテリアルを入れる")]
    private Material[] _playerMats;

    [SerializeField, Header("プレイヤーマテリアルに対応する名前を入れる")]
    private string[] _playerNames;

    [Header("～時間関係～")]

    [SerializeField, Header("カウントダウンの時間")]
    private float _countDown = 3.0f;

    [SerializeField, Header("ゲームの制限時間")]
    private float _timeLimit = 180f;

    [Header("～アイテム関係～")]

    [SerializeField, Header("アイテム取得時の回復割合"), Range(0.1f, 1.0f)]
    private float _inkHealValue_Item = 0.3f;

    [SerializeField, Header("アイテムのスポーン時間")]
    private float _itemSpawnTime = 5.0f;

    [SerializeField, Header("ゲーム後半のアイテムのスポーン時間")]
    private float _lastItemSpawnTime = 8.0f;

    [SerializeField, Header("何個アイテムを持てるか")]
    private int _maxHaveItem = 3;

    [SerializeField, Header("アイテムの効果時間")]
    private float _itemLimit = 3.0f;

    [SerializeField, Header("爆弾のプレファブ")]
    private GameObject _bomb;

    [SerializeField, Header("スタンの時間")]
    private float _stunTime = 5.0f;

    [Header("～システム関連～")]
    [SerializeField, Header("プレイヤーの加速量")]
    private float _playerAccelerationValue = 5.0f;

    [SerializeField, Header("インクの減少量")]
    private float _playerInkDecValue = 3.0f;

    [SerializeField, Header("アイテム取得時のインクの増加量")]
    private float _getItemInkUpValue = 50.0f;

    [SerializeField,Header("後半のスカイボックス")]
    private Material _lastSkyBox;

    [Header("～音楽関係～")]
    [SerializeField, Header("通常時のBGMのAudioSource")]
    private AudioSource _normalAudio;

    [SerializeField, Header("終盤時のBGMのAudioSource")]
    private AudioSource _lastAudio;
    //BGMが変わったかどうか
    private bool _changeBGM = false;

    [SerializeField, Header("速度アップ時のSE")]
    private AudioClip _speedUpSE;

    [SerializeField, Header("爆弾投下時のSE")]
    private AudioClip _throwBombSE;

    [Header("～マップ関係～")]
    [SerializeField, Header("最初のマップタイルの色")]
    private Material _defaultMapTileColor;

    [SerializeField, Header("赤のプレイヤーのマテリアル")]
    private Material _redPlayerMat;

    [SerializeField, Header("青のプレイヤーのマテリアル")]
    private Material _bluePlayerMat;

    [SerializeField, Header("緑のプレイヤーのマテリアル")]
    private Material _greenPlayerMat;

    [SerializeField, Header("紫のプレイヤーのマテリアル")]
    private Material _purplePlayerMat;

    [SerializeField, Header("マップの枠")]
    private GameObject _mapFence;


    //角タイルの枚数を保存
    //こっちは数を数える用
    private int _defaultTileNum = 0;
    private int _redTileNum = 0;
    private int _blueTileNum = 0;
    private int _greenTileNum = 0;
    private int _purpleTileNum = 0;

    /*内部処理用*/
    //ゲーム中かどうか
    private bool _isPlaying = false;

    //スコアセット済みか
    private bool _endScoreSet = false;

    //プレイヤーを保存しておく場所
    //全体のカメラ関係はゲームマネージャーで
    List<CarController> _carControllers = new List<CarController>();

    //マップのタイルを保存する変数
    GameObject[] _mapTiles;

    //マップタイルのレンダラー
    List<MeshRenderer> _mapTilesRender = new List<MeshRenderer>();

    [SerializeField, Header("終了のUI")]
    private FinishUI _finishUI;

    #region ゲッター
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

        //マップのタイルを最初に取得しておく
        _mapTiles = GameObject.FindGameObjectsWithTag("MapTile");

        //マップのタイルのマテリアルも取得
        foreach (GameObject obj in _mapTiles)
        {
            //メッシュレンダ―自体を登録しておく1
            _mapTilesRender.Add(obj.GetComponent<MeshRenderer>());
        }
    }

    void Update()
    {
        //ゲーム中じゃ無かった場合
        if (!_isPlaying)
        {
            //カウントダウンを下げて行く
            _countDown -= Time.deltaTime;
            //カウントダウンが0になったら
            if (_countDown <= 0)
            {
                _countDown = 0;
                //ゲームのプレイ中かどうかをtrueにする
                _isPlaying = true;
                _normalAudio.Play();
            }
        }
        //ゲーム中だった場合
        else
        {
            if (_timeLimit >= 0)
            {
                //制限時間を引いていく
                _timeLimit -= Time.deltaTime;
            }

            //残り時間が20秒切ったら
            if (_timeLimit <= 20)
            {
                //BGMが変更されていなかったら
                if (!_changeBGM)
                {
                    //通常時のBGMを止めて
                    _normalAudio.Stop();
                    //ラストスパートのBGMを流す
                    _lastAudio.Play();
                    //BGMを変えたフラグをオンに
                    _changeBGM = true;
                    //アイテムのリスポーン時間を伸ばす
                    _itemSpawnTime = _lastItemSpawnTime;

                    //空を夕方に
                    RenderSettings.skybox = _lastSkyBox;
                    //枠を消す
                    _mapFence.SetActive(false);
                }
            }

            //制限時間が0になったら
            if (_timeLimit <= 0)
            {
                if (_finishUI.FinishAction())
                {
                    //スコアのセットがまだだったら
                    if (!_endScoreSet)
                    {
                        //スコアをセットして
                        RankingSet();
                        //ランニングシーンに移動
                        ChangeScene.ChengeNextScene("RankingScene", 0.1f);
                        _endScoreSet = true;
                    }
                }
            }

            //ゲーム中はパネルを数え続ける
            CheckMapTilesColor();
        }
    }

    //現在のパネルの枚数を確認
    private void CheckMapTilesColor()
    {
        //それぞれのパネルの枚数をリセット
        _defaultTileNum = 0;
        _redTileNum = 0;
        _blueTileNum = 0;
        _greenTileNum = 0;
        _purpleTileNum = 0;
        //これでmapTileのレンダラーのマテリアルを確認
        foreach (MeshRenderer renderer in _mapTilesRender)
        {
            //色が変わっていなかった場合
            if (renderer.material.color == _defaultMapTileColor.color)
            {
                _defaultTileNum++;
            }
            //赤だった場合
            if (renderer.material.color == _redPlayerMat.color)
            {
                _redTileNum++;
            }
            //青だった場合
            if (renderer.material.color == _bluePlayerMat.color)
            {
                _blueTileNum++;
            }
            //緑だった場合
            if (renderer.material.color == _greenPlayerMat.color)
            {
                _greenTileNum++;
            }
            //紫だった場合
            if (renderer.material.color == _purplePlayerMat.color)
            {
                _purpleTileNum++;
            }
        }
    }

    //ランキングをセット
    public void RankingSet()
    {
        //ランキングデータの方に何人プレイかをセット
        RankingData._playerNum = _playerNum;

        //プレイヤー情報をクリア
        RankingData.playerDatas.Clear();

        //配列にMapTailを持つオブジェクトを入れる
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("MapTile");

        //何番目のプレイヤーかを確認
        int playerCount = 0;

        //マテリアルを数える
        foreach (CarController mat in _carControllers)
        {
            //マテリアルの数をプレイヤーのマテリアル事に数える
            int count = 0;

            foreach (GameObject obj in _mapTiles)
            {
                Material objMat = obj.GetComponent<MeshRenderer>().material;
                if (objMat.color == mat.MyColor.color)
                {
                    count++;
                }
            }

            //ここでセット
            RankingData.playerDatas.Add(new PlayerData(mat.Name, count, mat.MyColor, mat.ModelNum));
            playerCount++;
        }
    }

    //リストにカーコントローラーを登録
    public void SetCarController(CarController controller)
    {
        _carControllers.Add(controller);
    }

    //アイテムの全員のカメラの回転
    public void Item_CameraRotate_All(CarController controller)
    {
        foreach (CarController car in _carControllers)
        {
            //呼び出した本人じゃなかった場合
            if (car != controller)
            {
                car.HitCameraRotate();
            }
        }
    }

    //アイテムの全員スタン
    public void Item_Stun_All(CarController controller)
    {
        foreach (CarController car in _carControllers)
        {
            //呼び出した本人じゃなかった場合
            if (car != controller)
            {
                car.HitStun();
            }
        }
    }
}
