using CAFU.Core;
using UnityEngine;

namespace CAFU.Rotator.Application.ValueObject
{
    public struct RotatePosition : IStructure
    {
        public Vector2 FromPosition { get; }
        public Vector3 ToPosition { get; }

        public RotatePosition(Vector2 fromPosition, Vector3 toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }
    }
}
