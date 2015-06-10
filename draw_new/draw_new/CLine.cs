using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace draw_new
{
    class CLine : CBasicShape
    {
        public Line _myLine;
                
        public CLine()
        {
            _myLine = new Line();
            _myLine.Stroke = Brushes.Black;
            _myLine.StrokeThickness = 2;
        }

        public void Draw()
        {
            _myLine.StrokeDashArray = new DoubleCollection(_typeLine);
            _myCanvas.Children.Remove(_myLine);
            _myLine.X1 = _startPoint.X;
            _myLine.Y1 = _startPoint.Y;

            _myLine.X2 = _endPoint.X;
            _myLine.Y2 = _endPoint.Y;

            _myLine.Stroke = Brushes.Black;
            _myLine.StrokeThickness = 2;

            _myCanvas.Children.Add(_myLine);

        }

        
    }
}
