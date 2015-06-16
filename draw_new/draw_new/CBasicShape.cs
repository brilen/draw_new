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
        
        protected DoubleCollection TypeLine;

        public Point StartPoint { get; set; }

        public abstract void Draw(Canvas myCanvas);
    }
}
