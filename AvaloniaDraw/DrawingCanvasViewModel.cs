using System.Collections.ObjectModel;

namespace AvaloniaDraw;

public class DrawingCanvasViewModel : ViewModelBase
{
    public ObservableCollection<EllipseViewModel> Ellipses { get; } = new();
    
    //private EllipseViewModel ActiveEllipse { get; set; }
}