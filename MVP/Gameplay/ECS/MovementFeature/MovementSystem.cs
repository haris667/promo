using Leopotam.Ecs;

namespace Gameplay.ECS.MovementFeature{
    sealed class MovementSystem : IEcsRunSystem {
        // auto-injected fields.
        readonly EcsWorld _world = null;
        
        void IEcsRunSystem.Run () {
            // add your run code here.
        }
    }
}