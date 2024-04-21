using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace AvaloniaDraw;

public class CapturePointerInfoBehaviour : Behavior<Control>
{
    public static readonly StyledProperty<IObservable<PointerInfo>> PointerInfoStreamProperty =
        AvaloniaProperty.Register<CapturePointerInfoBehaviour, IObservable<PointerInfo>>(
            "PointerInfoStream");

    public IObservable<PointerInfo> PointerInfoStream
    {
        get => GetValue(PointerInfoStreamProperty);
        set => SetValue(PointerInfoStreamProperty, value);
    }

    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        SetPointerInfoStream();
    }

    private void SetPointerInfoStream()
    {
        var topLevel = TopLevel.GetTopLevel(AssociatedObject);
        
        if (AssociatedObject == null || topLevel == null)
        {
            return;
        }

        var pointerMoved = Observable.FromEventPattern<PointerEventArgs>(AssociatedObject, nameof(Control.PointerMoved))
            .Select(pm => new PointerEvent { MoveEvent = pm.EventArgs });

        var pointerPressed = Observable
            .FromEventPattern<PointerPressedEventArgs>(AssociatedObject, nameof(Control.PointerPressed))
            .Select(pp => new PointerEvent { PointerPressedEvent = pp.EventArgs });
        
        var pointerReleased = Observable
            .FromEventPattern<PointerReleasedEventArgs>(AssociatedObject, nameof(Control.PointerReleased))
            .Select(pr => new PointerEvent { PointerReleasedEvent = pr.EventArgs });
        
        // Hook into the key press events from the top level (the main window) as 
        // the events are not available on the canvas
        var keyDown = Observable.FromEventPattern<KeyEventArgs>(topLevel, nameof(Control.KeyDown))
            .Select(pm => new PointerEvent { KeyDownEvent = pm.EventArgs });
        
        var keyUp = Observable.FromEventPattern<KeyEventArgs>(topLevel, nameof(Control.KeyUp))
            .Select(pm => new PointerEvent { KeyUpEvent = pm.EventArgs });

        var pointerEvents = Observable.Merge(pointerMoved, pointerPressed, pointerReleased,
                                             keyDown, keyUp)
            .Scan(PointerInfo.Empty,
                (current, update) => UpdatePointerInfo(AssociatedObject, update, current));

        PointerInfoStream = pointerEvents;
    }

    private PointerInfo UpdatePointerInfo(Visual relativeTo, PointerEvent update, PointerInfo current)
    {
        if (update.MoveEvent != null)
        {
            return current.OnMove(update.MoveEvent, relativeTo);
        }

        if (update.PointerPressedEvent != null)
        {
            return current.OnPressed(update.PointerPressedEvent);
        }

        if (update.PointerReleasedEvent != null)
        {
            return current.OnReleased(update.PointerReleasedEvent);
        }
                    
        if (update.KeyDownEvent != null)
        {
            return current.OnKeyDown(update.KeyDownEvent);
        }
                    
        if (update.KeyUpEvent != null)
        {
            return current.OnKeyUp(update.KeyUpEvent);
        }

        return current;
    }
}