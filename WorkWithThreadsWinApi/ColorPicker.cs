using System.Collections.Generic;
using OxyPlot;

namespace WorkWithThreadsWinApi
{
    public class ColorPicker
    {
        private int _index = 7;

        private readonly List<OxyColor> _colors =
            new List<OxyColor> { 
                OxyColors.Blue, 
                OxyColors.Chocolate, 
                OxyColors.Gold, 
                OxyColors.HotPink, 
                OxyColors.Red, 
                OxyColors.MediumPurple, 
                OxyColors.Gray 
            };

        public OxyColor GetNextColor()
        {
            var nextColorIndex = _index % 7;
            _index++;
            return _colors[nextColorIndex];
        }
    }
}
