using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace draw_new
{
  public class MyThumb : Thumb
  {
      public List<CBasicShape> StartShape { get; private set; }
      public List<CBasicShape> EndShape { get; private set; }

    public MyThumb()
    {
        StartShape = new List<CBasicShape>();
        EndShape = new List<CBasicShape>();
    }
  }
}
