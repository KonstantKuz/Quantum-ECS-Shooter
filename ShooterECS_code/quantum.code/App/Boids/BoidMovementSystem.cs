using Photon.Deterministic;
using Quantum.App.Entity;
using Quantum.Physics3D;

namespace Quantum.App.Boids
{
    public class BoidMovementSystem : SystemMainThreadFilter<BoidMovementSystem.BoidFilter>
    {
        public unsafe struct BoidFilter
        {
            public EntityRef EntityRef;
            public Transform3D* Transform;
            public Boid* Boid;
        }
        
        public override unsafe void OnInit(Frame f)
        {
            var controller = f.GetSingleton<BoidsController>();
            var list = f.AllocateList<Boid>();
            var data = f.FindAsset<BoidControllerData>(controller.Data.Id);
            for (int i = 0; i < data.BoidsCount; i++)
            {
                var boidEntity = EntityFactory.Create(f, data.BoidPrototype.Id);
                var boid = f.Unsafe.GetPointer<Boid>(boidEntity);
                boid->velocity = new FPVector3(Random(f, boid->maxSpeed), Random(f, boid->maxSpeed), Random(f, boid->maxSpeed));
                f.Unsafe.GetPointer<Transform3D>(boidEntity)->Position = new FPVector3(Random(f, boid->maxDistanceFromAnchor), Random(f, boid->maxDistanceFromAnchor), Random(f, boid->maxDistanceFromAnchor));
                list.Add(*boid);
            }
            controller.Boids = list;
        }

        private unsafe FP Random(Frame frame, FP value)
        {
            return frame.Global->RngSession.Next(-value, value);
        }

        public override unsafe void Update(Frame f, ref BoidFilter filter)
        {
            CalculateVelocity(f, filter.Boid, filter.Transform);
            UpdateRotation(f, filter.Boid, filter.Transform);
            UpdatePosition(f, filter.Boid, filter.Transform);
        }
        
        private unsafe void CalculateVelocity(Frame frame, Boid* boid, Transform3D* boidTransform)
        {
            var sphereee = Shape3D.CreateSphere(FP._0_20);
            var obstaclesHit = frame.Physics3D.ShapeCast(boidTransform->Position, FPQuaternion.Identity, 
                sphereee, boidTransform->Forward * boid->obstacleCheckDistance, boid->obstaclesLayer);
            boid->ObstacleHit = obstaclesHit ?? default(Hit3D);
            if (obstaclesHit.HasValue)
            {
                boid->velocity = FPVector3.Reflect(boid->velocity, obstaclesHit.Value.Normal);
                return;
            }

            var sphere = Shape3D.CreateSphere(boid->cohesionRadius);
            var boids = frame.Physics3D.OverlapShape(boidTransform->Position, FPQuaternion.Identity, sphere, boid->boidsLayer);
            if (boids.Count < 2) return;

            boid->velocity = FPVector3.Zero;
            var cohesion = FPVector3.Zero;
            var separation = FPVector3.Zero;
            var separationCount = 0;
            var alignment = FPVector3.Zero;
            var avoidance = FPVector3.Zero;
        
            for (var i = 0; i < boids.Count && i < boid->maxBoids; i++)
            {
                if(!frame.TryGet(boids[i].Entity, out Boid neighbour) || neighbour.Equals(*boid)) continue;
                
                var neighbourTransform = frame.Get<Transform3D>(boids[i].Entity);
                cohesion += neighbourTransform.Position;
                alignment += neighbour.velocity;
                var vector = boidTransform->Position - neighbourTransform.Position;
                if (vector.SqrMagnitude > 0 && vector.SqrMagnitude < boid->separationDistance * boid->separationDistance)
                {
                    var scaler = FPMath.Clamp01(FP._1 - vector.SqrMagnitude / boid->separationDistance);
                    separation += vector * (scaler / vector.SqrMagnitude);
                    separationCount++;
                }

                if (!neighbour.ObstacleHit.Equals(default(Hit3D)))
                {
                    var distance = FPVector3.Distance(neighbourTransform.Position, neighbour.ObstacleHit.Point);
                    avoidance += FPVector3.Reflect(neighbour.velocity, neighbour.ObstacleHit.Normal) / distance;
                }
            }

            var count = (boids.Count > boid->maxBoids ? boid->maxBoids : boids.Count);
            avoidance = avoidance / count;
            avoidance = FPVector3.ClampMagnitude(avoidance, boid->maxSpeed);
            cohesion = cohesion / count;
            cohesion = FPVector3.ClampMagnitude(cohesion - boidTransform->Position, boid->maxSpeed);
            cohesion *= boid->cohesionCoefficient;
            if (separationCount > 0)
            {
                separation = separation / separationCount;
                separation = FPVector3.ClampMagnitude(separation, boid->maxSpeed);
                separation *= boid->separationCoefficient;
            }
            alignment = alignment / count;
            alignment = FPVector3.ClampMagnitude(alignment, boid->maxSpeed);
            alignment *= boid->alignmentCoefficient;
            var finalVelocity = cohesion + separation + alignment + avoidance;
            // if (!boid->ObstacleHit.Equals(default(Hit3D)))
            // {
            //     finalVelocity = FPVector3.Reflect(finalVelocity, boid->ObstacleHit.Normal);
            // }
            
            boid->velocity = FPVector3.ClampMagnitude(finalVelocity, boid->maxSpeed);
        }

        private unsafe void UpdateRotation(Frame frame, Boid* boid, Transform3D* transform)
        {
            if (boid->velocity != FPVector3.Zero && transform->Forward != boid->velocity.Normalized)
            {
                transform->Rotation = FPQuaternion.LookRotation(boid->velocity);
                // var next = FPQuaternion.Euler(boid->velocity);
                // transform->Rotation = FPQuaternion.Lerp(transform->Rotation, next, boid->turnSpeed * frame.DeltaTime);
            }
        }

        private unsafe void UpdatePosition(Frame frame, Boid* boid, Transform3D* transform)
        {
            if (FPVector3.Distance(transform->Position, boid->anchor) > boid->maxDistanceFromAnchor)
            {
                boid->velocity += (boid->anchor - transform->Position) / boid->maxDistanceFromAnchor;
            }
            transform->Position += boid->velocity * frame.DeltaTime;
        }
    }
}