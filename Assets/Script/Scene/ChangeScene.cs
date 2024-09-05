using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    //�ǂݍ��ރV�[����
    [SerializeField, Header("���Ɉړ�������V�[����")]
    public string ChangeSceneName;

    [SerializeField, Header("�V�[���ؑ֕b��")]
    private float ChangeTime = 1.0f;

    private void Update()
    {

    }

    //Debug�p
    public void DebugChange()
    {
        FadeManager.Instance.LoadScene(ChangeSceneName, ChangeTime);
    }

    public void Button_ChangeNext()
    {
        //�ړ�����V�[�����A�ړ�����
        FadeManager.Instance.LoadScene(ChangeSceneName, ChangeTime);
    }

    //�ʃV�[���ŌĂяo���p
    static public void ChengeNextScene(string name, float time)
    {
        FadeManager.Instance.LoadScene(name, time);
    }

}
