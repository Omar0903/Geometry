using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> uniquePoints = new List<Point>();
            foreach (var p in points)
            {
                if (!uniquePoints.Any(x => x.X == p.X && x.Y == p.Y))
                    uniquePoints.Add(p);
            }

            if (uniquePoints.Count <= 3)
            {
                outPoints = uniquePoints;
                return;
            }

            List<Point> extremePoints = new List<Point>();

            for (int i = 0; i < uniquePoints.Count; i++)
            {
                bool isInsideSomething = false;

                for (int j = 0; j < uniquePoints.Count; j++)
                {
                    for (int k = 0; k < uniquePoints.Count; k++)
                    {
                        for (int l = 0; l < uniquePoints.Count; l++)
                        {
                            if (i == j || i == k || i == l || j == k || j == l || k == l)
                                continue;

                            if (IsPointInsideOrOnTriangle(uniquePoints[i], uniquePoints[j], uniquePoints[k], uniquePoints[l]))
                            {
                                isInsideSomething = true;
                                break;
                            }
                        }
                        if (isInsideSomething) break;
                    }
                    if (isInsideSomething) break;
                }

                if (!isInsideSomething)
                {
                    extremePoints.Add(uniquePoints[i]);
                }
            }

            outPoints = extremePoints;
        }

        private bool IsPointInsideOrOnTriangle(Point p, Point a, Point b, Point c)
        {
            Enums.TurnType t1 = HelperMethods.CheckTurn(new Line(a, b), p);
            Enums.TurnType t2 = HelperMethods.CheckTurn(new Line(b, c), p);
            Enums.TurnType t3 = HelperMethods.CheckTurn(new Line(c, a), p);

            if (t1 == Enums.TurnType.Colinear && OnSegment(p, a, b)) return true;
            if (t2 == Enums.TurnType.Colinear && OnSegment(p, b, c)) return true;
            if (t3 == Enums.TurnType.Colinear && OnSegment(p, c, a)) return true;

            if ((t1 == Enums.TurnType.Left && t2 == Enums.TurnType.Left && t3 == Enums.TurnType.Left) ||
                (t1 == Enums.TurnType.Right && t2 == Enums.TurnType.Right && t3 == Enums.TurnType.Right))
            {
                return true;
            }

            return false;
        }

        private bool OnSegment(Point p, Point a, Point b)
        {
            return p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X) &&
                   p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y);
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}