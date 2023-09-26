using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField, Range(0,10)] private float _movementSpeed;
    private Rigidbody2D _rigidbody2D;
    private InputAction _OnMoveAction;
    private InputAction _OnSmashAction;
    void Start(){
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _OnMoveAction = _playerInput.actions.FindAction("Move");
        _OnSmashAction = _playerInput.actions.FindAction("Smash");
    }
    private void OnMove(InputValue value){
        Debug.Log("Move" + value.Get<Vector2>());
    }
    private void OnSmash(InputValue value){
        Debug.Log("Smash" + value.Get<float>());
    }
    void FixedUpdate(){
        _rigidbody2D.velocity = new Vector2(_OnMoveAction.ReadValue<Vector2>().x * _movementSpeed, _rigidbody2D.velocity.y);
    }
}
