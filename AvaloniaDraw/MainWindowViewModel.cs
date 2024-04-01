using System;
using Avalonia;

namespace AvaloniaDraw;

public class MainWindowViewModel : ViewModelBase, IDisposable
{
    private IDisposable _shapeSubscription;
    private Random _random = new();
    
    public MainWindowViewModel(ToolboxViewModel toolbox, DrawingCanvasViewModel drawingCanvas)
    {
        Toolbox = toolbox;
        DrawingCanvas = drawingCanvas;

        _shapeSubscription = toolbox.AddShape.Subscribe(
            _ => DrawingCanvas.Ellipses.Add(new EllipseViewModel
            {
                Width = 100,
                Height = 50,
                Opacity = 1,
                Centre = new Point(_random.NextDouble() * 500, _random.NextDouble() * 500)
            }));
    }

    public ToolboxViewModel Toolbox { get; }
    public DrawingCanvasViewModel DrawingCanvas { get; }

    public void Dispose()
    {
        _shapeSubscription.Dispose();
    }
}