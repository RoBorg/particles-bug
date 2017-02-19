using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    /**
     * A line from the current cursor position to the destination
     */
    public class RelativeHorizontalLine : PathSegment
    {
        public float offset;

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            var pos = startPosition;
            pos.x += offset * t;
            return pos;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            var direction = new Vector3();
            if (offset >= 0)
                direction.x = 1;
            else
                direction.x = -1;

            return direction;
        }
        public override void Normalize(Vector3 startPosition, float scale)
        {
            offset *= scale;
        }
    }
}