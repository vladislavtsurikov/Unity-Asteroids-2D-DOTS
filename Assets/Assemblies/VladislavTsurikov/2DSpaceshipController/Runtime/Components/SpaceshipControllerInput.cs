using Unity.Entities;
using Unity.Mathematics;

namespace VladislavTsurikov._2DSpaceshipController.Runtime.Components
{
    public struct SpaceshipControllerInput : IComponentData
    {
        public float2 Value;
    }
}