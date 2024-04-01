using System.Collections.Generic;
using Avalonia.Controls;

namespace AvaloniaDraw;

public partial class ColourSelector : UserControl
{
    public ColourSelector()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string LabelText { get; set; } = string.Empty;

    public List<string> Colours { get; } =
    [
        "Red",
        "Orange",
        "Yellow",
        "Green",
        "Blue",
        "Purple",
        "Black",
        "Gray",
        "White",
        "Transparent"
    ];


}