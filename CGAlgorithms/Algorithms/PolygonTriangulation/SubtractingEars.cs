using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    public class SubtractingEars : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref 
List<CGUtilities.Point> outPoints,ref List<CGUtilities.Line> outLines,ref List<CGUtilities.Polygon> outPolygons)
        {
            outLines = new List<CGUtilities.Line>();
            outPoints = new List<CGUtilities.Point>();
            outPolygons = new List<CGUtilities.Polygon>();

            if (points == null || points.Count < 3)
                return;

            List<CGUtilities.Point> poly = new List<CGUtilities.Point>(points);

            // Make polygon counter-clockwise
            if (PolygonArea(poly) < 0)
                poly.Reverse();

            while (poly.Count > 3)
            {
                bool earFound = false;

                for (int i = 0; i < poly.Count; i++)
                {
                    CGUtilities.Point prev = poly[(i - 1 + poly.Count) % poly.Count];
                    CGUtilities.Point curr = poly[i];
                    CGUtilities.Point next = poly[(i + 1) % poly.Count];

                    // Ear must be convex
                    if (!IsConvex(prev, curr, next))
                        continue;

                    bool hasPointInside = false;

                    for (int j = 0; j < poly.Count; j++)
                    {
                        CGUtilities.Point testPoint = poly[j];

                        if (SamePoint(testPoint, prev) ||
                            SamePoint(testPoint, curr) ||
                            SamePoint(testPoint, next))
                            continue;

                        if (PointInTriangle(testPoint, prev, curr, next))
                        {
                            hasPointInside = true;
                            break;
                        }
                    }

                    // Found ear
                    if (!hasPointInside)
                    {
                        if (!IsPolygonEdge(prev, next, poly))
                        {
                            outLines.Add(new CGUtilities.Line(prev, next));
                        }

                        poly.RemoveAt(i);

                        earFound = true;
                        break;
                    }
                }

                // Safety check
                if (!earFound)
                    break;
            }
        }

        public override string ToString()
        {
            return "Subtracting Ears";
        }

        private double PolygonArea(List<CGUtilities.Point> pts)
        {
            double area = 0;

            for (int i = 0; i < pts.Count; i++)
            {
                CGUtilities.Point p1 = pts[i];
                CGUtilities.Point p2 = pts[(i + 1) % pts.Count];

                area += (p1.X * p2.Y) - (p2.X * p1.Y);
            }

            return area / 2.0;
        }

        private bool IsConvex(CGUtilities.Point a, CGUtilities.Point b, CGUtilities.Point c)
        {
            return Cross(a, b, c) > 0;
        }

        private double Cross(CGUtilities.Point a, CGUtilities.Point b, CGUtilities.Point c)
        {
            return ((b.X - a.X) * (c.Y - a.Y)) -
                   ((b.Y - a.Y) * (c.X - a.X));
        }

        private bool PointInTriangle(
            CGUtilities.Point p,
            CGUtilities.Point a,
            CGUtilities.Point b,
            CGUtilities.Point c)
        {
            double d1 = Cross(p, a, b);
            double d2 = Cross(p, b, c);
            double d3 = Cross(p, c, a);

            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNeg && hasPos);
        }

        private bool IsPolygonEdge(
            CGUtilities.Point a,
            CGUtilities.Point b,
            List<CGUtilities.Point> poly)
        {
            for (int i = 0; i < poly.Count; i++)
            {
                CGUtilities.Point p1 = poly[i];
                CGUtilities.Point p2 = poly[(i + 1) % poly.Count];

                if ((SamePoint(p1, a) && SamePoint(p2, b)) ||
                    (SamePoint(p1, b) && SamePoint(p2, a)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool SamePoint(CGUtilities.Point p1, CGUtilities.Point p2)
        {
            return Math.Abs(p1.X - p2.X) < 1e-9 &&
                   Math.Abs(p1.Y - p2.Y) < 1e-9;
        }
    }
}