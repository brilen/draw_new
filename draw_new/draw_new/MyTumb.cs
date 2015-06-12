using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace draw_new
{
  public class MyThumb : Thumb
  {
      public List<CBasicShape> StartLines { get; private set; }
      public List<CBasicShape> EndLines { get; private set; }

    public MyThumb()
    {
        StartLines = new List<CBasicShape>();
        EndLines = new List<CBasicShape>();
    }
  }
}
