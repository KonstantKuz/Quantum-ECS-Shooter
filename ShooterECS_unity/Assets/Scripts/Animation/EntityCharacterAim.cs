using Extension;
using Quantum;
using UnityEngine;

namespace Animation
{
    public class EntityCharacterAim : MonoBehaviour
    {
        private const int SMOOTH_STEP = 100;
        [SerializeField] private Transform _pointer;
        
        private EntityView _entityView;

        private void Awake()
        {
            _entityView = gameObject.RequireComponentInParent<EntityView>();
        }

        private void LateUpdate()
        {
            if(!_entityView.EntityRef.IsValid) return;
            
            var game = QuantumRunner.Default.Game;
            var aim = game.Frames.Verified.Get<Aim>(_entityView.EntityRef);
            _pointer.position = Vector3.Lerp(_pointer.position, aim.CurrentAim.ToUnityVector3(), Time.deltaTime * SMOOTH_STEP);
        }
    }
}
