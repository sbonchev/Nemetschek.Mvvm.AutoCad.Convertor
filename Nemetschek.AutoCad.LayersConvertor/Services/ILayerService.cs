using Nemetschek.AutoCad.LayersConvertor.Models;

namespace Nemetschek.AutoCad.LayersConvertor.Services
{
    public interface ILayerService
    {
        /// <summary>
        /// Star App - CommandMethod Invocation.
        /// </summary>
        void UpdateLayer();

        /// <summary>
        /// Conver one layer to selected another one.
        /// </summary>
        /// <param name="dwgPath">Drawing path</param>
        /// <param name="oldLayer">From layer 1</param>
        /// <param name="newLayer">To Layer 2</param>
        InfoModel ProcessLayer(string dwgPath, string oldLayer, string newLayer);

        /// <summary>
        /// Get all available doc's layers
        /// </summary>
        /// <param name="dwgPath">Drawing file path.</param>
        /// <returns></returns>
        List<string> GetlayerList(string dwgPath);
    }
}