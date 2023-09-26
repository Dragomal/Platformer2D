using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovements : MonoBehaviour
{
    [SerializeField, Range(0,10)] private float _movementSpeed;
    [SerializeField, Range(0,50)] private float _jumpPower;
    [SerializeField, Range(0,1)] private float _airControl;
    [SerializeField, Range(0,4)] private float _gravityScaleJump;
    [SerializeField, Range(0,4)] private float _gravityScaleRelease;
    [SerializeField, Range(0,4)] private float _gravityScaleNormal;
    
    private bool _isGrounded;
    private bool _smashAction;
    private Vector2 _moveAction;
    private Rigidbody2D _rigidbody2D;
    private BoxGroundSensor _boxGroundSensor;
    private InputAction _onMoveAction;
    private InputAction _onSmashAction;
    private void Start(){
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxGroundSensor = GetComponent<BoxGroundSensor>();

        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _onMoveAction = _playerInput.actions.FindAction("Move");
        _onSmashAction = _playerInput.actions.FindAction("Smash");
    }
    private void FixedUpdate()
    {
        ReadValues();
        UpdateHorizontalMovements();
        UpdateGravityScale();
        
    }

    private void UpdateGravityScale()
    {
        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.gravityScale = _gravityScaleNormal;
        }
        if (_rigidbody2D.velocity.y > 0 && !_smashAction)
        {
            _rigidbody2D.gravityScale = _gravityScaleRelease;
        }
    }

    private void UpdateHorizontalMovements()
    {
        if (_boxGroundSensor.IsGrounded())
        {
            _rigidbody2D.velocity = new Vector2(_moveAction.x * _movementSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            float airMoveAction = Mathf.Lerp(_rigidbody2D.velocity.x, _moveAction.x * _movementSpeed, _airControl);
            _rigidbody2D.velocity = new Vector2(airMoveAction, _rigidbody2D.velocity.y);
        }
    }

    private void ReadValues()
    {
        _smashAction = _onSmashAction.ReadValue<float>() > 0.5f ? true : false;
        _moveAction = _onMoveAction.ReadValue<Vector2>();
    }

    private void OnSmash(InputValue value){
        if(!_boxGroundSensor.IsGrounded()) return;

        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
    }
}
