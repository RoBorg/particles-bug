using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    /**
     * A line from the current cursor position to the destination
     */
    public class AbsoluteLine : PathSegment
    {
        public Vector3 destination;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            return startPosition + (destination - startPosition) * t;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            var direction = destination - startPosition;
            direction.Normalize();
            return direction;
        }

        public override void Normalize(Vector3 startPosition, float scale)
        {
            destination = (destination + startPosition) * scale;
        }
    }
}
