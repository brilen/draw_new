using System.Windows.Media;

namespace draw_new
{
    class CShapeDot : CShape
    {
        public CShapeDot(){
        _color = Brushes.MediumSeaGreen;
        _typeLine = new DoubleCollection() { 1, 2 };
    }
    }
}
