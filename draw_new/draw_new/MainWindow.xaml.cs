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
        MyThumb _startThumbSave;
        MyThumb _finishThumb;
        CShape _objectNew;
        List<CLine> _allLine = new List<CLine>();
        List<CShape> _listShape = new List<CShape>();
        MyThumb _movingThumb;

        public MainWindow()
        {
            InitializeComponent();
            PreviewMouseLeftButtonDown += MainWindowPreviewMouseLeftButtonDown;
        }


        private void but_clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _allLine.Clear();
            _listShape.Clear();
            _startThumb = null;
            _finishThumb = null;
            _objectNew = null;
            _movingThumb = null;
            _LineNew = null;
        }
   
        private void MainWindowPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingShape == true)
            {
                if (rb_full.IsChecked == true)
                {
                    _objectNew = new CShapeFull();
                }
                else
                {
                    _objectNew = new CShapeDot();
                }

                _objectNew._isMoving = true;
                _objectNew.MyCanvas = this.canvas;
                _objectNew.StartPoint = e.GetPosition(this);
                _objectNew.Draw();
               
                Mouse.OverrideCursor = null;
            }
            else if (_isDrawingLine == true)
            {
                _startThumb = e.Source as MyThumb;
                if (_startThumb != null)
                {
                    _LineNew = null;
                    if (rb_full.IsChecked == true)
                    {
                        _LineNew = new CLineFull();
                    }
                    else
                    {
                        _LineNew = new CLineDot();
                    }
                    if (_listShape != null)
                    {
                        foreach (CShape _object in _listShape)
                        {
                            _object._isMoving = false;
                        }
                    }
                    _LineNew.MyCanvas = this.canvas;

                    var x = Canvas.GetLeft(_startThumb) + _startThumb.ActualWidth / 2;
                    var y = Canvas.GetTop(_startThumb) + _startThumb.ActualHeight/2;
                    _LineNew.StartPoint = new Point(x, y);

                    _LineNew.EndPoint = _LineNew.StartPoint;
                    _LineNew.Draw();

                    _startThumbSave = _startThumb;
                }
            }
            if (_isDrawingLine == false && _isDrawingShape == false)
            {
                _movingThumb = e.Source as MyThumb;
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
            if (_listShape.Count() > 1)
            {
                Mouse.OverrideCursor = Cursors.Pen;
                _isDrawingLine = true;
                _isDrawingShape = false;
            }
        }
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingLine)
            {
                _finishThumb = e.Source as MyThumb;
                if (_finishThumb != null)
                {
                    var x = Canvas.GetLeft(_finishThumb) + _finishThumb.ActualWidth / 2;
                    var y = Canvas.GetTop(_finishThumb) + _finishThumb.ActualHeight / 2;
                    _LineNew.EndPoint = new Point(x, y);

                    _LineNew.Draw();

                    _startThumbSave.StartLines.Add(_LineNew);
                    _finishThumb.EndLines.Add(_LineNew);

                    _allLine.Add(_LineNew);

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
            }
            else if (_isDrawingShape)
            {
                _listShape.Add(_objectNew);
                _isDrawingShape = false;
            }


        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == true && _LineNew != null)
            {
                _LineNew.EndPoint = e.GetPosition(canvas);
                _LineNew.Draw();

            }

            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == false && _isDrawingShape == false)
            {
                //перерисовываю все линии, связанные с данной фигурой
                if (_movingThumb != null && _allLine.Count>0)
                {
                    foreach (CLine line in _movingThumb.StartLines)
                    {
                        var x = Canvas.GetLeft(_movingThumb) + _movingThumb.ActualWidth / 2;
                        var y = Canvas.GetTop(_movingThumb) + _movingThumb.ActualHeight / 2;
                        line.StartPoint = new Point(x, y);
                        line.Draw();
                    }
                    foreach (CLine line in _movingThumb.EndLines)
                    {
                        var x = Canvas.GetLeft(_movingThumb) + _movingThumb.ActualWidth / 2;
                        var y = Canvas.GetTop(_movingThumb) + _movingThumb.ActualHeight / 2;
                        line.EndPoint = new Point(x, y);
                        line.Draw();
                    }
                }
            }
        }

    }
}
