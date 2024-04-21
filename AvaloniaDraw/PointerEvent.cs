using Avalonia.Input;

namespace AvaloniaDraw;

public class PointerEvent
{
    public PointerEventArgs? MoveEvent { get; init; }
    public PointerPressedEventArgs? PointerPressedEvent { get; init; }
    public PointerReleasedEventArgs? PointerReleasedEvent { get; init; }
    public KeyEventArgs? KeyDownEvent { get; init; }
    public KeyEventArgs? KeyUpEvent { get; init; }
        
}