using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WorkWithThreadsWinApi
{
    internal class PlotDataFormer
    {
        private List<List<int>> _values = new List<List<int>>();

        public PlotModel GetPlotModel(List<ThreadInfo> threadInfos)
        {
            ReverseList(threadInfos);

            var plotModel = new PlotModel();

            if (!threadInfos.Any())
                return plotModel;

            var times = new List<BarItem>();
            var ids = new List<string>();

            foreach (var threadInfo in threadInfos)
            {
                times.Add(new BarItem(threadInfo.ProcUsage));
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

        public void RefreshPlotModel(List<ThreadInfo> threadInfos, PlotModel threadPlotModel)
        {
            ReverseList(threadInfos);

            threadPlotModel.Series.Clear();
            threadPlotModel.Axes.Clear();

            if (!threadInfos.Any())
            {
                threadPlotModel.InvalidatePlot(true);
                return;
            }

            var times = new List<BarItem>();
            var ids = new List<string>();

            foreach (var threadInfo in threadInfos)
            {
                times.Add(new BarItem(threadInfo.ProcUsage));
                ids.Add(threadInfo.Id.ToString());
            }

            threadPlotModel.Series.Add(new BarSeries { ItemsSource = times });

            threadPlotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = ids
            });
            threadPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 100
            });

            threadPlotModel.InvalidatePlot(true);
        }

        // ReSharper disable once UnusedMember.Local
        private void CutLastValues()
        {
            foreach (var graph in _values)
            {
                if (graph.Count <= 25)
                    continue;

                graph.RemoveAt(0);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void AddToGraph(IReadOnlyList<ThreadInfo> threadInfos)
        {
            for (var i = 0; i < threadInfos.Count; i++)
            {
                if (i < _values.Count)
                {
                    _values[i].Add(threadInfos[i].ProcUsage);
                    continue;
                }

                if (i >= _values.Count)
                {
                    var temp = new List<int>();
                    _values.Add(temp);

                    _values[i].Add(threadInfos[i].ProcUsage);
                }
            }
        }

        private static void ReverseList(List<ThreadInfo> threadInfos)
        {
            threadInfos.Reverse();
        }
    }
}
