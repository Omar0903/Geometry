using System;
using System.Collections.Generic;
using System.Linq;
using CGUtilities;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines,
            List<Polygon> polygons,
            ref List<Point> outPoints,
            ref List<Line> outLines,
            ref List<Polygon> outPolygons)
        {
            outPoints = new List<Point>();

            // Sort lines based on minimum X coordinate
            var sortedLines = lines.OrderBy(l => Math.Min(l.Start.X, l.End.X)).ToList();

            for (int i = 0; i < sortedLines.Count; i++)
            {
                for (int j = i + 1; j < sortedLines.Count; j++)
                {
                    Point intersection;

                    if (SegmentsIntersect(sortedLines[i], sortedLines[j], out intersection))
                    {
                        outPoints.Add(intersection);
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }

        private bool SegmentsIntersect(Line l1, Line l2, out Point intersection)
        {
            intersection = new Point(0, 0);

            Point p1 = l1.Start;
            Point q1 = l1.End;
            Point p2 = l2.Start;
            Point q2 = l2.End;

            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
            {
                intersection = GetIntersectionPoint(l1, l2);
                return true;
            }

            return false;
        }

        private int Orientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) -
                         (q.X - p.X) * (r.Y - q.Y);

            if (Math.Abs(val) < 1e-9)
                return 0; // Collinear

            return (val > 0) ? 1 : 2; // Clockwise or Counterclockwise
        }

        private Point GetIntersectionPoint(Line l1, Line l2)
        {
            double x1 = l1.Start.X;
            double y1 = l1.Start.Y;
            double x2 = l1.End.X;
            double y2 = l1.End.Y;

            double x3 = l2.Start.X;
            double y3 = l2.Start.Y;
            double x4 = l2.End.X;
            double y4 = l2.End.Y;

            double denom = (x1 - x2) * (y3 - y4) -
                           (y1 - y2) * (x3 - x4);

            if (Math.Abs(denom) < 1e-9)
                return new Point(0, 0);

            double px = ((x1 * y2 - y1 * x2) * (x3 - x4) -
                        (x1 - x2) * (x3 * y4 - y3 * x4)) / denom;

            double py = ((x1 * y2 - y1 * x2) * (y3 - y4) -
                        (y1 - y2) * (x3 * y4 - y3 * x4)) / denom;

            return new Point(px, py);
        }
    }
}