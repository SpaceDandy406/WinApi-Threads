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
            var colorPicker = new ColorPicker();

            ReverseList(threadInfos);

            var plotModel = new PlotModel();

            if (!threadInfos.Any())
                return plotModel;

            foreach (var threadInfo in threadInfos)
            {
                var temp = new List<int> { threadInfo.ProcUsage };
                _values.Add(temp);
            }

            //var times = new List<BarItem>();
            //var ids = new List<string>();

            //foreach (var threadInfo in threadInfos)
            //{
            //    times.Add(new BarItem(threadInfo.ProcUsage));
            //    ids.Add(threadInfo.Id.ToString());
            //}

            //plotModel.Series.Add(new BarSeries { ItemsSource = times });

            //plotModel.Axes.Add(new CategoryAxis
            //{
            //    Position = AxisPosition.Left,
            //    Key = "CakeAxis",
            //    ItemsSource = ids
            //});

            var linearSeries = new List<LineSeries>();

            for (var i = 0; i < _values.Count; i++)
            {
                var graph = _values[i];
                for (var j = 0; j < graph.Count; j++)
                {
                    linearSeries[i].Points.Add(new DataPoint(j, graph[j]));
                }
            }

            for (var i = 0; i < linearSeries.Count; i++)
            {
                linearSeries[i].Color = colorPicker.GetNextColor();
                linearSeries[i].Title = threadInfos[i].Id.ToString();
            }

            foreach (var series in linearSeries)
            {
                plotModel.Series.Add(series);
            }

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                Minimum = 0,
                AbsoluteMaximum = 100,
                AbsoluteMinimum = 0,
                Maximum = 100
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 50,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 50
            });

            return plotModel;
        }

        public void RefreshPlotModel(List<ThreadInfo> threadInfos, PlotModel threadPlotModel)
        {
            var colorPicker = new ColorPicker();

            ReverseList(threadInfos);

            threadPlotModel.Series.Clear();
            threadPlotModel.Axes.Clear();

            if (!threadInfos.Any())
                return;

            AddToGraph(threadInfos);
            CutLastValues();

            //var times = new List<BarItem>();
            //var ids = new List<string>();

            //foreach (var threadInfo in threadInfos)
            //{
            //    times.Add(new BarItem(threadInfo.ProcUsage));
            //    ids.Add(threadInfo.Id.ToString());
            //}

            //threadPlotModel.Series.Add(new BarSeries { ItemsSource = times });

            //threadPlotModel.Axes.Add(new CategoryAxis
            //{
            //    Position = AxisPosition.Left,
            //    Key = "CakeAxis",
            //    ItemsSource = ids
            //});
            //threadPlotModel.Axes.Add(new LinearAxis
            //{
            //    Position = AxisPosition.Bottom,
            //    Minimum = 0,
            //    Maximum = 100
            //});

            var linearSeries = new List<LineSeries>();

            for (var i = 0; i < _values.Count; i++)
            {
                var graph = _values[i];
                linearSeries.Add(new LineSeries());
                for (var j = 0; j < graph.Count; j++)
                {
                    linearSeries[i].Points.Add(new DataPoint(25 - j, graph[(graph.Count - 1) - j]));
                }
            }

            for (var i = 0; i < linearSeries.Count; i++)
            {
                linearSeries[i].Color = colorPicker.GetNextColor();
                linearSeries[i].Title = threadInfos[i].Id.ToString();
            }

            foreach (var series in linearSeries)
            {
                threadPlotModel.Series.Add(series);
            }

            threadPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                Minimum = 0,
                AbsoluteMaximum = 100,
                AbsoluteMinimum = 0,
                Maximum = 100
            });
            threadPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 50,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 50
            });


            threadPlotModel.InvalidatePlot(true);
        }

        private void CutLastValues()
        {
            foreach (var graph in _values)
            {
                if (graph.Count <= 25)
                    continue;

                graph.RemoveAt(0);
            }
        }

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

                    //for (int j = 0; j < _values[i].Count - 1; j++)
                    //    _values[i].Add(0);

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
