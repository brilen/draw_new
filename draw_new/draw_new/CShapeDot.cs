using System.Windows.Media;

namespace draw_new
{
    class CShapeDot : CShape
    {
        public CShapeDot()
        {
            Сolor = Brushes.MediumSeaGreen;
            TypeLine = new DoubleCollection() { 1, 2 };
        }
    }
}
