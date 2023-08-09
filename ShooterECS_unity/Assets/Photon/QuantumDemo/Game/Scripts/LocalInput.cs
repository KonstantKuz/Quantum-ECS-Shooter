using System;
using Photon.Deterministic;
using Photon.QuantumDemo.Game.Scripts;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;
using QuantumInput = Quantum.Input;

public class LocalInput : MonoBehaviour
{
  private void OnEnable() {
    QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
  }

  public float sensitivity = 15;
  public float amplitude = 0.1f;
  public float period = 1f;

  private Vector2 _mouseInput;
  private Vector2 _directionInput;
  private LookInputWrapper _inputWrapper = new LookInputWrapper();
  private MoveInputWrapper _moveInputWrapper;

  private void Awake()
  {
    _moveInputWrapper = new MoveInputWrapper(FindObjectOfType<Joystick>());
  }

  private void Update()
  {
    _inputWrapper.UpdateInput();
    _moveInputWrapper.UpdateInput();
    // _mouseInput += new Vector2(UnityInput.GetAxisRaw("Mouse X"), UnityInput.GetAxisRaw("Mouse Y"));
    _mouseInput += _inputWrapper.InputDelta * sensitivity;
    _directionInput += _moveInputWrapper.InputDelta;
  }

  public void PollInput(CallbackPollInput callback)
  {
    var input = new QuantumInput();
    
    input.Jump = Input.GetButton("Jump");
    
    // var x = UnityInput.GetAxisRaw("Horizontal");
    // var y = UnityInput.GetAxisRaw("Vertical");
    input.Direction = _directionInput.ToFPVector2();
    _directionInput = Vector2.zero;
    
    // var mouseX = UnityInput.GetAxisRaw("Mouse X");
    // var mouseY = UnityInput.GetAxisRaw("Mouse Y");
    var sin = amplitude * Mathf.Sin(Time.time * period);
    input.MouseInput = _mouseInput.ToFPVector2();
    _mouseInput = Vector2.zero;
    
    input.Fire = Input.GetButton("Fire1");
    
    callback.SetInput(input, DeterministicInputFlags.Repeatable);
  }
}