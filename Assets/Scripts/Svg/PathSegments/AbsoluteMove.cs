using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    public class AbsoluteMove : PathSegment
    {
        public Vector3 moveTo;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            return moveTo;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            return Vector3.zero;
        }

        public override void Normalize(Vector3 startPosition, float scale)
        {
            moveTo = (moveTo + startPosition) * scale;
        }

        public override float CalculateLength()
        {
            return 0;
        }
    }
}
