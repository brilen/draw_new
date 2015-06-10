using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace draw_new
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isDrawingShape;
        private bool _isDrawingLine;
        CLine _LineNew;
        MyThumb _startThumb;
        MyThumb _finishThumb;
        CShape _objectNew;
        private Point _currentPosition;
        private Point _startPointLine;
        //List<CLine> _startLine = new List<CLine>();
        //List<CLine> _endLine = new List<CLine>();
        List<CShape> _listShape = new List<CShape>();

        public MainWindow()
        {
            InitializeComponent();
            PreviewMouseLeftButtonDown += MainWindowPreviewMouseLeftButtonDown;
        }


        private void but_clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
   
        private void MainWindowPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingShape == true)
            {
                
                 _objectNew = new CShape();
                _objectNew._isMoving = true;
                _objectNew.MyCanvas = this.canvas;
                if (rb_full.IsChecked == true)
                {
                    _objectNew.Color = Brushes.Coral;
                    _objectNew.TypeLine = new DoubleCollection() { 1,0 };
                }
                else
                {
                    _objectNew.Color = Brushes.MediumSeaGreen;
                    _objectNew.TypeLine = new DoubleCollection() { 1, 2 };
                }

                _objectNew.Draw(e.GetPosition(this));
                Mouse.OverrideCursor = null;
            }
            else if (_isDrawingLine == true)
            {
                _startThumb = e.Source as MyThumb;
                _currentPosition = e.GetPosition(canvas);
                //Panel.SetZIndex(_startThumb, 2);
                _LineNew = new CLine();
                if (_listShape != null)
                {
                    foreach (CShape _object in _listShape)
                    {
                        _object._isMoving = false;
                    }
                }
                _LineNew.MyCanvas = this.canvas;
                if (rb_full.IsChecked == true)
                {
                    _LineNew.TypeLine = new DoubleCollection() { 1, 0 };
                }
                else
                {
                    _LineNew.TypeLine = new DoubleCollection() { 1, 2 };
                }

                _LineNew.StartPoint = (_startThumb != null) ? new Point(Canvas.GetLeft(_startThumb), Canvas.GetTop(_startThumb)) : _currentPosition;
                _startPointLine = _LineNew.StartPoint;
                txt_test_.Text = _LineNew.StartPoint.ToString();

                _LineNew.EndPoint = _LineNew.StartPoint;
                _LineNew.Draw();
                //if (_startThumb != null) { _startThumb.StartLines.Add(_LineNew); }

            }

        }

        private void but_rec_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeAll;
            _isDrawingShape = true;
            _isDrawingLine = false;
            
        }

        private void but_arrow_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Pen;
            _isDrawingLine = true;
            _isDrawingShape = false;
        }
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingLine)
            {
            _finishThumb = e.Source as MyThumb;
            _LineNew.EndPoint = (_finishThumb != null) ? new Point(Canvas.GetLeft(_finishThumb), Canvas.GetTop(_finishThumb)) : e.GetPosition(canvas);
            //if (_finishThumb != null && _LineNew != null)
            //{
            //    _finishThumb.EndLines.Add(_LineNew);
            //}
            Mouse.OverrideCursor = null;
            _isDrawingLine = false;
            if (_listShape != null)
            {
                foreach (CShape _object in _listShape)
                {
                    _object._isMoving = true;
                }
            }
            }
            else if (_isDrawingShape) {
                _listShape.Add(_objectNew);
                _isDrawingShape = false;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == true)
            {
                _LineNew.StartPoint = _startPointLine;
                _LineNew.EndPoint = e.GetPosition(canvas);
                txt_test_.Text = _LineNew.EndPoint.ToString();
                _LineNew.Draw();

            }
        }

    }
}
