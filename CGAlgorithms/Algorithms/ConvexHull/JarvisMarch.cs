using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGUtilities.Enums;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
                return;
            }

            if (points.Count == 2)
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
                outLines.Add(new Line(points[0], points[1]));
                return;
            }

            if (points.Count == 3)
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
                outPoints.Add(points[2]);

                outLines.Add(new Line(points[0], points[1]));
                outLines.Add(new Line(points[1], points[2]));
                outLines.Add(new Line(points[2], points[0]));

                return;
            }

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[0].Y ||
                   (points[i].Y == points[0].Y && points[i].X < points[0].X))
                {
                    Point temp = points[0];
                    points[0] = points[i];
                    points[i] = temp;
                }
            }

            Point start = points[0];
            Point current = start;

            do
            {
                outPoints.Add(current);

                Point next = points[0];
                if (next.Equals(current))
                    next = points[1];

                foreach (var p in points)
                {
                    if (p.Equals(current)) continue;

                    Line l = new Line(current, next);
                    TurnType turn = HelperMethods.CheckTurn(l, p);

                    if (turn == TurnType.Right)
                    {
                        next = p;
                    }
                    else if (turn == TurnType.Colinear)
                    {
                        double d1 = DistanceSquared(current, next);
                        double d2 = DistanceSquared(current, p);

                        if (d2 > d1)
                            next = p;
                    }
                }

                current = next;

            } while (!current.Equals(start));

            for (int i = 0; i < outPoints.Count; i++)
            {
                Point a = outPoints[i];
                Point b = outPoints[(i + 1) % outPoints.Count];
                outLines.Add(new Line(a, b));
            }
        }

        double DistanceSquared(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) +
                   (a.Y - b.Y) * (a.Y - b.Y);
        }
        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
