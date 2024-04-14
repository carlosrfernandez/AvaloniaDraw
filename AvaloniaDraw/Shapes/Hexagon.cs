using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AvaloniaDraw.Shapes;

public class Hexagon : Shape
{
    /*
     * The vertices of a regular hexagon are given by
     * xi=0.5+0.5*cos(θi)
     * yi=0.5+0.5*sin(θi)
     *
     * The two constants for the margins represent where the top and bottom of the hexagon
     * is relative to the bounding box based on applying the above formula for each vertex
     */
    private const double TopMargin = 0.06669873;
    private const double BottomMargin = 0.93330127;

    static Hexagon()
    {
        AffectsGeometry<Hexagon>(BoundsProperty, StrokeThicknessProperty);
    }
    protected override Geometry CreateDefiningGeometry()
    {
        var geometry = new StreamGeometry();

        var rect = new Rect(Bounds.Size).Deflate(StrokeThickness / 2);
        var start = new Point(rect.Right, rect.Height * 0.5 + rect.Top);

        using var context = geometry.Open();
        
        context.BeginFigure(start, true);
        var point = new Point(rect.Width * 0.75 + rect.Left, rect.Height * BottomMargin + rect.Top);
        context.LineTo(point);
            
        point = new Point(rect.Width * 0.25 + rect.Left, rect.Height * BottomMargin + rect.Top);
        context.LineTo(point);
            
        point = new Point(rect.Left, rect.Height * 0.5 + rect.Top);
        context.LineTo(point);
            
        point = new Point(rect.Width * 0.25 + rect.Left, rect.Height * TopMargin + rect.Top);
        context.LineTo(point);       
            
        point = new Point(rect.Width * 0.75 + rect.Left, rect.Height * TopMargin + rect.Top);
        context.LineTo(point);   
            
        context.EndFigure(true);

        return geometry;
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(StrokeThickness, StrokeThickness);
    }
}