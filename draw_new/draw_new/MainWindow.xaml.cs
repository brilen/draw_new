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
        List<CLine> _allLine = new List<CLine>();
        List<MyThumb> _startShape = new List<MyThumb>();
        List<MyThumb> _endShape = new List<MyThumb>();
        List<CShape> _listShape = new List<CShape>();
        MyThumb _movingThumb;
        int count = 0;

        public MainWindow()
        {
            InitializeComponent();
            PreviewMouseLeftButtonDown += MainWindowPreviewMouseLeftButtonDown;
        }


        private void but_clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            count = 0;
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
                if (_startThumb != null)
                {
                    count++;
                    _startThumb._id = count;
                    _startShape.Add(_startThumb);
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

                    //_LineNew.StartPoint = (_startThumb != null) ? new Point(Canvas.GetLeft(_startThumb), Canvas.GetTop(_startThumb)) : _currentPosition;
                    _LineNew.StartPoint = new Point(Canvas.GetLeft(_startThumb), Canvas.GetTop(_startThumb));

                    _LineNew.EndPoint = _LineNew.StartPoint;
                    _LineNew.Draw();

                    _startShape.Add(_startThumb);
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
            Mouse.OverrideCursor = Cursors.Pen;
            _isDrawingLine = true;
            _isDrawingShape = false;
        }
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingLine)
            {
                _finishThumb = e.Source as MyThumb;
                if (_finishThumb != null)
                {
                    count++;
                    _finishThumb._id = count;
                    _endShape.Add(_finishThumb);
                    _LineNew.EndPoint = new Point(Canvas.GetLeft(_finishThumb), Canvas.GetTop(_finishThumb));
                    _LineNew.Draw();
                    _allLine.Add(_LineNew);
                    _endShape.Add(_finishThumb);
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
                txt_test_.Text = _LineNew.StartPoint.ToString();
                _LineNew.Draw();

            }
            if (_LineNew != null && ( _finishThumb == null || _startThumb == null))
            {
                canvas.Children.Remove(_LineNew);

            }
            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == false && _isDrawingShape == false)
            {

                if (_movingThumb != null && _movingThumb._id != -1)
                {
                    foreach (MyThumb _object in _endShape)
                    {
                        if (_movingThumb._id == _object._id)
                        {
                            foreach (CLine _line in _allLine)
                            {
                                _line.EndPoint = new Point(Canvas.GetLeft(_movingThumb), Canvas.GetTop(_movingThumb));
                                _line.Draw();
                            }
                        }
                        
                    }
                    foreach (MyThumb _object in _startShape)
                    {
                        if (_movingThumb._id == _object._id)
                        {
                            foreach (CLine _line in _allLine)
                            {
                                _line.StartPoint = new Point(Canvas.GetLeft(_movingThumb), Canvas.GetTop(_movingThumb));
                                _line.Draw();
                            }
                        }

                    }
                }
            }
        }

    }
}
