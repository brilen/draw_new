using System.Windows.Media;

namespace draw_new
{
    class CLineDot : CLine
    {
        public CLineDot()
        {
            _typeLine = new DoubleCollection() { 1, 2 };
        }
    }
}
