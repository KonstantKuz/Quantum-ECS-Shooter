using UnityEngine;

namespace Photon.QuantumDemo.Game.Scripts
{
    public class MoveInputWrapper
    {
        private Joystick _joystick;
        public Vector2 InputDelta { get; private set; }

        public MoveInputWrapper(Joystick joystick)
        {
            _joystick = joystick;
        }
        
        public void UpdateInput()
        {
#if UNITY_EDITOR
            InputDelta = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#elif UNITY_ANDROID || UNITY_IOS
            InputDelta = new Vector2(_joystick.Horizontal, _joystick.Vertical);
#endif
        }
    }
}