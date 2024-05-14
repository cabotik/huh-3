using Aspose.Cells;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Charts;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace huh
{
    public partial class MainWindow : Window
    {
        public ChartColorPalette graphPalette;
        Graph graph = new Graph();
        public string integ;
        List<GraphField> graphs = new List<GraphField>();
        GraphField gf = new GraphField(); 

        public MainWindow()
        {
            InitializeComponent();           
        }

        private void btnPChart_Click(object sender, RoutedEventArgs e)
        {
            spPalette.Visibility = Visibility.Visible;
            graph.graphType = "Pie";            
        }

        private void btnHGrahp_Click(object sender, RoutedEventArgs e)
        {
            spPalette.Visibility = Visibility.Visible;
            graph.graphType = "Horizontal";
        }

        private void btnVGraph_Click(object sender, RoutedEventArgs e)
        {
            spPalette.Visibility = Visibility.Visible;
            graph.graphType = "Vertical";
        }
        private void btnPolarChart_Click(object sender, RoutedEventArgs e)
        {
            spPalette.Visibility = Visibility.Visible;  
            graph.graphType = "Polar";
        }

        private void btnSChart_Click(object sender, RoutedEventArgs e)
        {
            spPalette.Visibility = Visibility.Visible;
            graph.graphType = "Spline";
        }
        private void btnFloraHues_Click(object sender, RoutedEventArgs e)
        {
            graphPalette = ChartColorPalette.FloraHues;
            spExport.Visibility = Visibility.Visible;
            spReference.Visibility = Visibility.Visible;
            spRefreash.Visibility = Visibility.Visible;
        }

        private void btnTomotoSpectrum_Click(object sender, RoutedEventArgs e)
        {
            graphPalette = ChartColorPalette.TomotoSpectrum;
            spExport.Visibility = Visibility.Visible;
            spReference.Visibility = Visibility.Visible;
            spRefreash.Visibility = Visibility.Visible;
        }

        private void btnPineapple_Click(object sender, RoutedEventArgs e)
        {
            graphPalette = ChartColorPalette.Pineapple;
            spExport.Visibility = Visibility.Visible;
            spReference.Visibility = Visibility.Visible;
            spRefreash.Visibility = Visibility.Visible;
        }

        private void btnAutumnBrights_Click(object sender, RoutedEventArgs e)
        {
            graphPalette = ChartColorPalette.AutumnBrights;
            spExport.Visibility = Visibility.Visible;
            spReference.Visibility = Visibility.Visible;
            spRefreash.Visibility = Visibility.Visible;
        }

        private void btnTyping_Click(object sender, RoutedEventArgs e)
        {
            spManualInput.Visibility = Visibility.Visible;
            spPalette.Visibility = Visibility.Collapsed;
            spTypyOfDiagram.Visibility = Visibility.Collapsed;
            spExport.Visibility = Visibility.Collapsed;
            spReference.Visibility = Visibility.Collapsed;
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            bool? success = openFileDialog.ShowDialog();
            var path = openFileDialog.FileName;
            if (success == true)
            {
                Workbook wb = new Workbook(path);
                WorksheetCollection collection = wb.Worksheets;
                Worksheet worksheet = collection[0];
                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;
                List<GraphField> gr = new List<GraphField>();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (worksheet.Cells[i, j].Value != null)
                        {
                            GraphField graphField = new GraphField();

                            graphField.graphName = worksheet.Cells[i, j + 1].Value.ToString();
                            try
                            {
                                graphField.graphValue = Convert.ToInt32(worksheet.Cells[i + 1, j].Value);
                                gr.Add(graphField);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }

                    }
                }
                CreateCharts(new ViewGraph(gr));
            }
            else { Message("File didnt choose"); }
        }
     
        private void btnJsonf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            JsonImport jsonImport = new JsonImport();
            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            bool? success = openFileDialog.ShowDialog();
            if (success == true)
            {
                jsonImport.path = openFileDialog.FileName;
                jsonImport.JI(out ViewGraph graphv);
                CreateCharts(graphv);
            }
            else { Message("File didnt choose"); }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            GraphField graphField = new GraphField();
            int a = 1;
            foreach (var stack in spValue.Children)                                 //пробежка по всем элементам окна
            {
                a++;
                StackPanel? stackPanel = stack as StackPanel;    //поиск StackPanel
                if (a % 2 == 1) 
                { graphField = new GraphField(); }
                if (stackPanel != null)
                {
                    foreach (var tbn in stackPanel.Children)                        //пробежка по всем полям
                    {
                      
                        if (tbn != null)
                        {
                            TextBox? textBox = tbn as TextBox;
                            if (textBox != null && textBox.Name == "tbGraphName")   //заполнение имени
                            {
                                graphField.graphName = textBox.Text;
                            }
                            if (textBox != null && textBox.Name == "tbGraphValue")  //заполнение величины
                            {
                                if (Int32.TryParse(textBox.Text, out var val)) graphField.graphValue = val;
                                else graphField.graphValue = 0;
                            }
                        }
                        graphs.Add(graphField);                                     //добавление графа
                    }
                    if(a % 2 == 0) 
                    { graphs.Add(graphField); }
                }
            }
            ViewGraph vgraph = new ViewGraph(graphs);
            CreateCharts(vgraph);
        }

        public void CreateCharts(ViewGraph graphv)
        { 
            graphv.graphPalette = graphPalette;
             switch (graph.graphType)
                {
                    case "Pie":
                        spSaveBtn.Visibility = Visibility.Visible;
                        spPieChart.Visibility = Visibility.Visible;                        
                        this.DataContext = graphv;
                   
                    break;
                    case "Vertical":
                        spSaveBtn.Visibility = Visibility.Visible;
                        spVChart.Visibility = Visibility.Visible;
                        this.DataContext = graphv;
                        break;
                    case "Horizontal":
                        spSaveBtn.Visibility = Visibility.Visible;
                        spHChart.Visibility = Visibility.Visible;
                        this.DataContext = graphv;
                        break;
                    case "Polar":
                        spSaveBtn.Visibility = Visibility.Visible;
                        spPolarChart.Visibility = Visibility.Visible;
                        this.DataContext = graphv;
                        break;
                    case "Spline":
                        spSaveBtn.Visibility = Visibility.Visible;
                        spSChart.Visibility = Visibility.Visible;
                        this.DataContext = graphv;
                        break;
                }
        }

        bool click = false;
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!click)
            { 
                spValue.Visibility = Visibility.Visible;
                spRefreash.Visibility = Visibility.Visible;
                FieldsCreater();
                click = true;
            }
        }
        private void Message(String mes)    //Упрощаем вызов сообщений
        {
            MessageBox.Show(mes, "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void CreateField()
        {
            //это стек нейм
            StackPanel spPanelN = GetPanel("Name");
            //это стек вэлью
            StackPanel spPanelV = GetPanel("Value");

            spValue.Children.Add(spPanelN);
            spValue.Children.Add(spPanelV);
        }
        private StackPanel GetPanel(string st)
        {
            StackPanel local = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Name = "spForTwoFields"
            };
            local.Children.Add(GetLabel(st));
            local.Children.Add(GetBox(st));
            return local;
        }
        private Label GetLabel(String str)
        {
            return new Label() { Content = "Enter " + str + " of graph:" };
        }
        private TextBox GetBox(String str)
        {
            return new TextBox() { Name = "tbGraph" + str };
        }
        public void FieldsCreater()
        {
            String tbText = tbFields.Text;
            Graph graphField = new Graph();
            int counter;
            if (String.IsNullOrEmpty(tbText)) Message("Enter something.");
            else
            {
                if (int.TryParse(tbText, out counter))
                {
                    graphField.fieldQuantity = int.Parse(tbText);
                    for (int i = 0; i < graphField.fieldQuantity; i++)
                    {
                        CreateField();
                    }
                }
                else Message("Not an integer.");
            }
            spBtnCreate.Visibility = Visibility.Visible;
        }
              
        private void btnRefreash_Click(object sender, RoutedEventArgs e)
        {
            //graph.fieldQuantity = 0;
            //tbFields.Clear();
            //graphs.Clear();
            //FieldsCreater();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();//лицензия <3

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg,*.jpeg)|*.jpg;*.jpeg|Gif (*.gif)|*.gif|PNG(*.png)|*.png|TIFF(*.tif,*.tiff)|*.tif|All files (*.*)|*.*";

            if (sfd.ShowDialog() == true)
            {
                using (Stream fs = sfd.OpenFile())
                {
                    ViewGraph vgraph = new ViewGraph(graphs);
                    switch (graph.graphType)
                    {
                        case "Pie":
                            cPie.Save(fs, new PngBitmapEncoder());
                            break;
                        case "cVertical":
                            cSpline.Save(fs, new PngBitmapEncoder());
                            break;
                        case "cHorizontal":
                            cSpline.Save(fs, new PngBitmapEncoder());
                            break;
                        case "Polar":
                            cPolar.Save(fs, new PngBitmapEncoder());
                            break;
                        case "Spline":
                            cSpline.Save(fs, new PngBitmapEncoder());
                            break;
                    }
                }

            }
        }
      
    }
}