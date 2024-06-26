﻿using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Media;
using AvaloniaDraw.Shapes;
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
        StrokeWidth = 3;
        ShapeType = ShapeTypes.Ellipse;
        
        MessageBus.Current.RegisterMessageSource(
            this.WhenPropertyChanged(vm => vm.FillColour).Select(pc => pc.Value),
            Topics.FillColour);
        
        MessageBus.Current.RegisterMessageSource(
            this.WhenPropertyChanged(vm => vm.OutlineColour).Select(pc => pc.Value),
            Topics.OutlineColour);
        
        MessageBus.Current.RegisterMessageSource(
            this.WhenPropertyChanged(vm => vm.StrokeWidth).Select(pc => pc.Value),
            Topics.StrokeWidth);
        
        MessageBus.Current.RegisterMessageSource(
            this.WhenPropertyChanged(vm => vm.ShapeType).Select(pc => pc.Value),
            Topics.ShapeType);
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

    private int _strokeWidth;

    public int StrokeWidth
    {
        get => _strokeWidth;
        set => this.RaiseAndSetIfChanged(ref _strokeWidth, value);
    }

    private string _shapeType = string.Empty;

    public string ShapeType
    {
        get => _shapeType;
        set => this.RaiseAndSetIfChanged(ref _shapeType, value);
    }

    public void OnClearButtonClicked()
    {
        MessageBus.Current.SendMessage(Unit.Default, Topics.ClearShapes);
    }
}