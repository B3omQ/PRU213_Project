using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public GameObject _heartPrefab;
    public PlayerHealth _playerHealth;
    List<Heart> _hearts = new List<Heart>();


    private void Awake()
    {
        if (_playerHealth == null)
            Debug.LogError($"{name}: PlayerHealth reference is missing!");
    }
    private void OnEnable()
    {
        PlayerHealth._OnplayerDamaged += DrawHearts;
        PlayerHealth._OnplayerDeath += DrawHearts;
        PlayerHealth._OnplayerHealed += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerHealth._OnplayerDamaged -= DrawHearts;
        PlayerHealth._OnplayerDeath -= DrawHearts;
        PlayerHealth._OnplayerHealed -= DrawHearts;
    }
    private void Start()
    {
        CreateHearts();
        UpdateHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        // 🧮 Mỗi tim = 4 máu
        int heartsToMake = Mathf.CeilToInt(_playerHealth._maxHealth / 4f);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < _hearts.Count; i++)
        {
            // Tính số máu còn lại trong mỗi tim (mỗi tim = 4 máu)
            int heartStatusRemainder = (int)Mathf.Clamp(_playerHealth._health - (i * 4), 0, 4);
            _hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(_heartPrefab, transform);

        // 🔧 Reset vị trí và tỉ lệ UI để không bị phóng to / lệch
        RectTransform rect = newHeart.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        rect.anchoredPosition3D = Vector3.zero;
        rect.localRotation = Quaternion.identity;

        Heart heartComponent = newHeart.GetComponent<Heart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        _hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        _hearts = new List<Heart>();
    }

    private void CreateHearts()
    {

        foreach (Transform child in transform)
            Destroy(child.gameObject);
        _hearts.Clear();

        int heartsToMake = Mathf.CeilToInt(_playerHealth._maxHealth / 4f);

        for (int i = 0; i < heartsToMake; i++)
        {
            GameObject newHeart = Instantiate(_heartPrefab, transform);
            newHeart.transform.SetParent(transform, false); // giữ layout chuẩn
            RectTransform rect = newHeart.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;

            Heart heartComponent = newHeart.GetComponent<Heart>();
            if (heartComponent == null)
            {
                Debug.LogError($"{_heartPrefab.name} prefab is missing Heart component!");
                continue;
            }

            _hearts.Add(heartComponent);
        }
    }

    private void UpdateHearts()
    {
        if (_playerHealth == null || _hearts.Count == 0)
            return;

        for (int i = 0; i < _hearts.Count; i++)
        {
            // Tính số máu còn lại trong mỗi tim (mỗi tim = 4 máu)
            int heartValue = Mathf.Clamp((int)(_playerHealth._health - (i * 4)), 0, 4);
            _hearts[i].SetHeartImage((HeartStatus)heartValue);
        }
    }
}
