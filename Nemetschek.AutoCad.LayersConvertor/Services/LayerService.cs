using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Nemetschek.AutoCad.LayersConvertor.Enums;
using Nemetschek.AutoCad.LayersConvertor.Models;
using Nemetschek.AutoCad.LayersConvertor.ViewModels;
using System.Windows.Input;

namespace Nemetschek.AutoCad.LayersConvertor.Services
{
    public class LayerService : ILayerService
    {
        public LayerService()
        {
            //var services = new ServiceCollection();
            //ConfigureServices(services);
        }

        [CommandMethod("LCU")]
        public void UpdateLayer()
        {
            //var lvm = _serviceProvider.GetRequiredService<LayerViewModel>();
            var mainWindow = new LayerConvertorWindow();
            mainWindow?.ShowDialog();
        }



        /// <summary>
        /// Conver one layer to selected another one.
        /// </summary>
        /// <param name="dwgPath">Drawing path</param>
        /// <param name="oldLayer">From layer 1</param>
        /// <param name="newLayer">To Layer 2</param>
        public InfoModel ProcessLayer(string dwgPath, string oldLayer, string newLayer)
        {
            var doc = GetDocument(dwgPath);
            if (doc == null)
                return new InfoModel { Text = "Invalid drawing path!", Status = ProcessStatus.Failed };

            doc.GetDocumentWindow().Activate();
            var db = doc.Database;
            var editor = doc.Editor;
            try
            {
                using (doc.LockDocument())
                {
                    using (var trans = db.TransactionManager.StartOpenCloseTransaction())
                    {
                        var modelSpaceId = SymbolUtilityServices.GetBlockModelSpaceId(db);
                        var modelSpace = (BlockTableRecord)trans.GetObject(modelSpaceId, OpenMode.ForRead);
                        bool isOk = false;
                        foreach (ObjectId id in modelSpace)
                        {
                            var acEnt = trans.GetObject(id, OpenMode.ForWrite) as Entity;
                            if (acEnt == null)
                                continue;

                            var layerName = acEnt.Layer;
                            if (layerName == oldLayer)
                            {
                                acEnt.Layer = newLayer;
                                isOk = true;
                                break;
                            }
                        }
                        if (isOk)
                        {
                            trans.Commit();
                            db.SaveAs(dwgPath, DwgVersion.Current);
                            return new InfoModel { Text = $"Layer: {oldLayer} has been converted to {newLayer} successfuly!", Status = ProcessStatus.Succed }; ;
                        }
                        else
                            throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.CopyDoesNotExist, $"Cannot find layer: {oldLayer}");
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                editor.WriteMessage($"Processing Error (status - {ex.ErrorStatus}): {ex.Message} ");
                return new InfoModel
                {
                    Text = $"Layer {oldLayer} conversion to {newLayer} failed, \n error status - {ex.ErrorStatus})!",
                    Status = ProcessStatus.Failed
                };
            }
            finally
            {
                //doc.CloseAndDiscard();
            }
        }

        /// <summary>
        /// Get all available doc's layers
        /// </summary>
        /// <param name="dwgPath">Drawing file path.</param>
        /// <returns></returns>
        public List<string> GetlayerList(string dwgPath)
        {
            var acDoc = GetDocument(dwgPath);
            if (acDoc == null)
                acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.Open(dwgPath);

            var acDb = acDoc.Database;
            var layerList = new List<string>();
            using (var trans = acDb.TransactionManager.StartTransaction())
            {
                try
                {
                    var modelSpaceId = SymbolUtilityServices.GetBlockModelSpaceId(acDb);
                    var modelSpace = (BlockTableRecord)trans.GetObject(modelSpaceId, OpenMode.ForRead);
                    foreach (ObjectId id in modelSpace)
                    {
                        Entity? acEnt = trans.GetObject(id, OpenMode.ForRead) as Entity;
                        if (acEnt == null)
                        {
                            continue;
                        }
                        var layerName = acEnt.Layer;
                        if (!layerList.Contains(layerName))
                            layerList.Add(layerName);
                    }
                    trans.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    acDoc.Editor.WriteMessage(ex.Message);
                }

                return layerList;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<LayerViewModel>();
            //services.AddTransient<ILayerService, LayerService>();
            services.AddTransient<ICommand, RelayRibbonCommand>();
            //services.AddSingleton<LayerConvertorWindow>();
            //_serviceProvider = services.BuildServiceProvider();
        }

        private Document? GetDocument(string dwgPath)
        {
            var docCollection = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager;
            if (string.IsNullOrEmpty(dwgPath))
                return null;

            string fileName = dwgPath.Trim().ToLower();
            foreach (Document doc in docCollection)
            {
                if (doc.Name.Trim().ToLower() == fileName)
                    return doc;
            }

            return null;
        }

    }
}
