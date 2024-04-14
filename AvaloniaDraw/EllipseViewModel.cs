using Avalonia;
using Avalonia.Media;
using ReactiveUI;

namespace AvaloniaDraw;

public class EllipseViewModel : ViewModelBase
{
    private Point _origin;

    public Point Origin
    {
        get => _origin;
        set => this.RaiseAndSetIfChanged(ref _origin, value);
    }

    private Rect _bounds;

    public Rect Bounds
    {
        get => _bounds;
        set => this.RaiseAndSetIfChanged(ref _bounds, value);
    }

    private double _opacity;

    public double Opacity
    {
        get => _opacity;
        set => this.RaiseAndSetIfChanged(ref _opacity, value);
    }

    private Brush _outlineColour;

    public Brush OutlineColour
    {
        get => _outlineColour;
        set => this.RaiseAndSetIfChanged(ref _outlineColour, value);
    }

    private Brush _fillColour;

    public Brush FillColour
    {
        get => _fillColour;
        set => this.RaiseAndSetIfChanged(ref _fillColour, value);
    }   
}