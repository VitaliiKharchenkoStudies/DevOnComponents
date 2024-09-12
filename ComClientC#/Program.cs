using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vector3DServerLib;

namespace ComClientC_
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створення COM-об'єктів
            I3DPoint point = new Point();
            I3DVector vector = new Vector();

            // Зчитуємо дані з консолі (в якості прикладу: "1 2 3" для точки і "4 5 6" для вектора)
            Console.WriteLine("Enter point coordinates (x y z): ");
            string pointInput = Console.ReadLine();
            var pointCoords = pointInput.Split(' ');
            point.SetCoordinates(double.Parse(pointCoords[0]), double.Parse(pointCoords[1]), double.Parse(pointCoords[2]));

            Console.WriteLine("Enter vector coordinates (x y z): ");
            string vectorInput = Console.ReadLine();
            vector.FromString(vectorInput);

            // Отримуємо одиничний вектор
            vector.GetUnitVector(out double unitX, out double unitY, out double unitZ);
            Console.WriteLine($"Unit vector: ({unitX}, {unitY}, {unitZ})");

            // Зміщення точки на дві довжини вектора
            vector.GetVector(out double vecX, out double vecY, out double vecZ);
            point.GetCoordinates(out double pointX, out double pointY, out double pointZ);

            double newPointX = pointX + 2 * vecX;
            double newPointY = pointY + 2 * vecY;
            double newPointZ = pointZ + 2 * vecZ;

            Console.WriteLine($"New point: ({newPointX}, {newPointY}, {newPointZ})");
        }
    }

}
