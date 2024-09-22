using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{

    [SerializeField, Header("コントローラーのInput")]
    private TitleInput[] _titleInputs;
    [SerializeField]
    InputAction[] _startAction;

    [SerializeField]
    Image[] _gaugeImages;
    [SerializeField]
    float _speed;

    [SerializeField, Header("準備完了のテキスト")]
    private GameObject[] _redyText;
 
    [SerializeField, Header("シーン切り替えまでの時間")]
    private float _changeTime = 0;

    private int _count = 0;

    bool _sceneChange = false;

    public void Set1TiteInput(TitleInput input)
    {
        _titleInputs[0] = input;
    }

    public void Set2TiteInput(TitleInput input)
    {
        _titleInputs[1] = input;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _gaugeImages.Length; i++)
        {
            _gaugeImages[i].fillAmount = 0;
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < _startAction.Length; i++)
        {
            _startAction[i].Enable();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _startAction.Length; i++)
        {
            _startAction[i].Dispose();
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _startAction.Length; i++)
        {
            _startAction[i].Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_sceneChange) return;

        _count = 0;

        for (int i = 0; i < _titleInputs.Length; i++)
        {
            if (_titleInputs[i].DownStartButton)
            {
                _gaugeImages[i].fillAmount += _speed;
                _gaugeImages[i].fillAmount = Mathf.Clamp(_gaugeImages[i].fillAmount, 0, 0.5f);
                if (_gaugeImages[i].fillAmount >= 0.5f)
                {
                    _count++;
                    _redyText[i].SetActive(true);
                    if (_count == _titleInputs.Length)
                    {
                        StartCoroutine(LateSceneChange());
                        _sceneChange = true;
                    }
                }
            }
            else
            {
                _gaugeImages[i].fillAmount -= _speed;
                _redyText[i].SetActive(false);
            }
        }
    }

    IEnumerator LateSceneChange()
    {
        yield return new WaitForSeconds(_changeTime);
        transform.parent.GetComponent<ChangeScene>().Button_ChangeNext();
    }

    public bool GetSceneChange()
    {
        return _sceneChange;
    }
}
