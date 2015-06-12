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
    public abstract class CBasicShape
    {
        
        protected DoubleCollection _typeLine;
        public Canvas _myCanvas;
                
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


        protected Point _startPoint;
        

        public Point StartPoint
        {
            get { return _startPoint; }
            set
            {
                _startPoint = value;
            }
        }
    

        public abstract void Draw();
    }
}
