using System.Windows.Media;

namespace draw_new
{
    class CLineDot : CLine
    {
        public CLineDot()
        {
            TypeLine = new DoubleCollection() { 1, 2 };
        }
    }
}
