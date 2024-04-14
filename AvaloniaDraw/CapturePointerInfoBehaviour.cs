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
        if (AssociatedObject == null)
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
        
        var pointerEvents = Observable.Merge(pointerMoved, pointerPressed, pointerReleased)
            .Scan(PointerInfo.Empty,
                (current, update) =>
                {
                    if (update.MoveEvent != null)
                    {
                        return current.OnMove(update.MoveEvent, AssociatedObject);
                    }

                    if (update.PointerPressedEvent != null)
                    {
                        return current.OnPressed(update.PointerPressedEvent);
                    }

                    if (update.PointerReleasedEvent != null)
                    {
                        return current.OnReleased(update.PointerReleasedEvent);
                    }

                    return current;
                });

        PointerInfoStream = pointerEvents;
    }
}