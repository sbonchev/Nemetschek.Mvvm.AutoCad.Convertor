using System.Windows.Threading;

namespace Nemetschek.AutoCad.LayersConvertor
{
    internal static class ProcessDispatcher
    {
        internal static void Execute(Action act)
        {
            var dispatcher = LayerConvertorWindow.AppWindow?.Dispatcher;
            if (dispatcher is null)
                return;

            // ---Marshall to Main Thread:
            dispatcher.BeginInvoke(DispatcherPriority.Background, act);
        }
    }
}
