using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {
    public static EnemyHealthBar instance { get; private set; }

    [SerializeField]
    private Image content;

    [SerializeField]
    private float lerpSpeed;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void UpdateBar(float currentAmount, float maxAmount)
    {
        content.fillAmount = Mathf.Lerp(content.fillAmount, currentAmount / maxAmount, Time.deltaTime * lerpSpeed);
    }
}
