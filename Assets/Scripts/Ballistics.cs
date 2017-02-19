using UnityEngine;

namespace MagicDuel
{
    public class Ballistics : MonoBehaviour
    {
        /// <summary>
        /// Throw an object towards a target
        /// </summary>
        /// <param name="objectToThrow">The object to throw</param>
        /// <param name="target">The position to throw at</param>
        /// <param name="speed">The speed to throw at</param>
        /// <param name="direct">Whether to take a direct or indirect path (to arc up and hit the target on the way back down)</param>
        public static Vector3? GetForce(Vector3 start, Vector3 target, float speed, bool direct = true)
        {
            Vector3 s0;
            Vector3 s1;

            var numSolutions = Fts.solve_ballistic_arc(start, speed, target, Vector3.zero, Physics.gravity.magnitude, out s0, out s1);

            if (numSolutions == 0)
            {
                return null;
            }

            return direct || (numSolutions < 2) ? s0 : s1;
        }
    }
}
