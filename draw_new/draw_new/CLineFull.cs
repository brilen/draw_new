using System.Windows.Media;

namespace draw_new
{
    class CLineFull: CLine
    {
        public CLineFull()
        {
            _typeLine = new DoubleCollection() { 1, 0 };
        }
    }
}
