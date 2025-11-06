using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldTime worldTime;
    [SerializeField] private Light2D globalLight; // Light 2D (Global Light)

    [Header("Lighting Settings")]
    [SerializeField, Range(0f, 1f)] private float dayIntensity = 1f;
    [SerializeField, Range(0f, 1f)] private float nightIntensity = 0.2f;
    [SerializeField] private Gradient lightColor; // đổi màu ánh sáng (tùy chọn)

    private void Update()
    {
        if (worldTime == null || globalLight == null)
            return;

        float timeOfDay = (float)worldTime.CurrentTime.TotalMinutes / WorldTimeConstants.MinutesInDay;

        // Chuyển từ sáng -> tối -> sáng theo thời gian
        float intensity = GetLightIntensity(timeOfDay);

        globalLight.intensity = Mathf.Lerp(globalLight.intensity, intensity, Time.deltaTime * 2f);

        // Nếu bạn muốn màu sắc thay đổi (sáng vàng, tối xanh lam)
        if (lightColor != null)
            globalLight.color = lightColor.Evaluate(timeOfDay);
    }

    private float GetLightIntensity(float time)
    {
        // 0.0 = 00:00, 0.5 = 12:00, 1.0 = 24:00
        // Giảm sáng khi về đêm (giữa 18h-6h)
        if (time < 0.25f) // 00:00 - 06:00
            return Mathf.Lerp(nightIntensity, dayIntensity, time * 4f);
        else if (time < 0.75f) // 06:00 - 18:00
            return dayIntensity;
        else // 18:00 - 24:00
            return Mathf.Lerp(dayIntensity, nightIntensity, (time - 0.75f) * 4f);
    }
}
