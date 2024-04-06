using Unity.Entities;

namespace VladislavTsurikov._2DSpaceshipController.Runtime.Components
{
    public struct SpaceshipController : IComponentData
    {
        public float MoveAcceleration;
        public float RotationSpeed;
    }
}