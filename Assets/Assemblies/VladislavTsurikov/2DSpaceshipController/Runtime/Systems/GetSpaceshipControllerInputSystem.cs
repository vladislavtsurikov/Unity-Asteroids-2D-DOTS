using Unity.Entities;
using UnityEngine;
using VladislavTsurikov._2DSpaceshipController.Runtime.Components;

namespace VladislavTsurikov._2DSpaceshipController.Runtime.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class GetSpaceshipControllerInputSystem : SystemBase
    {
        private PlayerAction _inputAction;
        
        protected override void OnCreate()
        {
            RequireForUpdate<SpaceshipControllerInput>();
            _inputAction = new PlayerAction();
        }

        protected override void OnStartRunning()
        {
            _inputAction.Enable();
        }

        protected override void OnStopRunning()
        {
            _inputAction.Disable();
        }

        protected override void OnUpdate()
        {
            foreach (RefRW<SpaceshipControllerInput> playerControllerInput in 
                     SystemAPI.Query<RefRW<SpaceshipControllerInput>>())
            {
                playerControllerInput.ValueRW.Value = _inputAction.Player.Movement.ReadValue<Vector2>();
            }
        }
    }
}