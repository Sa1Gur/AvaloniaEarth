using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace AvaloniaEarth.Views
{
    public partial class MainWindow : Window
    {
        private readonly ZoomBorder? _zoomBorder;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
            _zoomBorder = this.Find<ZoomBorder>("ZoomBorder");
            if (_zoomBorder != null)
            {
                _zoomBorder.KeyDown += ZoomBorder_KeyDown;
                
                _zoomBorder.ZoomChanged += ZoomBorder_ZoomChanged;
            }
            
            double r = 637.1302;//r* tg a - r * a = 0.00001;
            var alpha = Math.Cbrt(0.000015 / r);//0.00003 //tg a = a + a^3 / 3 + a^5 / 5 ...
            //r * (Math.tan(a) - a) = 0.00001 / r;//a^3 / 3 = 0.00001 / r; a = (0.00003 / r) ^ 1/3

            var difference =  r / Math.Cos(alpha) - r;
            
            PathFigure myPathFigure = new ();
            myPathFigure.StartPoint = new Point(650, 650);

            LineSegment myLineSegment1 = new ();
            myLineSegment1.Point = new Point(650, 650 - r / Math.Cos(alpha));
            
            LineSegment myLineSegment2 = new ();
            myLineSegment2.Point = new Point(650, 70);

            PathSegments myPathSegmentCollection = new ();
            myPathSegmentCollection.Add(myLineSegment1);
            //myPathSegmentCollection.Add(myLineSegment2);

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigures myPathFigureCollection = new ();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            Path myPath = this.Find<Path>("MyPathFigure");
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 0.001;
            myPath.Data = myPathGeometry;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void ZoomBorder_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F:
                    _zoomBorder?.Fill();
                    break;
                case Key.U:
                    _zoomBorder?.Uniform();
                    break;
                case Key.R:
                    _zoomBorder?.ResetMatrix();
                    break;
                case Key.T:
                    _zoomBorder?.ToggleStretchMode();
                    _zoomBorder?.AutoFit();
                    break;
            }
        }

        private void ZoomBorder_ZoomChanged(object sender, ZoomChangedEventArgs e)
        {
            Debug.WriteLine($"[ZoomChanged] {e.ZoomX} {e.ZoomY} {e.OffsetX} {e.OffsetY}");
        }
    }
}