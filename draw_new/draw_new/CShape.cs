using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Collections.Generic;

namespace draw_new
{
    public class CShape : CBasicShape
    {
        public List<CLine> StartShape { get; private set; }
        public List<CLine> EndShape { get; private set; }
        protected Brush Сolor;
        private Rectangle _newRectangle;

        public CShape()
        {
            _newRectangle = new Rectangle();
            StartShape = new List<CLine>();
            EndShape = new List<CLine>();
        }

        public Rectangle Rectangle
        {
            get { return _newRectangle; }
            set { _newRectangle = value; }
        }

        public override void Draw()
        {
            _newRectangle.Stroke = Brushes.Black;
            _newRectangle.Height = 80;
            _newRectangle.Width = 120;
            _newRectangle.StrokeThickness = 2;
            _newRectangle.Fill = Сolor;
            _newRectangle.StrokeDashArray = new DoubleCollection(TypeLine);

            Canvas.SetLeft(_newRectangle, StartPoint.X);
            Canvas.SetTop(_newRectangle, StartPoint.Y);
        }

    }
}
