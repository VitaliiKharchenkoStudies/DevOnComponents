using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Transactions;

[assembly: Autodesk.AutoCAD.Runtime.CommandClass(typeof(AutoCADExtension.Commands))]

namespace AutoCADExtension
{
    public class Commands
    {
        [CommandMethod("DrawLetter")]

        // Запитуємо користувача вибрати літеру (Д або Ї)
        public void DrawLetter()
        {
            Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            PromptKeywordOptions letterOptions = new PromptKeywordOptions("\nВиберіть літеру для побудови (Д або Ї): ");
            letterOptions.Keywords.Add("Д");
            letterOptions.Keywords.Add("Ї");
            letterOptions.AllowNone = false;
            PromptResult letterResult = ed.GetKeywords(letterOptions);
            if (letterResult.Status != PromptStatus.OK) return;

            PromptDoubleOptions sizeOptions = new PromptDoubleOptions("\nВведіть розмір літери: ");
            sizeOptions.AllowNegative = false;
            sizeOptions.AllowZero = false;
            PromptDoubleResult sizeResult = ed.GetDouble(sizeOptions);
            if (sizeResult.Status != PromptStatus.OK) return;
            double size = sizeResult.Value;

            using (Autodesk.AutoCAD.DatabaseServices.Transaction trans = doc.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)trans.GetObject(doc.Database.BlockTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead);
                BlockTableRecord blockTableRecord = (BlockTableRecord)trans.GetObject(doc.Database.CurrentSpaceId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForWrite);

                if (letterResult.StringResult == "Д")
                {
                    DrawLetterD(blockTableRecord, size, trans);
                }
                else if (letterResult.StringResult == "Ї")
                {
                    DrawLetterI(blockTableRecord, size, trans);
                }

                trans.Commit();
            }
        }

        // Координати для побудови контуру літери "Д"
        private void DrawLetterD(BlockTableRecord blockTableRecord, double size, Autodesk.AutoCAD.DatabaseServices.Transaction trans)
        {
            Point3d[] points = {
                new Point3d(0, 0, 0),       
                new Point3d(0, size, 0),        
                new Point3d(size / 2, size * 1.5, 0), 
                new Point3d(size, size, 0),      
                new Point3d(size, 0, 0),          
                new Point3d(0, 0, 0)             
            };

            for (int i = 0; i < points.Length - 1; i++)
            {
                Line line = new Line(points[i], points[i + 1]);
                blockTableRecord.AppendEntity(line);
                trans.AddNewlyCreatedDBObject(line, true);
            }
        }

        // Координати для побудови контуру літери "Ї"
        private void DrawLetterI(BlockTableRecord blockTableRecord, double size, Autodesk.AutoCAD.DatabaseServices.Transaction trans)
        {
            Point3d[] points = {
                new Point3d(0, 0, 0),          
                new Point3d(0, size, 0),        
                new Point3d(size, size, 0),     
                new Point3d(size, 0, 0),        
                new Point3d(0, 0, 0)      
            };

            for (int i = 0; i < points.Length - 1; i++)
            {
                Line line = new Line(points[i], points[i + 1]);
                blockTableRecord.AppendEntity(line);
                trans.AddNewlyCreatedDBObject(line, true);
            }

            Line dot1 = new Line(new Point3d(size / 4, size * 1.2, 0), new Point3d(size / 4, size * 1.3, 0));
            Line dot2 = new Line(new Point3d(3 * size / 4, size * 1.2, 0), new Point3d(3 * size / 4, size * 1.3, 0));
            blockTableRecord.AppendEntity(dot1);
            blockTableRecord.AppendEntity(dot2);
            trans.AddNewlyCreatedDBObject(dot1, true);
            trans.AddNewlyCreatedDBObject(dot2, true);
        }
    }
}
