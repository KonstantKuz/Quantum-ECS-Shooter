using Extension;
using Quantum;
using UnityEngine;

namespace Animation
{
    [RequireComponent(typeof(Animator))]
    public class EntityCharacterAnimator : MonoBehaviour
    {
        private static int VERTICAL_HASH = Animator.StringToHash("Vertical");
        private static int HORIZONTAL_HASH = Animator.StringToHash("Horizontal");
        private static int GROUNDED_HASH = Animator.StringToHash("IsGrounded");
        private const int SMOOTH_STEP = 10;
        
        private Animator _animator;
        private EntityView _entityView;
        private Vector3 _smoothDirection;

        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _entityView = gameObject.RequireComponentInParent<EntityView>();
        }

        private void LateUpdate()
        {
            if(!_entityView.EntityRef.IsValid) return;
            
            var game = QuantumRunner.Default.Game;
            var controller = game.Frames.Verified.Get<CharacterController3D>(_entityView.EntityRef);
            var characterVelocity = Vector3.ProjectOnPlane(controller.Velocity.ToUnityVector3(), Vector3.up);
            var localDirection = _animator.transform.InverseTransformDirection(characterVelocity);
            _smoothDirection = Vector3.Lerp(_smoothDirection, localDirection, Time.deltaTime * SMOOTH_STEP);
            _animator.SetFloat(VERTICAL_HASH, _smoothDirection.z);
            _animator.SetFloat(HORIZONTAL_HASH, _smoothDirection.x);
            _animator.SetBool(GROUNDED_HASH, controller.Grounded);
        }
    }
}
