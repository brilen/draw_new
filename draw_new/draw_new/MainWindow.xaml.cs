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
        private EStates _state = new EStates();
        private Rectangle _movingRectangle;
        private CShape _startShape;
        private CShape _finishShape;
        private CShape _movingShape;
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
            switch (_state)
            {
                case EStates.IsDrawingShape:
                    DrawingShapeStart(e);
                    break;

                case EStates.IsDrawingLine:
                    DrawingLineStart(e);
                    break;

                case EStates.Default:
                    //получение фигуры, которую перемещают, для перерисоваки линий
                    _movingShape = GetShape(e.Source as Rectangle);

                    //начало перемещения прямоугольника
                    _movingRectangle = e.Source as Rectangle;
                    if (_movingRectangle != null)
                    {
                        _state = EStates.IsMoving;
                    }
                    break;
            }

        }


        private void MainWindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (_state)
            {
                case EStates.IsDrawingShape:
                    //если рисовалась фигура
                    _listShape.Add(_shapeNew);
                    _state = EStates.Default;
                    break;

                case EStates.IsDrawingLine:
                    DrawingLineEnd(e);
                    break;

                case EStates.IsMoving:
                    //если двигалась фигура
                    _state = EStates.Default;
                    break;
            }
        }

        private void MainWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_state == EStates.IsDrawingLine && _lineNew != null && _finishShape == null)
                {
                    //перерисовка линий
                    DrawingLine(e);
                }
                if (_state == EStates.IsMoving)
                {
                    RedrawShape(e);

                    if (_movingShape != null && _listLine.Count > 0)
                    {
                        RedrawLines();
                    }
                }
            }
        }


        private CShape GetShape(Rectangle rectangle)
        {
            //получение фигуры
            foreach (CShape shape in _listShape)
            {
                if (shape.Rectangle == rectangle)
                {
                    return shape;
                }
            }
            return null;
        }

        //рисование фигуры при нажатии кнопки мыши
        private void DrawingShapeStart(MouseButtonEventArgs e)
        {
            //рисование прямоугольника
            _shapeNew = NewShape();
            _shapeNew.StartPoint = e.GetPosition(this);
            _shapeNew.Draw();
            canvas.Children.Add(_shapeNew.Rectangle);

            Mouse.OverrideCursor = null;
        }

        //перерисовка фигуры при движении мыши (с зажатой кнопкой)
        private void RedrawShape(MouseEventArgs e)
        {
            //задание новых координат прямоугольнику
            double x = e.GetPosition(canvas).X - _movingRectangle.ActualWidth / 2;
            double y = e.GetPosition(canvas).Y - _movingRectangle.ActualHeight / 2;
            _movingRectangle.SetValue(Canvas.LeftProperty, x);
            _movingRectangle.SetValue(Canvas.TopProperty, y);
        }

        //рисование линии при движении мыши (с зажатой кнопкой)
        private void DrawingLine(MouseEventArgs e)
        {
            canvas.Children.Remove(_lineNew.Line);
            _lineNew.EndPoint = e.GetPosition(canvas);
            _lineNew.Draw();
            canvas.Children.Add(_lineNew.Line);
        }

        //рисование линии при нажатии кнопки мыши
        private void DrawingLineStart(MouseButtonEventArgs e)
        {
            if (_lineNew == null)
            {
                _startShape = GetShape(e.Source as Rectangle);
                if (_startShape != null)
                {
                    _finishShape = null;
                    _lineNew = null;

                    _lineNew = NewLine();

                    double width = _startShape.Rectangle.ActualWidth / 2;
                    double height = _startShape.Rectangle.ActualHeight / 2;
                    double x = Canvas.GetLeft(_startShape.Rectangle) + width;
                    double y = Canvas.GetTop(_startShape.Rectangle) + height;

                    _lineNew.StartPoint = new Point(x, y);
                    _lineNew.EndPoint = _lineNew.StartPoint;
                    _lineNew.Draw();
                    canvas.Children.Add(_lineNew.Line);
                }
            }
        }

        //рисование линии, когда кнопка мыши была отпущена
        private void DrawingLineEnd(MouseButtonEventArgs e)
        {
            _finishShape = GetShape(e.Source as Rectangle);
            if (_finishShape != null)
            {
                if (_startShape != null && _lineNew != null && _startShape != _finishShape)
                {
                    //перерисовка линий
                    canvas.Children.Remove(_lineNew.Line);

                    double width = _finishShape.Rectangle.ActualWidth / 2;
                    double height = _finishShape.Rectangle.ActualHeight / 2;
                    double x = Canvas.GetLeft(_finishShape.Rectangle) + width;
                    double y = Canvas.GetTop(_finishShape.Rectangle) + height;
                    _lineNew.EndPoint = new Point(x, y);
                    _lineNew.Draw();
                    canvas.Children.Add(_lineNew.Line);

                    AddLineToShape();

                    _listLine.Add(_lineNew);

                    Mouse.OverrideCursor = null;
                    _state = EStates.Default;

                    _startShape = null;
                    _lineNew = null;
                }
            }
        }

        private void RedrawLines()
        {
            //перерисовывка всех линии, связанных с данной прямоугольником
            double width = _movingShape.Rectangle.ActualWidth / 2;
            double height = _movingShape.Rectangle.ActualHeight / 2;
            double x = Canvas.GetLeft(_movingShape.Rectangle) + width;
            double y = Canvas.GetTop(_movingShape.Rectangle) + height;

            foreach (CLine line in _movingShape.StartShape)
            {
                canvas.Children.Remove(line.Line);
                line.StartPoint = new Point(x, y);
                line.Draw();
                canvas.Children.Add(line.Line);
            }
            foreach (CLine line in _movingShape.EndShape)
            {
                canvas.Children.Remove(line.Line);
                line.EndPoint = new Point(x, y);
                line.Draw();
                canvas.Children.Add(line.Line);
            }
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
                else if (shape == _finishShape)
                {
                    shape.EndShape.Add(_lineNew);
                }
            }
        }

        private CShape NewShape()
        {
            bool? type = rb_dotted.IsChecked;
            if (type.HasValue)
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
            return null;
        }

        private CLine NewLine()
        {
            bool? type = rb_dotted.IsChecked;
            if (type.HasValue)
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
            return null;
        }

        private void but_draw_rectangle_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeAll;
            _state = EStates.IsDrawingShape;
        }

        private void but_draw_arrow_Click(object sender, RoutedEventArgs e)
        {
            if (_listShape.Count() > 1)
            {
                Mouse.OverrideCursor = Cursors.Pen;
                _state = EStates.IsDrawingLine;
            }
        }
    }
}
