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

  private Vector2 _lookInputAccumulated;
  private Vector2 _moveInputAccumulated;
  private LookInputWrapper _lookInputWrapper = new LookInputWrapper();
  private MoveInputWrapper _moveInputWrapper;

  private void Awake()
  {
    _moveInputWrapper = new MoveInputWrapper(FindObjectOfType<Joystick>());
  }

  private void Update()
  {
    _lookInputWrapper.UpdateInput();
    _moveInputWrapper.UpdateInput();
    // _mouseInput += new Vector2(UnityInput.GetAxisRaw("Mouse X"), UnityInput.GetAxisRaw("Mouse Y"));
    _lookInputAccumulated += _lookInputWrapper.InputDelta * sensitivity;
    _moveInputAccumulated += _moveInputWrapper.InputDelta;
  }

  public void PollInput(CallbackPollInput callback)
  {
    var input = new QuantumInput();
    
    input.Jump = Input.GetButton("Jump");
    
    // var x = UnityInput.GetAxisRaw("Horizontal");
    // var y = UnityInput.GetAxisRaw("Vertical");
    input.MoveInput = _moveInputAccumulated.ToFPVector2();
    _moveInputAccumulated = Vector2.zero;
    
    // var mouseX = UnityInput.GetAxisRaw("Mouse X");
    // var mouseY = UnityInput.GetAxisRaw("Mouse Y");
    var sin = amplitude * Mathf.Sin(Time.time * period);
    input.LookInput = _lookInputAccumulated.ToFPVector2();
    _lookInputAccumulated = Vector2.zero;
    
    input.Fire = Input.GetButton("Fire1");
    
    callback.SetInput(input, DeterministicInputFlags.Repeatable);
  }
}