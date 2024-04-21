using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaDraw.Shapes;
using ReactiveUI;

namespace AvaloniaDraw;

public sealed class DrawingCanvasViewModel : ViewModelBase, IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    private readonly SerialDisposable _pointerInfoDisposable = new();
    private IObservable<PointerInfo>? _pointerInfoStream = Observable.Never<PointerInfo>();
    private ShapeViewModel? _activeShape;
    private Color _currentFillColour;
    private Color _currentOutlineColour;
    private int _strokeWidth;
    private string _shapeType = string.Empty;

    public DrawingCanvasViewModel()
    {
        _disposables.Add(_pointerInfoDisposable);
        _disposables.Add(MessageBus.Current.ListenIncludeLatest<Color>(Topics.FillColour)
            .Subscribe(c => _currentFillColour = c));

        _disposables.Add(MessageBus.Current.ListenIncludeLatest<Color>(Topics.OutlineColour)
            .Subscribe(c => _currentOutlineColour = c));
        
        _disposables.Add(MessageBus.Current.ListenIncludeLatest<int>(Topics.StrokeWidth)
            .Subscribe(w => _strokeWidth = w));

        _disposables.Add(MessageBus.Current.ListenIncludeLatest<string>(Topics.ShapeType)
            .Subscribe(s => _shapeType = s));        
    }

    public ObservableCollection<ShapeViewModel> Shapes { get; } = [];
    
    public IObservable<PointerInfo>? PointerInfoStream
    {
        get => _pointerInfoStream;
        set
        {
            _pointerInfoStream = value;

            if (value != null)
            {
                _pointerInfoDisposable.Disposable = value.Subscribe(OnPointerInfoChanged);
            }
        }
    }
    
    private string? _pointerInfoText;
    
    public string? PointerInfoText
    {
        get => _pointerInfoText;
        set => this.RaiseAndSetIfChanged(ref _pointerInfoText, value);
    }

    private void OnPointerInfoChanged(PointerInfo pointerInfo)
    {
        PointerInfoText = FormatPointerInfo(pointerInfo);
        var cancelCurrentShape = pointerInfo.PressedKeys.Contains(Key.Escape);
        
        if (pointerInfo.IsDragging && _activeShape == null)
        {
            _activeShape = new ShapeViewModel
            {
                Opacity = 0.3,
                Origin = pointerInfo.Position,
                FillColour = new SolidColorBrush(_currentFillColour),
                OutlineColour = new SolidColorBrush(_currentOutlineColour),
                StrokeWidth = _strokeWidth,
                Type = _shapeType
            };

            // Then add it to the collection. This is the bit that causes it to be drawn on screen
            Shapes.Add(_activeShape);
        }

        if (_activeShape != null && pointerInfo.IsPressed)
        {
            if (cancelCurrentShape)
            {
                Shapes.Remove(_activeShape);
                _activeShape = null;
                return;
            }
            
            var makeUniform = pointerInfo.PressedKeys.Contains(Key.LeftShift) ||
                             pointerInfo.PressedKeys.Contains(Key.RightShift);

            var bounds = DetermineShapeBounds(_activeShape, pointerInfo, makeUniform);
            
            _activeShape.Bounds = bounds;
            _activeShape.EndPosition = pointerInfo.Position;
        }

        if (pointerInfo.IsEndDrag)
        {
            // Then when the mouse button goes up make it fully opaque
            if (_activeShape != null)
            {
                _activeShape.Opacity = 1;
                _activeShape = null;
            }
        }
    }
    
    private static Rect DetermineShapeBounds(ShapeViewModel activeShape, PointerInfo pointerInfo, bool makeUniform)
    {
        Rect bounds;
        if (makeUniform)
        {
            // Finds the smallest side of the shape and uses that to create a bounding box that is square in shape
            // in the direction of the position of the pointer.
            var distance = Math.Min(
                Math.Abs(activeShape.Origin.X - pointerInfo.Position.X),
                Math.Abs(activeShape.Origin.Y - pointerInfo.Position.Y)
            );

            var xPosition = pointerInfo.Position.X < activeShape.Origin.X
                ? activeShape.Origin.X - distance
                : activeShape.Origin.X + distance;

            var yPosition = pointerInfo.Position.Y < activeShape.Origin.Y
                ? activeShape.Origin.Y - distance
                : activeShape.Origin.Y + distance;

            return CalculateBoundsFromOriginToPosition(activeShape.Origin, new Point(xPosition, yPosition));
        }

        return CalculateBoundsFromOriginToPosition(activeShape.Origin, pointerInfo.Position);

        Rect CalculateBoundsFromOriginToPosition(Point origin, Point position)
        {
            var top = Math.Min(origin.Y, position.Y);
            var bottom = Math.Max(origin.Y, position.Y);
            var left = Math.Min(origin.X, position.X);
            var right = Math.Max(origin.X, position.X);

            var topLeft = new Point(left, top);
            var bottomRight = new Point(right, bottom);

            bounds = new Rect(topLeft, bottomRight);
            return bounds;
        }
    }

    private static string FormatPointerInfo(PointerInfo pointerInfo)
    {
        return $"X: {pointerInfo.Position.X} Y: {pointerInfo.Position.Y}";
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}