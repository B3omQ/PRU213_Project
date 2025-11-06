using System;
using System.Collections;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float _dayLength = 300f; // tổng thời gian 1 ngày (giây)
    [SerializeField] private int _startHour = 12;     // giờ bắt đầu (12 = trưa)
    [SerializeField] private int _startMinute = 0;
    [SerializeField] private int _startDay = 1;

    private TimeSpan _currentTime;
    public TimeSpan CurrentTime => _currentTime;
    public int CurrentDay { get; private set; }

    private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

    private void Start()
    {
        // 🔹 Đặt thời gian ban đầu (buổi trưa, ngày 1)
        _currentTime = new TimeSpan(_startHour, _startMinute, 0);
        CurrentDay = _startDay;

        StartCoroutine(AddMinute());
    }

    private IEnumerator AddMinute()
    {
        yield return new WaitForSeconds(_minuteLength);

        _currentTime += TimeSpan.FromMinutes(1);

        // 🔹 Khi hết ngày
        if (_currentTime.TotalMinutes >= WorldTimeConstants.MinutesInDay)
        {
            _currentTime = TimeSpan.Zero;
            CurrentDay++;
        }

        StartCoroutine(AddMinute());
    }
}


