using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Ribbon;
using System.Runtime.InteropServices;
using AdWin = Autodesk.Windows;
using Nemetschek.AutoCad.LayersConvertor.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;


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

        /// <summary>
        /// Avoid unmanage leak
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        private static System.Windows.Media.ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            IntPtr handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally 
            { 
                DeleteObject(handle); 
            }
        }

        private const string myTabId = "Nemetschek Tool";

        [CommandMethod("AddRibbon")]
        public void TAddRibbon()
        {
            var ribbonControl = RibbonServices.RibbonPaletteSet.RibbonControl;
            var ribbonTab = new AdWin.RibbonTab
            {
                Title = "Custom Ribbon",
                Id = myTabId
            };
            ribbonControl.Tabs.Add(ribbonTab);

            AddPanelGetEntity(ribbonTab);
            AddPanelEntityLine(ribbonTab);
            AddPanelEntityCircle(ribbonTab);
            AddPanelEntityConvertor(ribbonTab);

            ribbonTab.IsActive = true;
        }

        #region Add Panel Items

        private static void AddPanelGetEntity(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource
            {
                Title = "Select Entity"
            };
            var ribbonPanel = new AdWin.RibbonPanel
            {
                Source = ribbonPanelSource
            };
            ribbonTab.Panels.Add(ribbonPanel);

            var ribbonButtonGetEntity = new AdWin.RibbonButton
            {
                Text = "Get Custom Entity",
                ShowText = true,
                //Image = ImageSourceForBitmap(Resource.smiley_16x16_png),
                //LargeImage = ImageSourceForBitmap(Resource.smiley_32x32_png),
                ShowImage = true,
                Size = AdWin.RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                CommandParameter = "GetEntity ",
                CommandHandler = new RelayRibbonCommand((_) => AutoCadObjectPrimitives.GetCustomEntity(), (_) => true)
            };
            ribbonPanelSource.Items.Add(ribbonButtonGetEntity);
        }

        private static void AddPanelEntityLine(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource {  Title = "Line" };
            var ribbonPanel = new AdWin.RibbonPanel { Source = ribbonPanelSource };
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

        private static void AddPanelEntityCircle(AdWin.RibbonTab ribbonTab)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource
            {
                Title = "Circles"
            };
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

        private static void AddPanelEntityConvertor(AdWin.RibbonTab ribbonTab)
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
