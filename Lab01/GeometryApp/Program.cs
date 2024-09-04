namespace GeometryApp
{
    public interface ISerializable
    {
        void ReadData(string data);
    }

    public interface IPoint : ISerializable
    {
        double X { get; set; }
        double Y { get; set; }

        int DistanceTo(IPoint other);
    }

    public interface ICircumference : ISerializable
    {
        IPoint Center { get; set; }
        double Radius { get; set; }

        bool Contains(IPoint point);
    }

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

    public class Circle : ICircumference
    {
        public IPoint Center { get; set; } = null!;
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

    class Program
    {
        static void Main(string[] args)
        {
            IPoint firstPoint = new Point();
            IPoint secondPoint = new Point();
            IPoint thirdPoint = new Point();

            ICircumference initialCircle = new Circle();
            ProcessData(firstPoint, secondPoint, thirdPoint, initialCircle);

            // Find all points that's inside the circle
            var pointsInsideCircle = new IPoint[] { firstPoint, secondPoint, thirdPoint }.Where(initialCircle.Contains).ToList();
            double newRadius = firstPoint.DistanceTo(secondPoint);
            var secondCircle = new Circle(firstPoint, newRadius);

            Console.WriteLine($"All points that's inside the initial circle: {string.Join(",", pointsInsideCircle)}, information about the circle whose center coincides with the first point entered and the circle itself passes through the second point entered: {secondCircle}");
        }

        private static void ProcessData(IPoint firstPoint, IPoint secondPoint, IPoint thirdPoint, ICircumference circle)
        {
            Console.WriteLine("Enter data for 3 points and 1 circle (format: x1;y1 x2;y2 x3;y3 x4;y4;r):");
            var data = Console.ReadLine();

            var splittedData = data!.Split(' ');

            firstPoint.ReadData(splittedData[0]);
            secondPoint.ReadData(splittedData[1]);
            thirdPoint.ReadData(splittedData[2]);

            circle.ReadData(splittedData[3]);
        }
    }
}