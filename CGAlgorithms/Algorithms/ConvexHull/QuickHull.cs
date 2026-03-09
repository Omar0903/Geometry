using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGUtilities.Enums;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            {
                if (points.Count == 0) return;

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

                // find leftmost and rightmost
                Point minX = points[0];
                Point maxX = points[0];

                foreach (var p in points)
                {
                    if (p.X < minX.X)
                        minX = p;

                    if (p.X > maxX.X)
                        maxX = p;
                }

                outPoints.Add(minX);
                outPoints.Add(maxX);

                List<Point> leftSet = new List<Point>();
                List<Point> rightSet = new List<Point>();

                Line baseLine = new Line(minX, maxX);

                foreach (var p in points)
                {
                    if (p.Equals(minX) || p.Equals(maxX)) continue;

                    var turn = HelperMethods.CheckTurn(baseLine, p);

                    if (turn == TurnType.Left)
                        leftSet.Add(p);
                    else if (turn == TurnType.Right)
                        rightSet.Add(p);
                }

                FindHull(minX, maxX, leftSet, outPoints);
                FindHull(maxX, minX, rightSet, outPoints);

                // create hull lines
                for (int i = 0; i < outPoints.Count; i++)
                {
                    Point a = outPoints[i];
                    Point b = outPoints[(i + 1) % outPoints.Count];
                    outLines.Add(new Line(a, b));
                }
            }

            void FindHull(Point a, Point b, List<Point> set, List<Point> hull)
            {
                if (set.Count == 0)
                    return;

                double maxDistance = -1;
                Point farthest = null;

                Line ab = new Line(a, b);

                foreach (var p in set)
                {
                    double dist = DistanceFromLine(ab, p);
                    if (dist > maxDistance)
                    {
                        maxDistance = dist;
                        farthest = p;
                    }
                }

                hull.Insert(hull.IndexOf(b), farthest);

                List<Point> leftSetAP = new List<Point>();
                List<Point> leftSetPB = new List<Point>();

                Line ap = new Line(a, farthest);
                Line pb = new Line(farthest, b);

                foreach (var p in set)
                {
                    if (p.Equals(farthest)) continue;

                    if (HelperMethods.CheckTurn(ap, p) == TurnType.Left)
                        leftSetAP.Add(p);
                    else if (HelperMethods.CheckTurn(pb, p) == TurnType.Left)
                        leftSetPB.Add(p);
                }

                FindHull(a, farthest, leftSetAP, hull);
                FindHull(farthest, b, leftSetPB, hull);
            }

            double DistanceFromLine(Line l, Point p)
            {
                Point a = l.Start;
                Point b = l.End;

                return Math.Abs((b.X - a.X) * (a.Y - p.Y) -
                                (a.X - p.X) * (b.Y - a.Y));
            }

        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
