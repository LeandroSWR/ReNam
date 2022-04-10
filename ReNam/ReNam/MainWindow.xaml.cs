using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReNam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Need to save the margins to a variable at start to save resources
        Thickness gbonMargin;
        Thickness gbnnMargin;

        List<string> onList;
        List<string> nnList;

        public MainWindow()
        {
            InitializeComponent();

            onList = new List<string>();
            nnList = new List<string>();

            // Initialize the margin variables
            gbonMargin = _GbOriginalName.Margin;
            gbnnMargin = _GbNewNames.Margin;
        }

        // Update the Margins so the GroupBoxes never intersect
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gbonMargin.Right = Application.Current.MainWindow.Width - _ReLabel.TransformToAncestor(
                        Application.Current.MainWindow).Transform(
                        new Point(0, 0)).X;
            gbnnMargin.Left = Application.Current.MainWindow.Width - _ReLabel.TransformToAncestor(
                        Application.Current.MainWindow).Transform(
                        new Point(0, 0)).X + _ReLabel.Width;

            _GbOriginalName.Margin = gbonMargin;
            _GbNewNames.Margin = gbnnMargin;
        }

        private void GetFilesData(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null)
                {
                    string fileName;
                    string fileFormat;
                    for (int i = 0; i < files.Length; i++)
                    {
                        fileName = System.IO.Path.GetFileName(files[i]);
                        fileFormat = fileName.Remove(0, fileName.LastIndexOf('.') + 1);
                        if ((ListBox)sender == _ONList)
                        {
                            _ONList.Items.Add(new { Name = fileName.Remove(fileName.LastIndexOf('.')), Format = fileFormat });
                        }
                        else if ((ListBox)sender == _NNList)
                        {
                            _NNList.Items.Add(new { Name = System.IO.Path.GetFileName(files[i])});
                        }
                    }
                }
            }
        }
    }
}
