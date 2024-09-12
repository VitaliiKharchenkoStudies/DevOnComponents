using System;
using System.Runtime.InteropServices;

namespace COMServer
{
    [ComVisible(true)]
    [Guid("12345678-90AB-CDEF-1234-567890ABCDEF")]
    public interface ISerializable
    {
        void ReadData(string data);
    }

    [ComVisible(true)]
    [Guid("23456789-90AB-CDEF-1234-567890ABCDEF")]
    public interface I3DPoint : ISerializable
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        int DistanceTo(I3DPoint other);
        I3DPoint Translate(I3DVector vector, double scale);
    }

    [ComVisible(true)]
    [Guid("34567890-90AB-CDEF-1234-567890ABCDEF")]
    public interface I3DVector : ISerializable
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        double Length();
        I3DVector UnitVector();
    }

    [ComVisible(true)]
    [Guid("45678901-90AB-CDEF-1234-567890ABCDEF")]
    [ClassInterface(ClassInterfaceType.None)]
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

        public int DistanceTo(I3DPoint other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            double dz = Z - other.Z;

            double distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            return (int)Math.Round(distance);
        }

        public I3DPoint Translate(I3DVector vector, double scale)
        {
            return new Point3D(X + vector.X * scale, Y + vector.Y * scale, Z + vector.Z * scale);
        }

        public void ReadData(string data)
        {
            var points = data.Split(';');
            X = double.Parse(points[0]);
            Y = double.Parse(points[1]);
            Z = double.Parse(points[2]);
        }

        public override string ToString()
        {
            return $"({X};{Y};{Z})";
        }
    }

    [ComVisible(true)]
    [Guid("56789012-90AB-CDEF-1234-567890ABCDEF")]
    [ClassInterface(ClassInterfaceType.None)]
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
            return $"({X};{Y};{Z})";
        }
    }
}
