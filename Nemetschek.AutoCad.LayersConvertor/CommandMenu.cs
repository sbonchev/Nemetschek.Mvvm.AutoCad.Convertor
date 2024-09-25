using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Ribbon;
using System.Runtime.InteropServices;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using AdWin = Autodesk.Windows;
using Nemetschek.AutoCad.LayersConvertor.Services;

namespace Nemetschek.AutoCad.LayersConvertor
{
    /// <summary>
    /// Custom AutoCad Menu Extensions.
    /// </summary>
    public class CommandMenu : IExtensionApplication
    {
        /// <summary>
        /// Init Ribbon Panel Menus.
        /// </summary>
        void IExtensionApplication.Initialize()
        {
            TAddRibbon();
        }

        public void Terminate()
        {
            throw new ApplicationException("Applivation terminal exception!");
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        //public static ImageSource ImageSourceForBitmap(Bitmap bmp)
        //{
        //    var handle = bmp.GetHbitmap();
        //    try
        //    {
        //        return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //    }
        //    finally { DeleteObject(handle); }
        //}

        private const string myTabId = "Nemetschek Tool";

        [CommandMethod("AddRibbon")]
        public void TAddRibbon()
        {
            var ribbonControl = RibbonServices.RibbonPaletteSet.RibbonControl;
            var ribbonTab = new AdWin.RibbonTab();

            ribbonTab.Title = "Custom Ribbon";
            ribbonTab.Id = myTabId;
            ribbonControl.Tabs.Add(ribbonTab);

            AddPanelGetEntity(ribbonTab);
            AddPanelEntityLine(ribbonTab);
            AddPanelEntityCircle(ribbonTab);
            AddPanelEntityConvertor(ribbonTab);

            ribbonTab.IsActive = true;
        }

        #region Add Panel Items

        private void AddPanelGetEntity(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource();
            ribbonPanelSource.Title = "Select Entity";

            var ribbonPanel = new AdWin.RibbonPanel();
            ribbonPanel.Source = ribbonPanelSource;
            ribbonTab.Panels.Add(ribbonPanel);

            var ribbonButtonGetEntity = new AdWin.RibbonButton
            {
                Text = "Get Custom Entity",
                ShowText = true,
                //ribbonButtonGetEntity.Image = ImageSourceForBitmap(Resource.smiley_16x16_png);
                //ribbonButtonGetEntity.LargeImage = ImageSourceForBitmap(Resource.smiley_32x32_png);
                ShowImage = true,
                Size = AdWin.RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                CommandParameter = "GetEntity ",
                CommandHandler = new RelayRibbonCommand((_) => AutoCadObjectPrimitives.GetCustomEntity(), (_) => true)
            };
            ribbonPanelSource.Items.Add(ribbonButtonGetEntity);
        }

        private void AddPanelEntityLine(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource();
            ribbonPanelSource.Title = "Line";

            var ribbonPanel = new AdWin.RibbonPanel
            {
                Source = ribbonPanelSource
            };
            ribbonTab.Panels.Add(ribbonPanel);

            var ribbonButtonAddLine = new AdWin.RibbonButton
            {
                Text = "Add Custom Line",
                ShowText = true,
                CommandParameter = "AddLine ",
                CommandHandler = new RelayRibbonCommand((_) => AutoCadObjectPrimitives.DrawLine(), (_) => true)
            };
            ribbonPanelSource.Items.Add(ribbonButtonAddLine);
        }

        private void AddPanelEntityCircle(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource();
            ribbonPanelSource.Title = "Circles";

            var ribbonPanel = new AdWin.RibbonPanel
            {
                Source = ribbonPanelSource
            };
            ribbonTab.Panels.Add(ribbonPanel);

            var ribbonButtonAddLine = new AdWin.RibbonButton
            {
                Text = "Add Custom Circles",
                ShowText = true,
                CommandParameter = "AddCircles ",
                CommandHandler = new RelayRibbonCommand((_) => AutoCadObjectPrimitives.DrawCircles(), (_) => true)
            };
            ribbonPanelSource.Items.Add(ribbonButtonAddLine);
        }

        private void AddPanelEntityConvertor(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource();
            ribbonPanelSource.Title = "Convertors";

            var ribbonPanel = new AdWin.RibbonPanel
            {
                Source = ribbonPanelSource
            };
            ribbonTab.Panels.Add(ribbonPanel);

            var ribbonButtonAddConvertor = new AdWin.RibbonButton
            {
                Text = "Layer Convertor",
                ShowText = true,
                CommandParameter = "LCU ",
                CommandHandler = new RelayRibbonCommand((_) => new LayerService().UpdateLayer(), (_) => true)
            };
            ribbonPanelSource.Items.Add(ribbonButtonAddConvertor);
        }

        #endregion


    }

}
