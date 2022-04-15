using System;
using System.Collections.Generic;
using System.IO;
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
        private Thickness gbonMargin;
        private Thickness gbnnMargin;
        private Thickness gbRulesMargin;
        private Thickness bttRename;

        private List<FileName> onList;
        private List<string> nnList;

        private List<string> allfiles;

        private AddRuleWindow rulesWindow;

        public MainWindow()
        {
            InitializeComponent();

            onList = new List<FileName>();
            nnList = new List<string>();
            allfiles = new List<string>();

            // Initialize the margin variables
            gbonMargin = _GbOriginalName.Margin;
            gbnnMargin = _GbNewNames.Margin;
            gbRulesMargin = _GbRulesList.Margin;
            bttRename = _RenameBtt.Margin;
        }

        // Update the Margins so the GroupBoxes never intersect
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Define the right margin for Original Names
            gbonMargin.Right = Application.Current.MainWindow.Width - _ReLabel.TransformToAncestor(
                        Application.Current.MainWindow).Transform(
                        new Point(0, 0)).X;
            // Define the bottom margin
            gbonMargin.Bottom = Application.Current.MainWindow.Height / 4 + 10;

            // Define the left margin for New Names
            gbnnMargin.Left = Application.Current.MainWindow.Width - _ReLabel.TransformToAncestor(
                        Application.Current.MainWindow).Transform(
                        new Point(0, 0)).X + _ReLabel.Width;
            // Define the bottom margin
            gbnnMargin.Bottom = gbonMargin.Bottom;

            // Define the top margin for the Rules area
            gbRulesMargin.Top = Application.Current.MainWindow.Height / 4 * 3 - 50;



            _GbOriginalName.Margin = gbonMargin;
            _GbNewNames.Margin = gbnnMargin;
            _GbRulesList.Margin = gbRulesMargin;
        }

        private bool CheckIfDirectory(string path) => 
            (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;

        private void GetFilesData(object sender, DragEventArgs e)
        {
            // If we're using rules don't allow file drop on nn
            if ((ListBox)sender == _NNList && _UseRulesCheck.IsChecked == true)
            {
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get all file paths on drop
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (paths != null)
                {
                    // Clear the previous added files
                    allfiles.Clear();

                    // Check if there's directory paths in the array
                    for (int i = 0; i < paths.Length; i++)
                    {
                        // Check if it's directory path
                        if (CheckIfDirectory(paths[i]))
                        {
                            // Add all the found files to the list
                            allfiles.AddRange(Directory.GetFiles(paths[i], "*.*", SearchOption.AllDirectories));
                        }
                        else
                        {
                            // Add the current file to the list
                            allfiles.Add(paths[i]);
                        }
                    }
                    SaveFileData(sender);
                }
            }
        }

        private void SaveFileData(object sender)
        {
            if ((ListBox)sender == _ONList)
            {
                onList.Clear();
                _ONList.Items.Clear();
            }
            else if ((ListBox)sender == _NNList)
            {
                nnList.Clear();
                _NNList.Items.Clear();
            }
            
            for (int i = 0; i < allfiles.Count; i++)
            {
                string fileName = System.IO.Path.GetFileName(allfiles[i]);

                // When we drop items that have no path we still want the name :)
                // This will be for later uses when we'll get strings from APIs
                if (fileName == null)
                {
                    fileName = allfiles[i];
                }

                // If the method was called from the original names list
                if ((ListBox)sender == _ONList)
                {
                    // Creates a new temporary file
                    FileName currentFile =
                        new FileName(
                            fileName.Remove(fileName.LastIndexOf('.')),
                            allfiles[i].Replace(fileName, ""),
                            fileName.Substring(fileName.LastIndexOf('.'))
                            );

                    onList.Add(currentFile);
                    _ONList.Items.Add(new { Name = currentFile.Name, Format = currentFile.Extention });

                    if (_ONList.Items.Count <= _NNList.Items.Count)
                    {
                        _NNList.Items[_ONList.Items.Count - 1] =
                            new {
                                Name = nnList[_ONList.Items.Count - 1],
                                Format = onList[_ONList.Items.Count - 1].Extention
                            };
                    }

                }
                // If the method was called from the new names list
                else if ((ListBox)sender == _NNList)
                {
                    fileName = fileName.Remove(fileName.LastIndexOf('.'));

                    nnList.Add(fileName);
                    if (_ONList.Items.Count > _NNList.Items.Count)
                    {
                        // Update fileFormat to get the matching format from the original item
                        _NNList.Items.Add(new { Name = fileName, Format = onList[_NNList.Items.Count].Extention });
                    }
                    else
                    {
                        _NNList.Items.Add(new { Name = fileName, Visibility = "Hidden" });
                    }
                }
            }
        }

        private void OnAddRule(object sender, RoutedEventArgs e)
        {
            rulesWindow = new AddRuleWindow();
            rulesWindow.Show();
        }

        /// <summary>
        /// Removes the currently selected rule from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveRule(object sender, RoutedEventArgs e)
        {
            _Rules.Items.Remove(_Rules.SelectedItem);
        }
    }
}
