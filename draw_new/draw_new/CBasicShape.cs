using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace draw_new
{
    public abstract class CBasicShape : FrameworkElement
    {
        
        protected DoubleCollection _typeLine;
        public Canvas _myCanvas;
        public bool _isMoving = false;
        
        protected CBasicShape()
        {}

        public Canvas MyCanvas
        {
            get { return _myCanvas; }
            set
            {
                _myCanvas = value;
            }
        }



        public DoubleCollection TypeLine
        {
            get { return _typeLine; }
            set
            {
                _typeLine = value;
            }
        }


        protected void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = e.Source as MyThumb;
            if (_isMoving)
            {
                var left = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var top = Canvas.GetTop(thumb) + e.VerticalChange;

                Canvas.SetLeft(thumb, left);
                Canvas.SetTop(thumb, top);
            }
        }
        protected Point _startPoint;
        protected Point _endPoint;

        public Point StartPoint
        {
            get { return _startPoint; }
            set
            {
                _startPoint = value;
                _isMoving = false;
            }
        }


        public Point EndPoint
        {
            get { return _endPoint; }
            set
            {
                _endPoint = value;
                _isMoving = true;
            }
        }
    }
}
