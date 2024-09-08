using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("元の距離に戻るスピード")]
    private float _returnSpeed = 1f;
    [SerializeField, Header("初期位置に徐々に戻るスピード")]
    private float _initPosReturnSpeed = 0.05f;
    [SerializeField,Header("カメラが少し遅れてくる速度")]
    private float _smoothSpeed = 0.125f;
    [SerializeField,Header("遅れる大きさ")]
    private float _lateValue = 0.3f;
    [SerializeField,Header("上下反転時に表示する画像")]
    private Image _rotateImage;
    [SerializeField,Header("何秒後に反転するか")]
    private float _rotateTime;
    [SerializeField,Header("上下反転時の画像のY座標設定")]
    float _rotatePosY;

    //RotateImageを配置するCanvas
    private GameObject _canvas;
    //RotateImageの座標指定でカメラの描画範囲を基準にする
    Camera _camera;
    //生成したrotateImageを保存
    Image _image;

    //上下反転するか
    bool _isFlipped = false;

    //初期値
    private Vector3 _initLocalPos;
    Quaternion _rot;
    private float _distance;
    string _distanceFormatted;

    private Vector3 _offset;// オフセット値

    //カメラの位置が変わるときに基準となる位置を保存
    private Vector3 _localPos;

    //Rayと壁が当たったポジション
    private Vector3 _wallHitPosition;

    //画面揺れ中か判定
    private bool _isCameraShake = false;

    //左右移動中か
    bool _isLate = false;

    //リターン中か
    bool _isRetrun = false;

    // Start is called before the first frame update
    void Start()
    {
        //初期値
        _initLocalPos = transform.localPosition;
        _rot = transform.localRotation;
        //プレイヤーとカメラの距離を計算
        _distance = Vector3.Distance(transform.parent.position, transform.position);
        //小数点第2位四捨五入
        _distanceFormatted = _distance.ToString("F1");
        _localPos = transform.localPosition;

        RotateImageInstantiate();
    }

    // Update is called once per frame
    void Update()
    {
        //壁があったら
        if (WallCheck())
        {
            //Rayが当たったポジションにカメラを移動
            transform.position = _wallHitPosition;
        }
        else
        {
            ReturnDis();
        }

        LateUpdate();
    }

    //プレイヤーで呼ぶように
    public void Shake()
    {
        StartCoroutine(CameraShake(0.25f, 0.1f));
    }

    //カメラ操作（プレイヤーの周りを回転）
    //プラスマイナスで方向が変わる
    public void CameraOperation(float rotateSpeed)
    {
        transform.RotateAround(transform.parent.position, Vector3.up, rotateSpeed);
        _localPos = transform.localPosition;
    }

    //カメラ回転（180度）
    //もう一度呼ぶともとに戻る
    public void CameraRotate()
    {
        if (_isFlipped)
        {
            _isFlipped = !_isFlipped;
        }
        else
        {
            StartCoroutine(StartRotate(_rotateTime));
        }
    }

    IEnumerator StartRotate(float rotateTime)
    {
        //上下反転のImageを表示
        _image.gameObject.SetActive(true);

        //rotateTime秒待つ
        yield return new WaitForSeconds(rotateTime);

        //上下反転
        _isFlipped = !_isFlipped;
        //Imageを非表示に
        _image.gameObject.SetActive(false);
    }

    //カメラ揺れ（デフォルト値 : 0.25f, 0.1f）
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        //画面揺れ中か判定
        if (_isCameraShake) yield break;

        var _beforePos = transform.localPosition;

        var _beforeRot = transform.localRotation;

        var _elapsed = 0f;

        _isCameraShake = true;

        while (_elapsed < duration)
        {
            var x = _beforePos.x + Random.Range(-1f, 1f) * magnitude;
            var y = _beforePos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _beforePos.z);
            _elapsed += Time.deltaTime;

            yield return null;
        }

        //画面揺れ前の位置に戻す
        transform.localPosition = _beforePos;
        transform.localRotation = _beforeRot;
        _isCameraShake = false;
    }

    //カメラ位置リセット
    public void Reset()
    {
        transform.localPosition = _initLocalPos;
        transform.localRotation = _rot;
        _localPos = transform.localPosition;
    }

    //壁に埋まらないようにチェック
    bool WallCheck()
    {
        RaycastHit _wallHit;

        //Rayを飛ばしてプレイヤーとカメラの間にオブジェクトがあるかどうか
        if (Physics.Raycast(transform.parent.position, transform.position - transform.parent.position, out _wallHit, Vector3.Distance(transform.parent.position, transform.position)))
        {
            //Rayが当たったオブジェクトが壁だったら
            if (_wallHit.collider.CompareTag("Wall"))
            {
                //当たったポジションを保存
                _wallHitPosition = _wallHit.point;
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    //距離が初期値と違うときに調整
    void ReturnDis()
    {
        //プレイヤーとカメラの距離を計算
        float _dis = Vector3.Distance(transform.parent.position, transform.position);
        //小数点第2位四捨五入
        string _disFormatted = _dis.ToString("F1");
        Vector3 _dir = transform.parent.position - transform.position;
        _dir = Vector3.Normalize(_dir);

        //最初の距離と違ったら戻す
        if (float.Parse(_distanceFormatted) > float.Parse(_disFormatted))
        {
            transform.position -= _dir * Time.deltaTime * _returnSpeed;
            _localPos = transform.localPosition;
            _isRetrun = true;
        }
        else if (float.Parse(_distanceFormatted) < float.Parse(_disFormatted))
        {
            transform.position += _dir * Time.deltaTime * _returnSpeed;
            _localPos = transform.localPosition;
            _isRetrun = true;
        }
        else
        {
            _isRetrun = false;
        }
    }

    //初期位置に徐々に戻る
    //前進中かを引数に入れる
    //Updateに書く
    public void InitPosReturn(bool isFront)
    {
        if (!isFront) return;
        if (_isLate) return;

        //小数点第2位四捨五入
        string _localPosFormatted = _initLocalPos.x.ToString("F1");
        string _transformLocalPositionFormatted = transform.localPosition.x.ToString("F1");

        //floatに直して比較
        if (float.Parse(_transformLocalPositionFormatted) > float.Parse(_localPosFormatted))
        {
            CameraOperation(_initPosReturnSpeed);
        }
        else if (float.Parse(_transformLocalPositionFormatted) < float.Parse(_localPosFormatted))
        {
            CameraOperation(-_initPosReturnSpeed);
        }
    }

    void LateUpdate()
    {
        if (_isCameraShake) return;
        if (_isRetrun) return;
        Vector3 _desiredPosition = _localPos + _offset;

        // カメラの位置をスムーズに補間
        Vector3 _smoothedPosition = Vector3.Lerp(transform.localPosition, _desiredPosition, _smoothSpeed);

        // カメラの位置を更新
        transform.localPosition = _smoothedPosition;

        // ターゲットの方向を向くための方向ベクトルを計算
        Vector3 direction = transform.parent.position - transform.position;

        // 回転を計算
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 上下反転させるか
        if (_isFlipped)
        {
            // X軸を180度回転して上下反転
            rotation *= Quaternion.Euler(0f, 0f, 180f);
        }

        // 計算した回転を適用
        transform.rotation = rotation;
    }

    public void Smooth(float value)
    {
        if(value == 0)
        {
            _isLate = false;
            _offset.x = 0;
            _offset.z = 0;
        }
        else if(value > 0)
        {
            _isLate = true;
            // 角度をラジアンに変換
            float _radians = transform.localEulerAngles.y * Mathf.Deg2Rad;
            _offset.x = Mathf.Cos(_radians) * _lateValue;
            _offset.z = -Mathf.Sin(_radians) * _lateValue;
        }
        else if (value < 0)
        {
            _isLate = true;
            // 角度をラジアンに変換
            float _radians = transform.localEulerAngles.y * Mathf.Deg2Rad;
            _offset.x = -Mathf.Cos(_radians) * _lateValue;
            _offset.z = Mathf.Sin(_radians) * _lateValue;
        }
    }

    //初めに上下反転時の画像を生成、座標指定
    void RotateImageInstantiate()
    {
        //親となるCanvasを設定
        _canvas = GameObject.Find("2PVerCanvas");
        //カメラを設定
        _camera = GetComponent<Camera>();

        // カメラの中心をワールド座標で取得
        Vector3 cameraCenterWorld = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _camera.nearClipPlane));

        // ワールド座標をスクリーン座標に変換
        Vector2 screenPoint = _camera.WorldToScreenPoint(cameraCenterWorld);

        // スクリーン座標をUIキャンバスのローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPoint, _canvas.GetComponent<Canvas>().worldCamera, out Vector2 localPoint);
        //生成
        _image = Instantiate(_rotateImage, _canvas.transform);
        // UI要素の位置を設定
        _image.rectTransform.anchoredPosition = localPoint + new Vector2(0f, _rotatePosY * 10);
        //最初は表示しない状態から始める
        _image.gameObject.SetActive(false);
    }
}
