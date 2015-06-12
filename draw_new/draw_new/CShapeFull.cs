using System.Windows.Media;


namespace draw_new
{
    class CShapeFull : CShape
    {
        public CShapeFull(){
            _color = Brushes.Coral;
            _typeLine = new DoubleCollection() { 1, 0 };
    }
    }
}
