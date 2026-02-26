using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelItem : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text unitCountText;
    [SerializeField] private Image unitImage;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;

    private int unitCount;
    private Func<int> getCount;
    private Action onPlus;
    private Action onMinus;

    public void Initialize(
        string name,
        Sprite sprite,
        Func<int> getCount,
        Action plusAction,
        Action minusAction)
    {
        unitNameText.text = name;

        this.getCount = getCount;
        onPlus = plusAction;
        onMinus = minusAction;
        Debug.Log(sprite);
        unitImage.sprite = sprite;



        plusButton.onClick.AddListener(() =>
        {
            onPlus?.Invoke();
            UpdateText();
        });

        minusButton.onClick.AddListener(() =>
        {
            onMinus?.Invoke();
            UpdateText();
        });

        UpdateText();
    }

    private void UpdateText()
    {
        unitCount = getCount?.Invoke() ?? 0;
        unitCountText.text = unitCount.ToString();
    }

    public int GetUnitCount() => unitCount;
}





