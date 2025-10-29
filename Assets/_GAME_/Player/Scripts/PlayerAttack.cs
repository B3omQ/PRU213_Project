using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject _melee;
    bool _isAttack = false;
    float _atkDuration = 0.7f;
    float _atkTimer = 0f;

    [SerializeField] Animator _animator;
    private const string _rightClick = "RightClick";
    private const string _lefttClick = "LeftClick";

    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();

        if (InputManager.AttackPressed)
        {
            Debug.Log("Attack Press");
            OnAttack();
        }
    }

    void OnAttack() 
    {
        if (!_isAttack)
        {
            _melee.SetActive(true);
            _isAttack = true;
            //animation 
            _animator.SetBool(_lefttClick, true);
        }
    }

    void CheckMeleeTimer()
    {
        if (_isAttack)
        {
            _atkTimer += Time.deltaTime;
            if(_atkTimer >= _atkDuration)
            {
                _atkTimer = 0;
                _isAttack = false;
                _melee.SetActive(false);
                _animator.SetBool(_lefttClick, false);
            }
        }
    }
}
