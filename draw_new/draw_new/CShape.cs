using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace draw_new
{
    class CShape : CBasicShape
    {
        protected Brush _color;
        public Rectangle _baseShape;
        private MyThumb _newThumb;
        

        public CShape()
        {
            _baseShape = new Rectangle();
            _newThumb = new MyThumb();
        }

        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
            }
        }

        public void Draw(Point position)
        {
            _newThumb.Template = ((MainWindow)System.Windows.Application.Current.MainWindow).Resources["template1"] as ControlTemplate;
            _newThumb.ApplyTemplate();
            _newThumb.DragDelta += OnDragDelta;
            _myCanvas.Children.Add(_newThumb);

            _baseShape = (Rectangle)_newThumb.Template.FindName("tplRectangle", _newThumb);
            _baseShape.Stroke = Brushes.Black;
            _baseShape.Height = 80;
            _baseShape.Width = 120;
            _baseShape.StrokeThickness = 2;
            _baseShape.Fill = _color;
            _baseShape.StrokeDashArray = new DoubleCollection(_typeLine);

            Canvas.SetLeft(_newThumb, position.X);
            Canvas.SetTop(_newThumb, position.Y);

            _newThumb.UpdateLayout();
        }
        
   }
}
