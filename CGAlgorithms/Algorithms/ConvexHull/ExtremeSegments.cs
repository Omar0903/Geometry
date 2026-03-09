using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints = new List<Point>();
            outLines = new List<Line>();
            outPolygons = new List<Polygon>();

            if (points == null || points.Count == 0) return;

            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
                return;
            }

            if (points.Count == 2)
            {
                AddUnique(outPoints, points[0]);
                AddUnique(outPoints, points[1]);
                return;
            }

            List<Point> hull = new List<Point>();
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    bool left = false, right = false;
                    bool validSegment = true;

                    Line l = new Line(points[i], points[j]);

                    for (int k = 0; k < n; k++)
                    {
                        if (k == i || k == j) continue;

                        Enums.TurnType turn = HelperMethods.CheckTurn(l, points[k]);

                        if (turn == Enums.TurnType.Left)
                            left = true;
                        else if (turn == Enums.TurnType.Right)
                            right = true;
                        else
                        {

                            if (!PointOnSegment(points[i], points[j], points[k]))
                            {
                                validSegment = false;
                                break;
                            }
                        }

                        if (left && right)
                        {
                            validSegment = false;
                            break;
                        }
                    }

                    if (validSegment)
                    {
                        AddUnique(hull, points[i]);
                        AddUnique(hull, points[j]);
                    }
                }
            }

            if (hull.Count <= 2)
            {
                outPoints = hull;
                return;
            }

            Point center = new Point(
                hull.Average(p => p.X),
                hull.Average(p => p.Y)
            );

            outPoints = hull
                .OrderBy(p => Math.Atan2(p.Y - center.Y, p.X - center.X))
                .ToList();
        }
        private void AddUnique(List<Point> list, Point p)
        {
            foreach (Point q in list)
            {
                if (Math.Abs(q.X - p.X) < 1e-9 && Math.Abs(q.Y - p.Y) < 1e-9)
                    return;
            }
            list.Add(p);
        }

        private bool PointOnSegment(Point a, Point b, Point p)
        {
            return p.X >= Math.Min(a.X, b.X) - 1e-9 &&
                   p.X <= Math.Max(a.X, b.X) + 1e-9 &&
                   p.Y >= Math.Min(a.Y, b.Y) - 1e-9 &&
                   p.Y <= Math.Max(a.Y, b.Y) + 1e-9;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
