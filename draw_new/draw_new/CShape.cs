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
        public bool _isMoving = false;
        
        public CShape()
        {
            _baseShape = new Rectangle();
            _newThumb = new MyThumb();
        }

        protected void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = e.Source as MyThumb;
            if (_isMoving && thumb != null)
            {
                var left = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var top = Canvas.GetTop(thumb) + e.VerticalChange;

                Canvas.SetLeft(thumb, left);
                Canvas.SetTop(thumb, top);
            }
        }

        public override void Draw(Canvas myCanvas)
        {
            if (myCanvas != null)
            {
                _newThumb.Template = ((MainWindow)System.Windows.Application.Current.MainWindow).Resources["template1"] as ControlTemplate;
                _newThumb.ApplyTemplate();
                _newThumb.DragDelta += OnDragDelta;
                myCanvas.Children.Add(_newThumb);

                _baseShape = (Rectangle)_newThumb.Template.FindName("tplRectangle", _newThumb);
                _baseShape.Stroke = Brushes.Black;
                _baseShape.Height = 80;
                _baseShape.Width = 120;
                _baseShape.StrokeThickness = 2;
                _baseShape.Fill = _color;
                _baseShape.StrokeDashArray = new DoubleCollection(TypeLine);

                Canvas.SetLeft(_newThumb, StartPoint.X);
                Canvas.SetTop(_newThumb, StartPoint.Y);

                //Обеспечивает правильное обновление всех визуальных дочерних элементов данного элемента
                _newThumb.UpdateLayout();
            }
        }
        
   }
}
