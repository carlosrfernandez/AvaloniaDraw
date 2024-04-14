using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Media;
using AvaloniaDraw.Shapes;
using ReactiveUI;

namespace AvaloniaDraw;

public class DrawingCanvasViewModel : ViewModelBase, IDisposable
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
            // If the mouse if down and we get an event coming through then update the bounds of the shape
            var top = Math.Min(_activeShape.Origin.Y, pointerInfo.Position.Y);
            var bottom = Math.Max(_activeShape.Origin.Y, pointerInfo.Position.Y);
            var left = Math.Min(_activeShape.Origin.X, pointerInfo.Position.X);
            var right = Math.Max(_activeShape.Origin.X, pointerInfo.Position.X);

            var topLeft = new Point(left, top);
            var bottomRight = new Point(right, bottom);

            var bounds = new Rect(topLeft, bottomRight);
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

    private string FormatPointerInfo(PointerInfo pointerInfo)
    {
        return $"X: {pointerInfo.Position.X} Y: {pointerInfo.Position.Y}";
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}