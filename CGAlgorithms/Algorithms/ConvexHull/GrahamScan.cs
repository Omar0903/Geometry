using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> uniquePoints = points.Distinct().ToList();
            if (uniquePoints.Count <= 3)
            {
                outPoints = uniquePoints;
                return;
            }

            Point pivot = uniquePoints[0];
            foreach (var p in uniquePoints)
            {
                if (p.Y < pivot.Y || (p.Y == pivot.Y && p.X < pivot.X))
                {
                    pivot = p;
                }
            }

            var sortedPoints = uniquePoints
                .Where(p => p != pivot)
                .OrderBy(p => Math.Atan2(p.Y - pivot.Y, p.X - pivot.X))
                .ThenBy(p => Math.Sqrt(Math.Pow(p.X - pivot.X, 2) + Math.Pow(p.Y - pivot.Y, 2)))
                .ToList();

            Stack<Point> hull = new Stack<Point>();
            hull.Push(pivot);
            hull.Push(sortedPoints[0]);

            for (int i = 1; i < sortedPoints.Count; i++)
            {
                Point current = sortedPoints[i];

                while (hull.Count >= 2)
                {
                    Point top = hull.Pop();
                    Point nextToTop = hull.Peek();

                    Enums.TurnType turn = HelperMethods.CheckTurn(new Line(nextToTop, top), current);

                    if (turn == Enums.TurnType.Left)
                    {
                        hull.Push(top);
                        break;
                    }
                }

                hull.Push(current);
            }

            outPoints = hull.ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}