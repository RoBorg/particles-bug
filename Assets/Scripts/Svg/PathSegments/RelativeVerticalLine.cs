using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    /**
     * A line from the current cursor position to the destination
     */
    public class RelativeVerticalLine : PathSegment
    {
        public float offset;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            var pos = startPosition;
            pos.y += offset * t;
            return pos;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            var direction = new Vector3();
            if (offset >= 0)
                direction.y = 1;
            else
                direction.y = -1;

            return direction;
        }
        public override void Normalize(Vector3 startPosition, float scale)
        {
            offset *= scale;
        }
    }
}
