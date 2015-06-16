using System.Windows.Media;

namespace draw_new
{
    class CShapeDot : CShape
    {
        public CShapeDot(){
        _color = Brushes.MediumSeaGreen;
        TypeLine = new DoubleCollection() { 1, 2 };
    }
    }
}
