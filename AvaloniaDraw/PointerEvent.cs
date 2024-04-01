using Avalonia.Input;

namespace AvaloniaDraw;

public class PointerEvent
{
    public PointerEventArgs? MoveEvent { get; init; }
    public PointerPressedEventArgs? PointerPressedEvent { get; init; }
    public PointerReleasedEventArgs? PointerReleasedEvent { get; init; }
        
}