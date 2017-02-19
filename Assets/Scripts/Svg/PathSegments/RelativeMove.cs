using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    public class RelativeMove : PathSegment
    {
        public Vector3 offset;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            return startPosition + offset;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            return Vector3.zero;
        }
        public override void Normalize(Vector3 startPosition, float scale)
        {
            offset *= scale;
        }

        public override float CalculateLength()
        {
            return 0;
        }
    }
}
