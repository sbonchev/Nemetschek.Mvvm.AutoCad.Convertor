﻿using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Ribbon;
using System.Runtime.InteropServices;
using AdWin = Autodesk.Windows;
using Nemetschek.AutoCad.LayersConvertor.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;

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

        private static ImageSource ImageSourceForBitmap(Bitmap bmp)
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
                Title = "Custom Menu",
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


        private static void AddPanelEntity(AdWin.RibbonTab ribbonTab, string title, string text, string prmName, ICommand command, ImageSource imgSource)
        {
            var ribbonPanelSource = new AdWin.RibbonPanelSource { Title = title };
            var ribbonPanel = new AdWin.RibbonPanel { Source = ribbonPanelSource };
            ribbonTab.Panels.Add(ribbonPanel);
            var tt = new AdWin.RibbonToolTip { Image = imgSource, Title = text };
            var ribbonButtonAddLine = new AdWin.RibbonButton
            {
                Text = text,
                ShowText = true,
                Image = imgSource, 
                LargeImage = imgSource, 
                ShowImage = true,
                Size = AdWin.RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                ResizeStyle = AdWin.RibbonItemResizeStyles.HideText,
                ToolTip = tt,
                CommandParameter = prmName,
                CommandHandler = command
            };
            ribbonPanelSource.Items.Add(ribbonButtonAddLine);
        }

        private static void AddPanelGetEntity(AdWin.RibbonTab ribbonTab)
        {
            AddPanelEntity(ribbonTab, "Entity", "Get Entity", "GetEntity ",
                new RelayRibbonCommand((_) => AutoCadObjectPrimitives.GetCustomEntity(), (_) => true),
                ImageSourceForBitmap(Resources.cad_select_32)
            );
        }

        private static void AddPanelEntityLine(AdWin.RibbonTab ribbonTab)
        {
            AddPanelEntity(ribbonTab, "Line", "Add Line", "AddLine ", 
                new RelayRibbonCommand((_) => AutoCadObjectPrimitives.DrawLine(), (_) => true),
                ImageSourceForBitmap(Resources.lines_png_32)
             );
        }

        private static void AddPanelEntityCircle(AdWin.RibbonTab ribbonTab)
        {
            AddPanelEntity(ribbonTab, "Circles", "Add Circles", "AddCircles ", 
                new RelayRibbonCommand((_) => AutoCadObjectPrimitives.DrawCircles(), (_) => true),
                ImageSourceForBitmap(Resources.cad_circle_32)
            );
        }

        private static void AddPanelEntityConvertor(AdWin.RibbonTab ribbonTab)
        {
            AddPanelEntity(ribbonTab, "Convertors", "L-Convertor", "LCU ", 
                new RelayRibbonCommand((_) => new LayerService().UpdateLayer(), (_) => true),
                ImageSourceForBitmap(Resources.cad_process2_32)
            );
        }

        #endregion


    }

}
