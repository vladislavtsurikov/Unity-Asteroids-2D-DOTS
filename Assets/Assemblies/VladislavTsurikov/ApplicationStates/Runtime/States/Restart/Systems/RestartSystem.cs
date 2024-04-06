using System;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Systems
{
    [UpdateInGroup(typeof(RestartSystemGroup))]
    public partial class RestartSystem : SystemBase
    {
        public event Action RestartEvent;
        
        protected override void OnCreate()
        {
            RequireForUpdate<RestartEvent>();
        }

        protected override void OnUpdate()
        {
            RestartEvent?.Invoke();
        }
    }
}