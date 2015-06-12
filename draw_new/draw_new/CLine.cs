using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace draw_new
{
    class CLine : CBasicShape
    {
        public Line _myLine;
        protected Point _endPoint;        
                
        public CLine()
        {
            _myLine = new Line();

        }

        public override void Draw()
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

        public Point EndPoint
        {
            get { return _endPoint; }
            set
            {
                _endPoint = value;
            }
        }
    }
}
