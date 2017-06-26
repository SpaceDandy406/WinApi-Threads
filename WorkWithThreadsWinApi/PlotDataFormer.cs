using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WorkWithThreadsWinApi
{
    internal class PlotDataFormer
    {
        public PlotModel GetPlotModel(List<ThreadInfo> threadInfos)
        {
            var plotModel = new PlotModel();

            if (!threadInfos.Any())
                return plotModel;

            var times = new List<BarItem>();
            var ids = new List<string>();

            foreach (var threadInfo in threadInfos)
            {
                times.Add(new BarItem(threadInfo.ThreadTime.TotalMilliseconds));
                ids.Add(threadInfo.Id.ToString());
            }

            plotModel.Series.Add(new BarSeries { ItemsSource = times });

            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = ids
            });

            return plotModel;
        }
    }
}
