using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace draw_new
{
    class CLine : CBasicShape
    {
        public Line _myLine;
        
        public CLine()
        {
            _myLine = new Line();

        }

        public override void Draw(Canvas myCanvas)
        {
            if (myCanvas != null)
            {
                _myLine.StrokeDashArray = new DoubleCollection(TypeLine);
                myCanvas.Children.Remove(_myLine);
                _myLine.X1 = StartPoint.X;
                _myLine.Y1 = StartPoint.Y;

                _myLine.X2 = EndPoint.X;
                _myLine.Y2 = EndPoint.Y;

                _myLine.Stroke = Brushes.Black;
                _myLine.StrokeThickness = 2;

                myCanvas.Children.Add(_myLine);
            }
        }

        public Point EndPoint { get; set;}
    }
}
