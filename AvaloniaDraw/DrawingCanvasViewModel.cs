using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.Json;
using Avalonia.Media;
using ReactiveUI;

namespace AvaloniaDraw;

public class DrawingCanvasViewModel : ViewModelBase, IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    private readonly SerialDisposable _pointerInfoDisposable = new();
    private IObservable<PointerInfo>? _pointerInfoStream = Observable.Never<PointerInfo>();
    private EllipseViewModel? _activeEllipse;
    private Color _currentFillColour;
    private Color _currentOutlineColour;

    public DrawingCanvasViewModel()
    {
        _disposables.Add(_pointerInfoDisposable);
        _disposables.Add(MessageBus.Current.ListenIncludeLatest<Color>(Topics.FillColour)
            .Subscribe(c => _currentFillColour = c));

        _disposables.Add(MessageBus.Current.ListenIncludeLatest<Color>(Topics.OutlineColour)
            .Subscribe(c => _currentOutlineColour = c));
    }

    public ObservableCollection<EllipseViewModel> Ellipses { get; } = [];
    
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
        var text = JsonSerializer.Serialize(pointerInfo);
        PointerInfoText = text;
        
        if (pointerInfo.IsDragging && _activeEllipse == null)
        {
            _activeEllipse = new EllipseViewModel
            {
                Opacity = 0.3,
                Centre = pointerInfo.Position,
                FillColour = new SolidColorBrush(_currentFillColour),
                OutlineColour = new SolidColorBrush(_currentOutlineColour)
            };

            // Then add it to the collection. This is the bit that causes it to be drawn on screen
            Ellipses.Add(_activeEllipse);
        }

        if (_activeEllipse != null && pointerInfo.IsPressed)
        {
            // If the mouse if down and we get an event coming through then update the width and height of the ellipse
            var width = Math.Abs(Convert.ToInt32(pointerInfo.Position.X - _activeEllipse.Centre.X));
            var height = Math.Abs(Convert.ToInt32(pointerInfo.Position.Y - _activeEllipse.Centre.Y));
            
            _activeEllipse.Width = width;
            _activeEllipse.Height = height;
        }

        if (pointerInfo.IsEndDrag)
        {
            // Then when the mouse button goes up make it fully opaque
            if (_activeEllipse != null)
            {
                _activeEllipse.Opacity = 1;
                _activeEllipse = null;
            }
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}