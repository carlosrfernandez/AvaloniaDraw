using System.Reactive;
using Avalonia.Media;
using ReactiveUI;

namespace AvaloniaDraw;

public class ToolboxViewModel : ViewModelBase
{
    public ToolboxViewModel()
    {
        AddShape = ReactiveCommand.Create(() => { });
        Outline = Colors.Black;
        Fill = Colors.Red;
    }

    public ReactiveCommand<Unit, Unit> AddShape { get; set; }
    
    private Color _fill;

    public Color Fill
    {
        get => _fill;
        set => this.RaiseAndSetIfChanged(ref _fill, value);
    }

    private Color _outline;
    
    public Color Outline
    {
        get => _outline;
        set => this.RaiseAndSetIfChanged(ref _outline, value);
    }
}