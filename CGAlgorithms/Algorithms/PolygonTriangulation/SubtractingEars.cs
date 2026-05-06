using System;
using System.Collections.Generic;
using System.Linq;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class SubtractingEars : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines,
            List<Polygon> polygons,
            ref List<Point> outPoints,
            ref List<Line> outLines,
            ref List<Polygon> outPolygons)
        {
            outLines = new List<Line>();

            List<Point> poly = new List<Point>(points);

            if (PolygonArea(poly) < 0)
                poly.Reverse();

            while (poly.Count > 3)
            {
                bool earFound = false;

                for (int i = 0; i < poly.Count; i++)
                {
                    Point prev = poly[(i - 1 + poly.Count) % poly.Count];
                    Point curr = poly[i];
                    Point next = poly[(i + 1) % poly.Count];

                    if (IsConvex(prev, curr, next))
                    {
                        bool hasPointInside = false;

                        for (int j = 0; j < poly.Count; j++)
                        {
                            if (poly[j] == prev ||
                                poly[j] == curr ||
                                poly[j] == next)
                                continue;

                            if (PointInTriangle(poly[j], prev, curr, next))
                            {
                                hasPointInside = true;
                                break;
                            }
                        }

                        if (!hasPointInside)
                        {
                            // add triangulation diagonal only
                            if (!IsPolygonEdge(prev, next, poly))
                            {
                                outLines.Add(new Line(prev, next));
                            }

                            // remove ear vertex
                            poly.RemoveAt(i);

                            earFound = true;
                            break;
                        }
                    }
                }

                if (!earFound)
                    break;
            }
        }

        public override string ToString()
        {
            return "Subtracting Ears";
        }

        private double PolygonArea(List<Point> pts)
        {
            double area = 0;

            for (int i = 0; i < pts.Count; i++)
            {
                Point p1 = pts[i];
                Point p2 = pts[(i + 1) % pts.Count];

                area += (p1.X * p2.Y - p2.X * p1.Y);
            }

            return area / 2.0;
        }

        private bool IsConvex(Point a, Point b, Point c)
        {
            return Cross(a, b, c) > 0;
        }

        private double Cross(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - a.Y) -
                   (b.Y - a.Y) * (c.X - a.X);
        }

        private bool PointInTriangle(Point p, Point a, Point b, Point c)
        {
            double d1 = Cross(p, a, b);
            double d2 = Cross(p, b, c);
            double d3 = Cross(p, c, a);

            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNeg && hasPos);
        }

        private bool IsPolygonEdge(Point a, Point b, List<Point> poly)
        {
            for (int i = 0; i < poly.Count; i++)
            {
                Point p1 = poly[i];
                Point p2 = poly[(i + 1) % poly.Count];

                if ((p1 == a && p2 == b) ||
                    (p1 == b && p2 == a))
                    return true;
            }

            return false;
        }
    }
}