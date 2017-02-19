using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Assertions;
using System;
using MagicDuel.Svg;

namespace MagicDuel.Sigils
{
    public class Sigil
    {
        /** @var name The display name of the sigil */
        public string name;

        /** @var description The textual description for the spellbook */
        public string description;

        /** @var paths The path segments to draw the sigil */
        protected SvgParser svg;

        /** @var rank The mastery rank at which this sigil becomes available */
        public int rank;

        /** @var unlocked Whether this sigil has been unlocked yet */
        public bool unlocked = true; // ToDo: logic for this

        /** @var featuresHash A hash of the features describing this sigil */
        public string featuresHash;

        /** @var points A cache of the points at a distancePerStep */
        protected Dictionary<float, Vector2[]> points = new Dictionary<float, Vector2[]>();

        public Sigil(Recogniser recogniser, SvgParser svg)
        {
            this.svg = svg;

            featuresHash = recogniser.run(svg.GetPoints(0.05f));
        }

        /**
         * Draw the sigil
         * 
         * A "point" object is created for every point along the path
         * 
         * @param parent The object to parent the points to
         * @param point The object to create at each point on the path
         */
        public void Draw(GameObject parent, GameObject point)
        {
            svg.Draw(parent, point);
        }

        /**
         * Get a list of points along the entire path
         * @param distancePerStep How far apart to space the points
         */
        public Vector2[] GetPoints(float distancePerStep = 0.01f)
        {
            if (!points.ContainsKey(distancePerStep))
            {
                points[distancePerStep] = svg.GetPoints(distancePerStep);
            }

            return points[distancePerStep];
        }
    }
}