using Extension;
using Quantum;
using Quantum.App.Combat.Animation;
using UnityEngine;

namespace Animation.Attack
{
    public class AttackCommandSender : MonoBehaviour
    {
        private const string ATTACK_EVENT = "Attack";
  
        private EntityView _entityView;
        private AnimationEventHandler _eventHandler;
        private QuantumGame Game => QuantumRunner.Default.Game;
        
        private void Awake()
        {
            _entityView = gameObject.RequireComponentInParent<EntityView>();
            _eventHandler = gameObject.RequireComponent<AnimationEventHandler>();
            _eventHandler.OnAnimationEvent += OnAnimationEvent;
        }

        private void OnAnimationEvent(string eventName)
        {
            if(!eventName.Equals(ATTACK_EVENT)) return;
            var command = new AttackCommand { Sender = _entityView.EntityRef };
            Game.SendCommand(command);
        }

        private void OnDestroy()
        {
            _eventHandler.OnAnimationEvent -= OnAnimationEvent;
        }
    }
}