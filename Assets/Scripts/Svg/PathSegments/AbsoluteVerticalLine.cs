using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    /**
     * A line from the current cursor position to the destination
     */
    public class AbsoluteVerticalLine : PathSegment
    {
        public float position;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            var endPosition = new Vector3();
            endPosition.x = startPosition.x;
            endPosition.y = position;
            endPosition.z = startPosition.z;

            return startPosition + (endPosition - startPosition) * t;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            var direction = new Vector3();
            if (position >= startPosition.y)
                direction.x = 1;
            else
                direction.x = -1;

            return direction;
        }

        public override void Normalize(Vector3 startPosition, float scale)
        {
            position = (position + startPosition.y) * scale;
        }
    }
}
