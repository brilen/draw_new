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
        enum EStates { IsDrawingShape, IsDrawingLine, IsMoving, Default };
        private int _state = (int)EStates.Default;
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
                case (int)EStates.IsDrawingShape:
                    //рисование прямоугольника
                    _shapeNew = NewShape();
                    _shapeNew.StartPoint = e.GetPosition(this);
                    _shapeNew.Draw();
                    canvas.Children.Add(_shapeNew.Rectangle);

                    Mouse.OverrideCursor = null;
                    break;

                case (int)EStates.IsDrawingLine:
                    if (_lineNew == null)
                    {
                        _startShape = GetShape(e.Source as Rectangle);
                        if (_startShape != null)
                        {
                            //начало рисования линии
                            _finishShape = null;
                            _lineNew = null;

                            _lineNew = NewLine();

                            double x = Canvas.GetLeft(_startShape.Rectangle) + _startShape.Rectangle.ActualWidth / 2;
                            double y = Canvas.GetTop(_startShape.Rectangle) + _startShape.Rectangle.ActualHeight / 2;
                            _lineNew.StartPoint = new Point(x, y);
                            _lineNew.EndPoint = _lineNew.StartPoint;
                            _lineNew.Draw();
                            canvas.Children.Add(_lineNew.Line);
                        }
                    }
                    break;
                case (int)EStates.Default:
                    //получение фигуры, которую перемещают, для перерисоваки линий
                    _movingShape = GetShape(e.Source as Rectangle);

                    //начало перемещения прямоугольника
                    _movingRectangle = e.Source as Rectangle;
                    if (_movingRectangle != null)
                    {
                        _state = (int)EStates.IsMoving;
                    }
                    break;
            }

        }


        private void but_draw_rectangle_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeAll;
            _state = (int)EStates.IsDrawingShape;
        }

        private void but_draw_arrow_Click(object sender, RoutedEventArgs e)
        {
            if (_listShape.Count() > 1)
            {
                Mouse.OverrideCursor = Cursors.Pen;
                _state = (int)EStates.IsDrawingLine;
            }
        }

        private void MainWindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (_state)
            {
                case (int)EStates.IsDrawingShape:
                    //если рисовалась фигура
                    _listShape.Add(_shapeNew);
                    _state = (int)EStates.Default;
                    break;

                case (int)EStates.IsDrawingLine:
                    //если рисовалась линия
                    _finishShape = GetShape(e.Source as Rectangle);

                    if (_startShape != null && _finishShape != null && _lineNew != null && _startShape != _finishShape)
                    {
                        //перерисовка линий
                        canvas.Children.Remove(_lineNew.Line);

                        double x = Canvas.GetLeft(_finishShape.Rectangle) + _finishShape.Rectangle.ActualWidth / 2;
                        double y = Canvas.GetTop(_finishShape.Rectangle) + _finishShape.Rectangle.ActualHeight / 2;
                        _lineNew.EndPoint = new Point(x, y);
                        _lineNew.Draw();
                        canvas.Children.Add(_lineNew.Line);

                        AddLineToShape();

                        _listLine.Add(_lineNew);

                        Mouse.OverrideCursor = null;
                        _state = (int)EStates.Default;

                        _startShape = null;
                        _lineNew = null;
                    }
                    break;
                case (int)EStates.IsMoving:
                    //если двигалась фигура
                    _state = (int)EStates.Default;
                    break;
            }
        }

        private void MainWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _state == (int)EStates.IsDrawingLine && _lineNew != null && _finishShape == null)
            {
                //перерисовка линий
                canvas.Children.Remove(_lineNew.Line);
                _lineNew.EndPoint = e.GetPosition(canvas);
                _lineNew.Draw();
                canvas.Children.Add(_lineNew.Line);

            }

            if (e.LeftButton == MouseButtonState.Pressed && _state == (int)EStates.IsMoving)
            {
                //задание новых координат прямоугольнику
                _movingRectangle.SetValue(Canvas.LeftProperty, e.GetPosition(canvas).X - _movingRectangle.ActualWidth / 2);
                _movingRectangle.SetValue(Canvas.TopProperty, e.GetPosition(canvas).Y - _movingRectangle.ActualHeight / 2);

                if (_movingShape != null && _listLine.Count > 0)
                {
                    RedrawLines();
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

        private void RedrawLines()
        {
            //перерисовывка всех линии, связанных с данной прямоугольником
            foreach (CLine line in _movingShape.StartShape)
            {
                canvas.Children.Remove(line.Line);
                double x = Canvas.GetLeft(_movingShape.Rectangle) + _movingShape.Rectangle.ActualWidth / 2;
                double y = Canvas.GetTop(_movingShape.Rectangle) + _movingShape.Rectangle.ActualHeight / 2;
                line.StartPoint = new Point(x, y);
                line.Draw();
                canvas.Children.Add(line.Line);
            }
            foreach (CLine line in _movingShape.EndShape)
            {
                canvas.Children.Remove(line.Line);
                double x = Canvas.GetLeft(_movingShape.Rectangle) + _movingShape.Rectangle.ActualWidth / 2;
                double y = Canvas.GetTop(_movingShape.Rectangle) + _movingShape.Rectangle.ActualHeight / 2;
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
    }
}
