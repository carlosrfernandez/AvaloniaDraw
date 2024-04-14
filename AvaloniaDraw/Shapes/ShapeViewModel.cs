using Avalonia;
using Avalonia.Media;
using ReactiveUI;

namespace AvaloniaDraw.Shapes;

public class ShapeViewModel : ViewModelBase
{
    private string _type = string.Empty;

    public string Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }
    
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

    private Point _endPosition;

    public Point EndPosition
    {
        get => _endPosition;
        set => this.RaiseAndSetIfChanged(ref _endPosition, value);
    }

    private double _opacity;

    public double Opacity
    {
        get => _opacity;
        set => this.RaiseAndSetIfChanged(ref _opacity, value);
    }

    private int _strokeWidth;

    public int StrokeWidth
    {
        get => _strokeWidth;
        set => this.RaiseAndSetIfChanged(ref _strokeWidth, value);
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