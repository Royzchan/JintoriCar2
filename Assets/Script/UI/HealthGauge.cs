using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGauge : MonoBehaviour
{
    [SerializeField] private Image burnImage;
    [SerializeField] private Image healthImage;

    private CarController _carController;

    RectTransform rectTransform;

    public float _maxHealth = 100f;
    public float delaySpeed = 0.3f;

    private void Start()
    {
        UpdateHealthBars(_maxHealth);
        rectTransform = GetComponent<RectTransform>();
    }

    //�Ή�����Ԃ��Z�b�g
    public void SetCar(CarController car)
    {
        _carController = car;
        _maxHealth = _carController.DefMaxInk;
    }

    private void Update()
    {
        //�������Ŗ��t���[��HP�X�V
        SetHealth(_carController.Ink);
        //�T�C�Y�X�V
        SetInkSize(_carController.MaxInk);
    }

    // �O������HP�̒l��ݒ肷�郁�\�b�h
    public void SetHealth(float health)
    {
        float clampedHealth = Mathf.Clamp(health, 0, _maxHealth);
        UpdateHealthBars(clampedHealth);
    }

    //�C���N�̃T�C�Y���X�V
    public void SetInkSize(float maxInk)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxInk);
        _maxHealth = maxInk;
        burnImage.fillAmount = _carController.Ink / _maxHealth;
    }

    // HP�o�[���X�V���郁�\�b�h
    private void UpdateHealthBars(float currentHealth)
    {
        // ����HP�o�[�̍X�V
        healthImage.fillAmount = currentHealth / _maxHealth;

        // �x��ĒǏ]����HP�o�[�̍X�V
        StopAllCoroutines(); // �O��̃R���[�`�����~
        StartCoroutine(UpdateDelayedHealthBar(currentHealth / _maxHealth));
    }

    // �x��ĕω��𔽉f����HP�o�[���X�V����R���[�`��
    private System.Collections.IEnumerator UpdateDelayedHealthBar(float targetFillAmount)
    {
        while (Mathf.Abs(burnImage.fillAmount - targetFillAmount) > 0.01f)
        {
            burnImage.fillAmount = Mathf.MoveTowards(burnImage.fillAmount, targetFillAmount, delaySpeed * Time.deltaTime);
            yield return null;
        }
        burnImage.fillAmount = targetFillAmount; // �ŏI�I�ɖڕW�l�ɍ��킹��
    }
}