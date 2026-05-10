using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run( List<Point> points, List<Line> lines,List<Polygon> polygons,ref
            List<Point> outPoints, ref List<Line> outLines,ref List<Polygon> outPolygons)
        {
            if (points == null || points.Count == 0)
            {
                outPoints = new List<Point>();
                return;
            }

            List<Point> pts = points.Distinct().OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            outPoints = DivideHull(pts);
        }

        private List<Point> DivideHull(List<Point> pts)
        {
            // Base case
            if (pts.Count <= 3)
                return ComputeSmallHull(pts);

            int mid = pts.Count / 2;

            List<Point> left = pts.Take(mid).ToList();
            List<Point> right = pts.Skip(mid).ToList();

            // Conquer recursively
            List<Point> leftHull = DivideHull(left);
            List<Point> rightHull = DivideHull(right);

            // Merge
            return MergeHulls(leftHull, rightHull);
        }

        private List<Point> ComputeSmallHull(List<Point> pts)
        {
            pts = pts.Distinct().OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            if (pts.Count <= 2)
                return pts;

            var turn = HelperMethods.CheckTurn(
                new Line(pts[0], pts[1]),
                pts[2]);

            if (turn == Enums.TurnType.Colinear)
                return new List<Point> { pts[0], pts[2] };

            return pts;
        }

        private List<Point> MergeHulls(List<Point> leftHull, List<Point> rightHull)
        {
            List<Point> allPoints = leftHull
                .Concat(rightHull)
                .Distinct()
                .OrderBy(p => p.X)
                .ThenBy(p => p.Y)
                .ToList();

            return MonotoneHull(allPoints);
        }

        private List<Point> MonotoneHull(List<Point> pts)
        {
            if (pts.Count <= 3)
                return pts;

            List<Point> lower = new List<Point>();
            foreach (var p in pts)
            {
                while (lower.Count >= 2 &&
                    HelperMethods.CheckTurn(
                        new Line(lower[lower.Count - 2], lower[lower.Count - 1]),
                        p) != Enums.TurnType.Left)
                {
                    lower.RemoveAt(lower.Count - 1);
                }
                lower.Add(p);
            }

            List<Point> upper = new List<Point>();
            for (int i = pts.Count - 1; i >= 0; i--)
            {
                Point p = pts[i];
                while (upper.Count >= 2 &&
                    HelperMethods.CheckTurn(
                        new Line(upper[upper.Count - 2], upper[upper.Count - 1]),
                        p) != Enums.TurnType.Left)
                {
                    upper.RemoveAt(upper.Count - 1);
                }
                upper.Add(p);
            }

            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            return lower.Concat(upper).ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
    }
}