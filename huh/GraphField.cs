using Syncfusion.UI.Xaml.Charts;

namespace huh
{
    public class GraphField
    {
        public string graphName { get; set; }
        public int graphValue { get; set; }
        public ChartColorPalette graphPalette { get; set; }

        public GraphField()
        {
            graphName = new String("");
            graphValue = 0;
        }
    }
}