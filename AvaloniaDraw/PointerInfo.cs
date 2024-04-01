using Avalonia;
using Avalonia.Input;

namespace AvaloniaDraw;

/// <summary>
/// This is a simple struct to define the bits about the mouse movement we're interested in
/// so we can get all the information into the ViewModel without having reference to anything
/// UI related
/// </summary>
public readonly record struct PointerInfo(
    Point Position,
    bool IsPressed,
    bool IsDragging,
    bool IsEndDrag)
{
    public static PointerInfo Empty => new(new Point(0, 0), false, false, false);

    public PointerInfo OnMove(PointerEventArgs pointerMoved, Visual relativeTo)
    {
        var isStartDrag = IsPressed;
        return this with
        {
            Position = pointerMoved.GetPosition(relativeTo),
            IsDragging = isStartDrag,
            IsEndDrag = false
        };
    }

    public PointerInfo OnPressed(PointerPressedEventArgs _)
    {
        return this with
        {
            IsPressed = true
        };
    }
    
    public PointerInfo OnReleased(PointerReleasedEventArgs _)
    {
        var isEndDrag = IsDragging;
        
        return this with
        {
            IsPressed = false,
            IsEndDrag = isEndDrag
        };
    }
}