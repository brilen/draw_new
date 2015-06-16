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
        CLine _lineNew;
        MyThumb _startThumb;
        MyThumb _startThumbSave;
        MyThumb _finishThumb;
        CShape _shapeNew;
        List<CLine> _listLine = new List<CLine>();
        List<CShape> _listShape = new List<CShape>();
        MyThumb _movingThumb;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void but_clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _listLine.Clear();
            _listShape.Clear();
            _startThumb = null;
            _finishThumb = null;
            _shapeNew = null;
            _movingThumb = null;
            _lineNew = null;
        }


        private CShape NewShape(bool type)
        {
            if (type)
            {
                return new CShapeFull();
            }
            else
            {
                return new CShapeDot();
            }
        }

        private CLine NewLine(bool type)
        {
            if (type)
            {
                return new CLineFull();
            }
            else
            {
                return new CLineDot();
            }
        }

        private void SetMoving(bool isMoving)
        {
            if (_listShape.Count > 1)
            {
                foreach (CShape _object in _listShape)
                {
                    _object._isMoving = isMoving;
                }
            }
        }

        private void MainWindowPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (_isDrawingShape == true)
            {
                bool type = (bool)rb_full.IsChecked;
                _shapeNew = NewShape(type);
                _shapeNew._isMoving = true;
                _shapeNew.StartPoint = e.GetPosition(this);
                _shapeNew.Draw(this.canvas);

                Mouse.OverrideCursor = null;
            }
            else if (_isDrawingLine == true && _lineNew == null)
            {
                _startThumb = e.Source as MyThumb;
                if (_startThumb != null)
                {
                    _finishThumb = null;
                    _lineNew = null;
                    bool type = (bool)rb_full.IsChecked;
                    _lineNew = NewLine(type);

                    SetMoving(false);

                    _lineNew.StartPoint = new Point(Canvas.GetLeft(_startThumb) + _startThumb.ActualWidth / 2, Canvas.GetTop(_startThumb) + _startThumb.ActualHeight / 2);

                    _lineNew.EndPoint = _lineNew.StartPoint;
                    _lineNew.Draw(this.canvas);

                    _startThumbSave = _startThumb;
                                    }
            }
            if (_isDrawingLine == false && _isDrawingShape == false)
            {
                _movingThumb = e.Source as MyThumb;
            }

        }


        private void but_draw_rectangle_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeAll;
            _isDrawingShape = true;
            _isDrawingLine = false;
            
            
        }

        private void but_draw_arrow_Click(object sender, RoutedEventArgs e)
        {
            if (_listShape.Count() > 1)
            {
                Mouse.OverrideCursor = Cursors.Pen;
                _isDrawingLine = true;
                _isDrawingShape = false;
            }
        }

        private void MainWindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingLine)
            {
                _finishThumb = e.Source as MyThumb;
                if (_startThumbSave != null && _finishThumb != null && _lineNew != null && _startThumbSave != _finishThumb)
                {
                    _lineNew.EndPoint = new Point(Canvas.GetLeft(_finishThumb) + _finishThumb.ActualWidth / 2, Canvas.GetTop(_finishThumb) + _finishThumb.ActualHeight / 2);

                    _lineNew.Draw(this.canvas);

                    _startThumbSave.StartShape.Add(_lineNew);
                    _finishThumb.EndShape.Add(_lineNew);

                    _listLine.Add(_lineNew);

                    Mouse.OverrideCursor = null;
                    _isDrawingLine = false;

                    SetMoving(true);

                    _startThumbSave = null;
                    _startThumb = null;
                    _lineNew = null;
                }

                else if (_lineNew != null)
                {
                    canvas.Children.Remove(_lineNew);
                }
            }
            else if (_isDrawingShape)
            {
                _listShape.Add(_shapeNew);
                _isDrawingShape = false;
            }
        }

        private void MainWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == true && _lineNew != null && _finishThumb == null)
            {
                _lineNew.EndPoint = e.GetPosition(canvas);
                _lineNew.Draw(this.canvas);

            }

            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == false && _isDrawingShape == false)
            {
                //перерисовываю все линии, связанные с данной фигурой
                if (_movingThumb != null && _listLine.Count > 0)
                {
                    foreach (CLine line in _movingThumb.StartShape)
                    {
                        line.StartPoint = new Point(Canvas.GetLeft(_movingThumb) + _movingThumb.ActualWidth / 2, Canvas.GetTop(_movingThumb) + _movingThumb.ActualHeight / 2);
                        line.Draw(this.canvas);
                    }
                    foreach (CLine line in _movingThumb.EndShape)
                    {
                        line.EndPoint = new Point(Canvas.GetLeft(_movingThumb) + _movingThumb.ActualWidth / 2, Canvas.GetTop(_movingThumb) + _movingThumb.ActualHeight / 2);
                        line.Draw(this.canvas);
                    }
                }
            }
        }
    }
}
