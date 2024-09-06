using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    [SerializeField,Header("グレースケール用のマテリアル")]
    Material _grayscaleMaterial;
    [SerializeField,Header("スタン時の画像")]
    private Image _stunImage;
    [SerializeField,Header("スタン時の画像のY座標設定")]
    float _stunPosY;
    [SerializeField,Header("何秒後にスタンするか")]
    private float _stunTime;

    //StunImageを配置するCanvas
    private GameObject _canvas;
    //StunImageの座標指定でカメラの描画範囲を基準にする
    Camera _camera;
    //生成したrotateImageを保存
    Image _image;

    Material _material;

    bool _enabled = false;

    private void Start()
    {
        StunImageInstantiate();
    }

    public void GreyScale()
    {
        if (_enabled)
        {
            _material = null;
            _enabled = !_enabled;
        }
        else
        {
            StartCoroutine(StartStun(_stunTime));
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_material != null)
        {
            //マテリアルを適用
            Graphics.Blit(src, dest, _material);
        }
        else
        {
            //通常の状態
            Graphics.Blit(src, dest);
        }
    }

    IEnumerator StartStun(float rotateTime)
    {
        //スタン画像を表示
        _image.gameObject.SetActive(true);

        //rotateTime秒待つ
        yield return new WaitForSeconds(rotateTime);

        //マテリアルを適用
        _material = _grayscaleMaterial;
        _enabled = !_enabled;
        //画像非表示
        _image.gameObject.SetActive(false);
    }

    //初めにスタン時の画像を生成、座標指定
    void StunImageInstantiate()
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
        _image = Instantiate(_stunImage, _canvas.transform);
        // UI要素の位置を設定
        _image.rectTransform.anchoredPosition = localPoint + new Vector2(0f, _stunPosY * 10);
        //最初は表示しない状態から始める
        _image.gameObject.SetActive(false);
    }
}
