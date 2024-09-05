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

    //対応する車をセット
    public void SetCar(CarController car)
    {
        _carController = car;
        _maxHealth = _carController.DefMaxInk;
    }

    private void Update()
    {
        //こっちで毎フレームHP更新
        SetHealth(_carController.Ink);
        //サイズ更新
        SetInkSize(_carController.MaxInk);
    }

    // 外部からHPの値を設定するメソッド
    public void SetHealth(float health)
    {
        float clampedHealth = Mathf.Clamp(health, 0, _maxHealth);
        UpdateHealthBars(clampedHealth);
    }

    //インクのサイズを更新
    public void SetInkSize(float maxInk)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxInk);
        _maxHealth = maxInk;
        burnImage.fillAmount = _carController.Ink / _maxHealth;
    }

    // HPバーを更新するメソッド
    private void UpdateHealthBars(float currentHealth)
    {
        // 即時HPバーの更新
        healthImage.fillAmount = currentHealth / _maxHealth;

        // 遅れて追従するHPバーの更新
        StopAllCoroutines(); // 前回のコルーチンを停止
        StartCoroutine(UpdateDelayedHealthBar(currentHealth / _maxHealth));
    }

    // 遅れて変化を反映するHPバーを更新するコルーチン
    private System.Collections.IEnumerator UpdateDelayedHealthBar(float targetFillAmount)
    {
        while (Mathf.Abs(burnImage.fillAmount - targetFillAmount) > 0.01f)
        {
            burnImage.fillAmount = Mathf.MoveTowards(burnImage.fillAmount, targetFillAmount, delaySpeed * Time.deltaTime);
            yield return null;
        }
        burnImage.fillAmount = targetFillAmount; // 最終的に目標値に合わせる
    }
}