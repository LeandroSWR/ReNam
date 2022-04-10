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

        List<FileName> onList;
        List<string> nnList;

        public MainWindow()
        {
            InitializeComponent();

            onList = new List<FileName>();
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
                    for (int i = 0; i < files.Length; i++)
                    {
                        fileName = System.IO.Path.GetFileName(files[i]);

                        // When we drop items that have no path we still want the name :)
                        if (fileName == null)
                        {
                            fileName = files[i];
                        }
                        
                        // If the method was called from the original names list
                        if ((ListBox)sender == _ONList)
                        {
                            onList.Clear();
                            _ONList.Items.Clear();

                            // Creates a new temporary file
                            FileName currentFile = 
                                new FileName(
                                    fileName.Remove(fileName.LastIndexOf('.')), 
                                    files[i].Replace(fileName, ""),
                                    fileName.Remove(0, fileName.LastIndexOf('.'))
                                    );

                            onList.Add(currentFile);
                            _ONList.Items.Add(new { Name = currentFile.Name, Format = currentFile.Extention });

                            if (_ONList.Items.Count <= _NNList.Items.Count)
                            {
                                _NNList.Items[_ONList.Items.Count - 1] = 
                                    new { 
                                        Name = nnList[_ONList.Items.Count - 1], 
                                        Format = onList[_ONList.Items.Count - 1].Extention};
                            }
                            
                        }
                        // If the method was called from the new names list
                        else if ((ListBox)sender == _NNList)
                        {
                            nnList.Clear();
                            _NNList.Items.Clear();

                            fileName = fileName.Remove(fileName.LastIndexOf('.'));

                            nnList.Add(fileName);
                            if (_ONList.Items.Count > _NNList.Items.Count)
                            {
                                // Update fileFormat to get the matching format from the original item
                                _NNList.Items.Add(new { Name = fileName, Format = onList[_NNList.Items.Count].Extention });
                            }
                            else
                            {
                                _NNList.Items.Add(new { Name = fileName, Visibility = "Hidden"});
                            }
                        }
                    }
                }
            }
        }
    }
}
