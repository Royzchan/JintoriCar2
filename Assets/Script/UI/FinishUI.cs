using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishUI : MonoBehaviour
{
    [SerializeField]
    float _speed = 1;
    [SerializeField,Header("•Ï‰»Œã‚Ì‘Ò‚¿ŽžŠÔ")]
    float _aftertasteTime;

    GameManager _gameManager;

    Image _image;

    float _value = 0;

    float _afterValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _image = GetComponent<Image>();
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _value);
        _image.rectTransform.localScale = new Vector2(_value, _value);
    }

    public bool FinishAction()
    {
        _value += Time.deltaTime * _speed;
        _value = Mathf.Clamp01(_value);
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _value);
        _image.rectTransform.localScale = new Vector2(_value, _value);
        if (_value >= 1)
        {
            _afterValue += Time.deltaTime;
        }
        if (_afterValue < _aftertasteTime)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
