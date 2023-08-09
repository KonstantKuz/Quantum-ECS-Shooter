using Extension;
using Quantum;
using UnityEngine;

namespace Animation.Attack
{
    public class AttackEventReceiver : MonoBehaviour
    {
        private const string ATTACK = "Attack";

        private EntityView _entityView;
        private Animator _animator;
        private DispatcherSubscription _subscription;

        private void Awake()
        {
            _entityView = gameObject.RequireComponentInParent<EntityView>();
            _animator = gameObject.RequireComponent<Animator>();
            _subscription = QuantumEvent.Subscribe<EventEntityAttack>(this, OnEntityAttack);
        }

        private void OnEntityAttack(EventEntityAttack attackEvent)
        {
            if(attackEvent.Sender != _entityView.EntityRef) return;
            _animator.Play(ATTACK);
        }

        private void OnDestroy()
        {
            QuantumEvent.Unsubscribe(_subscription);
        }
    }
}