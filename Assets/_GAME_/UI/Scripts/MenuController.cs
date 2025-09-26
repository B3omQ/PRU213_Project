using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject _Menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.OpenMenuPressed)
        {
            _Menu.SetActive(!_Menu.activeSelf);
        }
    }
}
