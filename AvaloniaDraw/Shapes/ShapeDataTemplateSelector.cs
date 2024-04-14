using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace AvaloniaDraw.Shapes;

public class ShapeDataTemplateSelector : IDataTemplate
{
    [Content]
    // ReSharper disable once CollectionNeverUpdated.Global
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new();
    
    public Control? Build(object? param)
    {
        if (param is ShapeViewModel shapeViewModel)
        {
            return AvailableTemplates[shapeViewModel.Type].Build(param);
        }

        throw new ArgumentException("Could not determine shape", nameof(param));
    }

    public bool Match(object? data)
    {
        if (data is ShapeViewModel shapeViewModel)
        {
            return data is ShapeViewModel                       
                   && !string.IsNullOrEmpty(shapeViewModel.Type)          
                   && AvailableTemplates.ContainsKey(shapeViewModel.Type);
        }

        return false;

    }
}