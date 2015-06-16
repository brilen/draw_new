using System.Windows.Media;

namespace draw_new
{
    class CLineFull: CLine
    {
        public CLineFull()
        {
            TypeLine = new DoubleCollection() { 1, 0 };
        }
    }
}
