using UnityEngine;

namespace MagicDuel.Svg.PathSegments
{
    public class AbsoluteCubicBezierCurve : PathSegment
    {
        public Vector3 endPosition;
        public Vector3[] controlPoints;

        public AbsoluteCubicBezierCurve()
        {
            controlPoints = new Vector3[2];
        }

        public override Vector3 GetPosition(Vector3 startPosition, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * startPosition +
                3f * oneMinusT * oneMinusT * t * controlPoints[0] +
                3f * oneMinusT * t * t * controlPoints[1] +
                t * t * t * endPosition;
        }

        public override Vector3 GetDirection(Vector3 startPosition, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            var velocity = 3f * oneMinusT * oneMinusT * (controlPoints[0] - startPosition) +
                6f * oneMinusT * t * (controlPoints[1] - controlPoints[0]) +
                3f * t * t * (endPosition - controlPoints[1]);

            // Convert from velocity to direction
            velocity.Normalize();

            return velocity;
        }


        public override void Normalize(Vector3 startPosition, float scale)
        {
            endPosition = (endPosition + startPosition) * scale;

            for (var i = 0; i < 2; ++i)
                controlPoints[i] = (controlPoints[i] + startPosition) * scale;
        }
    }
}
