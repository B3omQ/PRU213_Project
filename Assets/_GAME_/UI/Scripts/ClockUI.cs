using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldTime worldTime;
    [SerializeField] private TMP_Text clockText;

    private void Update()
    {
        if (worldTime == null || clockText == null) return;

        System.TimeSpan time = worldTime.CurrentTime;

        // Format HH:mm (giờ:phút, có 0 ở đầu)
        string timeString = $"{time.Hours:00}:{time.Minutes:00}";
        string dayString = $"DAY: {worldTime.CurrentDay}";

        clockText.text = $"{timeString} {dayString}";
    }
}

