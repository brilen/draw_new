using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace draw_new
{
    public class CLine : CBasicShape
    {
        private Line _myLine;

        public CLine()
        {
            _myLine = new Line();

        }
        public Line Line
        {
            get { return _myLine; }
        }
        public override void Draw()
        {
            _myLine.StrokeDashArray = new DoubleCollection(TypeLine);
            _myLine.X1 = StartPoint.X;
            _myLine.Y1 = StartPoint.Y;

            _myLine.X2 = EndPoint.X;
            _myLine.Y2 = EndPoint.Y;

            _myLine.Stroke = Brushes.Black;
            _myLine.StrokeThickness = 2;
        }


    }
}
