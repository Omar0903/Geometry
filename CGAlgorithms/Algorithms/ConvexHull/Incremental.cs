using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons,
    ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points == null || points.Count == 0)
            {
                outPoints = new List<Point>();
                return;
            }

            List<Point> pts = points.Distinct().ToList();

            if (pts.Count <= 3)
            {
                outPoints = pts;
                return;
            }

            // Sort by X then Y
            pts = pts.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            List<Point> lower = new List<Point>();
            foreach (var p in pts)
            {
                while (lower.Count >= 2)
                {
                    var turn = HelperMethods.CheckTurn(
                        new Line(lower[lower.Count - 2], lower[lower.Count - 1]),
                        p
                    );

                    if (turn != Enums.TurnType.Left)
                        lower.RemoveAt(lower.Count - 1);
                    else
                        break;
                }
                lower.Add(p);
            }

            List<Point> upper = new List<Point>();
            for (int i = pts.Count - 1; i >= 0; i--)
            {
                var p = pts[i];
                while (upper.Count >= 2)
                {
                    var turn = HelperMethods.CheckTurn(
                        new Line(upper[upper.Count - 2], upper[upper.Count - 1]),
                        p
                    );

                    if (turn != Enums.TurnType.Left)
                        upper.RemoveAt(upper.Count - 1);
                    else
                        break;
                }
                upper.Add(p);
            }

            // remove duplicate endpoints
            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            outPoints = lower.Concat(upper).ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
