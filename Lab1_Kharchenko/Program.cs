using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Kharchenko
{
    public interface ISerializable
    {
        void ReadData(string data);
    }

    public interface I3DPoint : ISerializable
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        I3DPoint Translate(I3DVector vector, double scale);
    }

    public interface I3DVector : ISerializable
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        double Length();
        I3DVector UnitVector();
    }

    public class Point3D : I3DPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D() { }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public I3DPoint Translate(I3DVector vector, double scale)
        {
            return new Point3D(X + vector.X * scale, Y + vector.Y * scale, Z + vector.Z * scale);
        }

        public void ReadData(string data)
        {
            var coordinates = data.Split(';');
            X = double.Parse(coordinates[0]);
            Y = double.Parse(coordinates[1]);
            Z = double.Parse(coordinates[2]);
        }

        public override string ToString()
        {
            return $"Point({X}; {Y}; {Z})";
        }
    }

    public class Vector3D : I3DVector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3D() { }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public I3DVector UnitVector()
        {
            double length = Length();
            return new Vector3D(X / length, Y / length, Z / length);
        }

        public void ReadData(string data)
        {
            var components = data.Split(';');
            X = double.Parse(components[0]);
            Y = double.Parse(components[1]);
            Z = double.Parse(components[2]);
        }

        public override string ToString()
        {
            return $"Vector({X}; {Y}; {Z})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            I3DPoint point = new Point3D();
            I3DVector vector = new Vector3D();

            ProcessData(point, vector);

            // Calculate the unit vector in the direction of the input vector
            I3DVector unitVector = vector.UnitVector();

            // Move the point in the direction of the vector, scaled by 2 (twice its length)
            I3DPoint newPoint = point.Translate(vector, 2);

            Console.WriteLine($"Unit vector in the direction of the input vector: {unitVector}");
            Console.WriteLine($"New point after moving along the vector twice its length: {newPoint}");
        }

        private static void ProcessData(I3DPoint point, I3DVector vector)
        {
            Console.WriteLine("Enter data for point and vector in the format: x1;y1;z1 x2;y2;z2");
            var data = Console.ReadLine();

            var splittedData = data.Split(' ');

            point.ReadData(splittedData[0]);
            vector.ReadData(splittedData[1]);
        }
    }

}
