using System;
using Avalonia;
using AvaloniaDraw.Toolbox;

namespace AvaloniaDraw;

public class MainWindowViewModel(ToolboxPanelViewModel toolboxPanel, DrawingCanvasViewModel drawingCanvas)
    : ViewModelBase
{
    public ToolboxPanelViewModel ToolboxPanel { get; } = toolboxPanel;
    public DrawingCanvasViewModel DrawingCanvas { get; } = drawingCanvas;
}