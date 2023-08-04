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

  public void PollInput(CallbackPollInput callback)
  {
    var input = new QuantumInput();
    
    input.Jump = UnityInput.GetButton("Jump");
    
    var x = UnityInput.GetAxis("Horizontal");
    var y = UnityInput.GetAxis("Vertical");
    input.Direction = new Vector2(x, y).ToFPVector2();
    
    var mouseX = UnityInput.GetAxis("Mouse X");
    var mouseY = UnityInput.GetAxis("Mouse Y");
    input.MouseInput = new Vector2(mouseX, mouseY).ToFPVector2();

    input.Fire = UnityInput.GetButton("Fire1");
    
    callback.SetInput(input, DeterministicInputFlags.Repeatable);
  }
}
