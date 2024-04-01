using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Media;
using DynamicData.Binding;
using ReactiveUI;

namespace AvaloniaDraw.Toolbox;

public class ToolboxPanelViewModel : ViewModelBase
{
    private static readonly Color DefaultOutlineColour = Colors.Black;
    private static readonly Color DefaultFillColour = Colors.Red;
    
    public ToolboxPanelViewModel()
    {
        OutlineColour = DefaultOutlineColour;
        FillColour = DefaultFillColour;
        
        MessageBus.Current.RegisterMessageSource(
            this.WhenPropertyChanged(vm => vm.FillColour).Select(pc => pc.Value),
            Topics.FillColour);
        
        MessageBus.Current.RegisterMessageSource(
            this.WhenPropertyChanged(vm => vm.OutlineColour).Select(pc => pc.Value),
            Topics.OutlineColour);
    }
    
    private Color _fillColour;

    public Color FillColour
    {
        get => _fillColour;
        set => this.RaiseAndSetIfChanged(ref _fillColour, value);
    }

    private Color _outlineColour;
    
    public Color OutlineColour
    {
        get => _outlineColour;
        set => this.RaiseAndSetIfChanged(ref _outlineColour, value);
    }
}