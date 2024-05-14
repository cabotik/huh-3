using Syncfusion.UI.Xaml.Charts;

namespace huh
{
    public class ViewGraph
    {
        public List<GraphField> graphs { get; set; }
        public ChartColorPalette graphPalette { get; set; } = ChartColorPalette.RedChrome;
        public ViewGraph() { }
        public ViewGraph(List<GraphField> fields)
        {
            graphs = new List<GraphField>();
            graphs.AddRange(fields);
        }
    }
}