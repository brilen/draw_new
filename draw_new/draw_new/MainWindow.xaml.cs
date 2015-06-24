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
        private bool _isMoving;
        private Rectangle _movingRectangle;
        private CShape _startShape = new CShape();
        private CShape _finishShape = new CShape();
        private CShape _movingShape = new CShape();
        private CLine _lineNew;
        private CShape _shapeNew;
        List<CLine> _listLine = new List<CLine>();
        List<CShape> _listShape = new List<CShape>();


        public MainWindow()
        {
            InitializeComponent();
        }


        private void but_clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _listLine.Clear();
            _listShape.Clear();
            _shapeNew = null;
            _lineNew = null;
        }

        private void MainWindowPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (_isDrawingShape)
            {
                //рисование прямоугольника
                bool? type = rb_dotted.IsChecked;
                _shapeNew = NewShape(type);
                _shapeNew.StartPoint = e.GetPosition(this);
                _shapeNew.Draw();
                canvas.Children.Add(_shapeNew.Rectangle);

                Mouse.OverrideCursor = null;
            }
            else if (_isDrawingLine && _lineNew == null)
            {
                _startShape = GetShape(e.Source as Rectangle);
                if (_startShape != null)
                {
                    //начало рисования линии
                    _finishShape = null;
                    _lineNew = null;

                    bool? type = rb_dotted.IsChecked;
                    _lineNew = NewLine(type);
                    _lineNew.StartPoint = new Point(Canvas.GetLeft(_startShape.Rectangle) + _startShape.Rectangle.ActualWidth / 2, Canvas.GetTop(_startShape.Rectangle) + _startShape.Rectangle.ActualHeight / 2);
                    _lineNew.EndPoint = _lineNew.StartPoint;
                    _lineNew.Draw();
                    canvas.Children.Add(_lineNew.Line);
                }
            }
            if (_isDrawingLine == false && _isDrawingShape == false)
            {
                //получение фигуры, которую перемещают, для перерисоваки линий
                _movingShape = GetShape(e.Source as Rectangle);

                //начало перемещения прямоугольника
                _movingRectangle = e.Source as Rectangle;
                if (_movingRectangle != null)
                {
                    _isMoving = true;
                }
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
                _finishShape = GetShape(e.Source as Rectangle);

                if (_startShape != null && _finishShape != null && _lineNew != null && _startShape != _finishShape)
                {
                    //перерисовка линий
                    canvas.Children.Remove(_lineNew.Line);
                    _lineNew.EndPoint = new Point(Canvas.GetLeft(_finishShape.Rectangle) + _finishShape.Rectangle.ActualWidth / 2, Canvas.GetTop(_finishShape.Rectangle) + _finishShape.Rectangle.ActualHeight / 2);
                    _lineNew.Draw();
                    canvas.Children.Add(_lineNew.Line);

                    AddLineToShape();

                    _listLine.Add(_lineNew);

                    Mouse.OverrideCursor = null;
                    _isDrawingLine = false;

                    _startShape = null;
                    _lineNew = null;
                }
            }
            if (_isDrawingShape)
            {
                _listShape.Add(_shapeNew);
                _isDrawingShape = false;
            }
            _isMoving = false;
        }

        private void MainWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine && _lineNew != null && _finishShape == null)
            {
                //перерисовка линий
                canvas.Children.Remove(_lineNew.Line);
                _lineNew.EndPoint = e.GetPosition(canvas);
                _lineNew.Draw();
                canvas.Children.Add(_lineNew.Line);

            }

            if (_isMoving)
            {
                //задание новых координат прямоугольнику
                _movingRectangle.SetValue(Canvas.LeftProperty, e.GetPosition(canvas).X -_movingRectangle.ActualWidth / 2);
                _movingRectangle.SetValue(Canvas.TopProperty, e.GetPosition(canvas).Y - _movingRectangle.ActualHeight / 2);
            }
            if (e.LeftButton == MouseButtonState.Pressed && _isDrawingLine == false && _isDrawingShape == false)
            {
                //перерисовывка всех линии, связанных с данной прямоугольником
                if (_movingShape != null && _listLine.Count > 0)
                {
                    foreach (CLine line in _movingShape.StartShape)
                    {
                        canvas.Children.Remove(line.Line);
                        line.StartPoint = new Point(Canvas.GetLeft(_movingShape.Rectangle) + _movingShape.Rectangle.ActualWidth / 2, Canvas.GetTop(_movingShape.Rectangle) + _movingShape.Rectangle.ActualHeight / 2);
                        line.Draw();
                        canvas.Children.Add(line.Line);
                    }
                    foreach (CLine line in _movingShape.EndShape)
                    {
                        canvas.Children.Remove(line.Line);
                        line.EndPoint = new Point(Canvas.GetLeft(_movingShape.Rectangle) + _movingShape.Rectangle.ActualWidth / 2, Canvas.GetTop(_movingShape.Rectangle) + _movingShape.Rectangle.ActualHeight / 2);
                        line.Draw();
                        canvas.Children.Add(line.Line);
                    }
                }
            }
        }


        private CShape GetShape (Rectangle rectangle)
        {
            //получение фигуры
            foreach (CShape shape in _listShape){
                if (shape.Rectangle == rectangle)
                {
                    return shape;
                }
            }
            return null;
        }

        private void AddLineToShape()
        {
            //добавление линии в массивы начальной и конечной фигуры
            foreach (CShape shape in _listShape)
            {
                if (shape == _startShape)
                {
                    shape.StartShape.Add(_lineNew);
                }
                else if (shape == _finishShape) {
                    shape.EndShape.Add(_lineNew);
                }
            }
        }

        private CShape NewShape(bool? type)
        {
            if (type.Value)
            {
                return new CShapeDot();
            }
            else
            {
                return new CShapeFull();
            }
        }

        private CLine NewLine(bool? type)
        {
            if (type.Value)
            {
                return new CLineDot();
            }
            else
            {
                return new CLineFull();
            }
        }
    }
}
