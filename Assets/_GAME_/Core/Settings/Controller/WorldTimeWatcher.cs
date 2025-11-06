using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TimedEvent
{
    public string name;              // tên event (để tiện nhìn)
    public int hour;                 // giờ
    public int minute;               // phút
    public UnityEvent onTriggered;   // hành động xảy ra
    [HideInInspector] public bool hasTriggered; // để tránh chạy lại trong cùng ngày
}

public class WorldTimeWatcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldTime worldTime;

    [Header("Timed Events")]
    [SerializeField] private List<TimedEvent> timedEvents = new List<TimedEvent>();

    private int lastDayChecked = -1;

    private void Update()
    {
        if (worldTime == null) return;

        TimeSpan currentTime = worldTime.CurrentTime;
        int currentDay = worldTime.CurrentDay;

        // reset event flags mỗi ngày mới
        if (currentDay != lastDayChecked)
        {
            foreach (var e in timedEvents)
                e.hasTriggered = false;
            lastDayChecked = currentDay;
        }

        // kiểm tra các event
        foreach (var e in timedEvents)
        {
            if (!e.hasTriggered)
            {
                TimeSpan eventTime = new TimeSpan(e.hour, e.minute, 0);

                // Nếu đã qua thời gian chỉ định
                if (currentTime >= eventTime)
                {
                    e.onTriggered?.Invoke();
                    e.hasTriggered = true;
                    Debug.Log($"[WorldTimeWatcher] Event '{e.name}' triggered at {eventTime}");
                }
            }
        }
    }
}

