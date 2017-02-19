using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    /**
     * A line from the current cursor position to the offset
     */
    public class RelativeLine : PathSegment
    {
        public Vector3 offset;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            return startPosition + offset * t;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            var direction = offset;
            direction.Normalize();
            return direction;
        }

        public override void Normalize(Vector3 startPosition, float scale)
        {
            offset *= scale;
        }
    }
}
