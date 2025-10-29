using UnityEngine;
[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    [SerializeField] private Animator _animator;


    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";


    [Header("Aim Settings")]
    public Transform _Aim;
    [SerializeField] private float _aimOffset = 0.6f;
    bool _isMoving = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        _rb.linearVelocity = _movement * _moveSpeed;

        _animator.SetFloat(_vertical, _movement.y);
        _animator.SetFloat(_horizontal, _movement.x);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        UpdateAimPosition();
    }

    private void UpdateAimPosition()
    {
        if (_movement != Vector2.zero)
        {
            // Xoay Aim theo hướng di chuyển
            _Aim.rotation = Quaternion.LookRotation(Vector3.forward, _movement);

            // Di chuyển Aim ra trước Player trong local space
            Vector3 aimLocalOffset = new Vector3(_movement.x, _movement.y, 0).normalized * _aimOffset;
            _Aim.localPosition = aimLocalOffset;
        }
    }
}
