using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    public class SweepLine:Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref
 List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            outPoints = new List<CGUtilities.Point>();
            outLines = new List<CGUtilities.Line>();
            outPolygons = new List<CGUtilities.Polygon>();

            // Sort lines by minimum X
            List<CGUtilities.Line> sortedLines =
                lines.OrderBy(l => Math.Min(l.Start.X, l.End.X)).ToList();

            for (int i = 0; i < sortedLines.Count; i++)
            {
                for (int j = i + 1; j < sortedLines.Count; j++)
                {
                    CGUtilities.Point intersection;

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

        private bool SegmentsIntersect(
            CGUtilities.Line l1,
            CGUtilities.Line l2,
            out CGUtilities.Point intersection)
        {
            intersection = new CGUtilities.Point(0, 0);

            CGUtilities.Point p1 = l1.Start;
            CGUtilities.Point q1 = l1.End;

            CGUtilities.Point p2 = l2.Start;
            CGUtilities.Point q2 = l2.End;

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

        private int Orientation(
            CGUtilities.Point p,
            CGUtilities.Point q,
            CGUtilities.Point r)
        {
            double val =
                (q.Y - p.Y) * (r.X - q.X) -
                (q.X - p.X) * (r.Y - q.Y);

            if (Math.Abs(val) < 1e-9)
                return 0;

            return (val > 0) ? 1 : 2;
        }

        private CGUtilities.Point GetIntersectionPoint(
            CGUtilities.Line l1,
            CGUtilities.Line l2)
        {
            double x1 = l1.Start.X;
            double y1 = l1.Start.Y;

            double x2 = l1.End.X;
            double y2 = l1.End.Y;

            double x3 = l2.Start.X;
            double y3 = l2.Start.Y;

            double x4 = l2.End.X;
            double y4 = l2.End.Y;

            double denom =
                (x1 - x2) * (y3 - y4) -
                (y1 - y2) * (x3 - x4);

            if (Math.Abs(denom) < 1e-9)
                return new CGUtilities.Point(0, 0);

            double px =
                ((x1 * y2 - y1 * x2) * (x3 - x4) -
                (x1 - x2) * (x3 * y4 - y3 * x4)) / denom;

            double py =
                ((x1 * y2 - y1 * x2) * (y3 - y4) -
                (y1 - y2) * (x3 * y4 - y3 * x4)) / denom;

            return new CGUtilities.Point(px, py);
        }
    }
}