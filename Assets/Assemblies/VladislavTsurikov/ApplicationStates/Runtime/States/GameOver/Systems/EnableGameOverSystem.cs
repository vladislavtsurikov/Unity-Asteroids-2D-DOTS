using System;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Systems
{
    [UpdateInGroup(typeof(GameOverSystemGroup))]
    public partial class EnableGameOverSystem : SystemBase
    {
        public event Action GameOverEvent;
        
        protected override void OnCreate()
        {
            RequireForUpdate<GameOverEvent>();
            RequireForUpdate<GameOverEnableableComponent>();
        }

        protected override void OnUpdate()
        {
            foreach (var (_, entity) in SystemAPI.Query<GameOverEnableableComponent>().WithEntityAccess().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                EntityManager.SetComponentEnabled<GameOverEnableableComponent>(entity, true);
                GameOverEvent?.Invoke();
            }
        }
    }
}