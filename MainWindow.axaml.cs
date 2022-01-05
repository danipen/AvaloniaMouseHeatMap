using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace AvaloniaMouseHeatMap
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        
            IControl windowContent = (IControl)Content;
            Content = null;

            mContent = new Grid();
            mContent.Children.Add(windowContent);

            Content = mContent;

            mHeatMapPanel = new HeatMapPanel();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F3)
            {
                if (mContent.Children.Contains(mHeatMapPanel))
                {
                    mContent.Children.Remove(mHeatMapPanel);
                }
                else
                {
                    mContent.Children.Add(mHeatMapPanel);
                }
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            mHeatMapPanel.RegisterPoint(e.GetPosition(this));
            mHeatMapPanel.InvalidateVisual();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            mHeatMapPanel.RegisterPoint(e.GetPosition(this));
            mHeatMapPanel.InvalidateVisual();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        class HeatMapPanel : Panel
        {
            internal HeatMapPanel()
            {
                Background = new ImmutableSolidColorBrush(
                    ColorMap.GetColorForValue(0, 25));
            }

            internal void RegisterPoint(Point p)
            {
                Rect r = GetGridRect(p);

                if (!mMouseAreas.ContainsKey(r))
                {
                    mMouseAreas.Add(r, 0);
                }

                mMouseAreas[r]++;
            }

            static Rect GetGridRect(Point p)
            {
                int x = (int)Math.Floor(p.X / mGridSize);
                int y = (int)Math.Floor(p.Y / mGridSize);

                return new Rect(x * mGridSize, y * mGridSize, mGridSize, mGridSize);
            }

            public override void Render(DrawingContext context)
            {
                base.Render(context);

                foreach (var p in mMouseAreas.Keys)
                {
                    context.FillRectangle(new SolidColorBrush(
                        ColorMap.GetColorForValue(mMouseAreas[p], 15)),
                        new Rect(p.X, p.Y, mGridSize, mGridSize));
                }
            }

            const double mGridSize = 5;
            Dictionary<Rect, int> mMouseAreas = new Dictionary<Rect, int>();
        }

        Grid mContent;
        HeatMapPanel mHeatMapPanel;
    }
}