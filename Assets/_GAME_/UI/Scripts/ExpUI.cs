using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image expFillImage; 
    [SerializeField] private TMP_Text levelText;    

    [Header("EXP Reference")]
    [SerializeField] private Exp expScript;         

    [Header("UI Settings")]
    [SerializeField] private float fillSpeed = 3f;  

    private float targetFill = 0f;                  
    private float currentFill = 0f;                 

    private void Start()
    {
        if (expScript == null)
        {
            Debug.LogError($"{name}: Exp script reference is missing!");
            enabled = false;
            return;
        }

        // Thiết lập giá trị ban đầu
        currentFill = (float)expScript._currentExp / expScript._maxExp;
        targetFill = currentFill;
        expFillImage.fillAmount = currentFill;
        levelText.text = $"{expScript._level}";
    }

    private void Update()
    {
        // Nếu player chưa có expScript hoặc expImage thì bỏ qua
        if (expScript == null || expFillImage == null) return;

        // Tính toán lại fill target
        targetFill = (float)expScript._currentExp / expScript._maxExp;

        // Lerp giúp thanh exp chạy mượt dần
        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * fillSpeed);
        expFillImage.fillAmount = currentFill;

        // Cập nhật text level
        levelText.text = $"{expScript._level}";
    }
}

