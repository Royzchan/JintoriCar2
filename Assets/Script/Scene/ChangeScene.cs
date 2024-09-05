using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    //読み込むシーン名
    [SerializeField, Header("次に移動させるシーン名")]
    public string ChangeSceneName;

    [SerializeField, Header("シーン切替秒数")]
    private float ChangeTime = 1.0f;

    private void Update()
    {

    }

    //Debug用
    public void DebugChange()
    {
        FadeManager.Instance.LoadScene(ChangeSceneName, ChangeTime);
    }

    public void Button_ChangeNext()
    {
        //移動するシーン名、移動時間
        FadeManager.Instance.LoadScene(ChangeSceneName, ChangeTime);
    }

    //別シーンで呼び出す用
    static public void ChengeNextScene(string name, float time)
    {
        FadeManager.Instance.LoadScene(name, time);
    }

}
