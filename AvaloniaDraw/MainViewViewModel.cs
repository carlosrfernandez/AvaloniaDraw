using AvaloniaDraw.Toolbox;

namespace AvaloniaDraw;

public class MainViewViewModel(ToolboxPanelViewModel toolboxPanel, DrawingCanvasViewModel drawingCanvas)
    : ViewModelBase
{
    public ToolboxPanelViewModel ToolboxPanel { get; } = toolboxPanel;
    public DrawingCanvasViewModel DrawingCanvas { get; } = drawingCanvas;
}