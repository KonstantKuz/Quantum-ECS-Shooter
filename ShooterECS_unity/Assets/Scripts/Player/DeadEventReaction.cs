using Extension;
using Quantum;
using UnityEngine;

namespace Player
{
    public class DeadEventReaction : MonoBehaviour
    {
        [SerializeField] private GameObject _regularView;
        [SerializeField] private GameObject _deadView;
        
        private EntityView _entityView;
        private DispatcherSubscription _subscription;
        
        private void Awake()
        {
            _entityView = gameObject.RequireComponent<EntityView>();
            _subscription = QuantumEvent.Subscribe<EventEntityDead>(this, OnEntityDead);
        }

        private void OnEntityDead(EventEntityDead deadEvent)
        {
            if(deadEvent.Target != _entityView.EntityRef) return;
            _regularView.SetActive(false);
            _deadView.SetActive(true);
        }

        private void OnDestroy()
        {
            QuantumEvent.Unsubscribe(_subscription);
        }
    }
}