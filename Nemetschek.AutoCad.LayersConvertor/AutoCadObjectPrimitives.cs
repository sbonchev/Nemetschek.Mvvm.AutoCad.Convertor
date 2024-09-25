using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using AppDoc = Autodesk.AutoCAD.ApplicationServices.Application;


namespace Nemetschek.AutoCad.LayersConvertor
{
    /// <summary>
    /// Test AutoCad Drawing Primitives Command Methods
    /// </summary>
    public class AutoCadObjectPrimitives
    {
        /// <summary>
        /// Draw a custom line test.
        /// </summary>
        [CommandMethod("AddLine")]
        public static void DrawLine()
        {
            Document doc = AppDoc.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            var line = new Line
            {
                StartPoint = new Point3d(10, 20, 0),
                ColorIndex = (int)PrimitiveColors.Blue,
                EndPoint = new Point3d(20, 20, 0)
            };
            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction acTrans = db.TransactionManager.StartTransaction())
                    {
                        var bt = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        var btr = acTrans.GetObject(bt![BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        btr?.AppendEntity(line);
                        acTrans.AddNewlyCreatedDBObject(line, true);

                        acTrans.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                doc.Editor.WriteMessage($"Draw Line Error (status - {ex.ErrorStatus}): {ex.Message} ");
            }
        }

        /// <summary>
        /// Draw different circles
        /// </summary>
        [CommandMethod("AddCircles")]
        public static void DrawCircles()
        {
            Document doc = AppDoc.DocumentManager.MdiActiveDocument; // --- Get the current document and database
            Database db = doc.Database;
            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        // ---Open the BlockTable for read:
                        var bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        // ---Open the Block Table record Modelspace for write:
                        var btr = trans.GetObject(bt![BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        using (var circle1 = CreateCircle(new Point3d(0, 0, 0),  radius:2, PrimitiveColors.Green, ref btr, trans))
                        using (var circle3 = CreateCircle(new Point3d(30, 30, 0), radius:3, PrimitiveColors.Blue, ref btr, trans))
                        // --- Create a new circle by copy:
                        using (var circle2 = CreateCircle(new Point3d(10, 0, 10), radius:1, PrimitiveColors.Green, ref btr, trans, circle1.Clone() as Circle))
                        {
                            // ---Create a circle objects' collection:
                            var col = new DBObjectCollection { circle1,
                                                               circle2,
                                                               circle3 };
                        }
                        trans.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                doc.Editor.WriteMessage($"Exception occured: {ex.Message}, \n stack trace: {ex.StackTrace}");
                //trans.Abort(); -- no need
            }
        }

        private static Circle CreateCircle(Point3d center, double radius, PrimitiveColors color, 
                                    ref BlockTableRecord? btr, Transaction trans, Circle? circleClone = null)
        {
            var circle = circleClone != null ? circleClone 
                                             : new Circle {Center = center,
                                                           Radius = radius, 
                                                           ColorIndex = (int)color };
            // ---Add the new object to the BlockTable record
            btr?.AppendEntity(circle);
            trans.AddNewlyCreatedDBObject(circle, true);

            return circle;
        }

        /// <summary>
        /// Select entity.
        /// </summary>
        [CommandMethod("GetEntity")]
        public static void GetCustomEntity()
        {
            Document doc = AppDoc.DocumentManager.MdiActiveDocument;
            Editor AcEd = doc.Editor;
            PromptEntityOptions pEntOpts = new("\nSelect entity: ")
            {
                AllowNone = true
            };
            PromptEntityResult pEntRes = AcEd.GetEntity(pEntOpts);

            if (pEntRes.Status == PromptStatus.OK)
            {
                AcEd.WriteMessage("\nEnity selected is: " + pEntRes.ObjectId.ObjectClass.DxfName + ".");
            }
            else
            {
                AcEd.WriteMessage("\nThere is no entity selected.");
            }
        }
    }

    internal enum PrimitiveColors
    {
        Red = 1,
        Green = 3,
        Blue = 5
    }

}
