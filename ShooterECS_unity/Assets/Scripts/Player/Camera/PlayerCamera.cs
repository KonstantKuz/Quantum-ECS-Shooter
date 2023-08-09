using Extension;
using UnityEngine;

namespace Player.Camera
{
    public class PlayerCamera : MonoBehaviour
    {
        private void Start()
        {
            var entityView = gameObject.RequireComponent<EntityView>();
            var game = QuantumRunner.Default.Game;
            var playerRef = game.Frames.Verified.Get<Quantum.PlayerCamera>(entityView.EntityRef).PlayerRef;
            gameObject.SetActive(game.PlayerIsLocal(playerRef));
        }
    }
}
