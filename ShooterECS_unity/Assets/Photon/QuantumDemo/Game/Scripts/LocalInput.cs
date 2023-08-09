using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using QuantumInput = Quantum.Input;

public class LocalInput : MonoBehaviour
{
  private void OnEnable() {
    QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
  }

  public float amplitude = 0.1f;
  public float period = 1f;

  private Vector2 _mouseInput;
  private Vector2 _directionInput;
  
  private void Update()
  {
    _mouseInput += new Vector2(UnityInput.GetAxisRaw("Mouse X"), UnityInput.GetAxisRaw("Mouse Y"));
    _directionInput += new Vector2(UnityInput.GetAxisRaw("Horizontal"), UnityInput.GetAxisRaw("Vertical"));
  }

  public void PollInput(CallbackPollInput callback)
  {
    var input = new QuantumInput();
    
    input.Jump = UnityInput.GetButton("Jump");
    
    // var x = UnityInput.GetAxisRaw("Horizontal");
    // var y = UnityInput.GetAxisRaw("Vertical");
    input.Direction = _directionInput.ToFPVector2();
    _directionInput = Vector2.zero;
    
    // var mouseX = UnityInput.GetAxisRaw("Mouse X");
    // var mouseY = UnityInput.GetAxisRaw("Mouse Y");
    var sin = amplitude * Mathf.Sin(Time.time * period);
    input.MouseInput = _mouseInput.ToFPVector2();
    _mouseInput = Vector2.zero;
    
    input.Fire = UnityInput.GetButton("Fire1");
    
    callback.SetInput(input, DeterministicInputFlags.Repeatable);
  }
}
