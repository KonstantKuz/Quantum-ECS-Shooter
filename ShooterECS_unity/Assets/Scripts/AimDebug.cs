using Quantum;
using UnityEngine;

public class AimDebug : MonoBehaviour
{
    private EntityView _entityView;
    private GameObject _sphere;
    private void Awake()
    {
        _entityView = gameObject.GetComponent<EntityView>();
        _sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _sphere.transform.localScale = Vector3.one * 0.3f;
    }

    private void Update()
    {
        if(!_entityView.EntityRef.IsValid) return;
            
        var frame = QuantumRunner.Default.Game.Frames.Verified;
        var aim = frame.Get<Aim>(_entityView.EntityRef);
        if(!aim.HasHit) return;
        _sphere.transform.position = aim.CurrentHit.Point.ToUnityVector3();
    }
}