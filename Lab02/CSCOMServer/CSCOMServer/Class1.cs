using System;
using System.Runtime.InteropServices;

namespace CSCOMServer
{
    [ComVisible(true)]
    [Guid("2c88f579-faec-4c32-be76-763723e81a4d")]
    public interface ISerializable
    {
        void ReadData(string data);
    }

    [ComVisible(true)]
    [Guid("dd3fb53c-afe9-4e7d-b288-f00a81be90ce")]
    public interface IPoint : ISerializable
    {
        double X { get; set; }
        double Y { get; set; }

        int DistanceTo(IPoint other);
    }

    [ComVisible(true)]
    [Guid("ff65d9ff-83db-4d95-8ab0-e517abb6383a")]
    public interface ICircumference : ISerializable
    {
        IPoint Center { get; set; }
        double Radius { get; set; }

        bool Contains(IPoint point);
    }

    [ComVisible(true)]
    [Guid("68f53439-c0f1-484a-9d3d-748c84e86146")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Point : IPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point() { }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public int DistanceTo(IPoint other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);

            return (int)Math.Round(distance);
        }

        public void ReadData(string data)
        {
            var points = data.Split(';');

            X = double.Parse(points[0]);
            Y = double.Parse(points[1]);
        }

        public override string ToString()
        {
            return $"({X};{Y})";
        }
    }

    [ComVisible(true)]
    [Guid("9cdea747-c857-411d-be1e-5ee65b9d4f08")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Circle : ICircumference
    {
        public IPoint Center { get; set; }
        public double Radius { get; set; }

        public Circle() { }

        public Circle(IPoint center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Contains(IPoint point)
        {
            return Center.DistanceTo(point) <= Radius;
        }

        public void ReadData(string data)
        {
            var parts = data.Split(';');

            Center = new Point(double.Parse(parts[0]), double.Parse(parts[1]));
            Radius = double.Parse(parts[2]);
        }

        public override string ToString()
        {
            return $"Center: {Center}, Radius: {Radius}";
        }
    }
}