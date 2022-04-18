using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

        private ObservableCollection<FileName> onList;
        public ObservableCollection<FileName> ONList
        {
            get => onList;
            set => onList = value;
        }

        private ObservableCollection<FileName> nnList;
        public ObservableCollection<FileName> NNList
        {
            get => nnList;
            set => nnList = value;
        }

        private ObservableCollection<IRule> rulesList;
        public ObservableCollection<IRule> RulesList
        {
            get => rulesList;
            set => rulesList = value;
        }

        private List<string> allfiles;

        private AddRuleWindow rulesWindow;

        public MainWindow()
        {
            InitializeComponent();

            onList = new ObservableCollection<FileName>();
            nnList = new ObservableCollection<FileName>();
            rulesList = new ObservableCollection<IRule>();
            allfiles = new List<string>();

            // Initialize the margin variables
            gbonMargin = _GbOriginalName.Margin;
            gbnnMargin = _GbNewNames.Margin;
            gbRulesMargin = _GbRulesList.Margin;
            bttRename = _RenameBtt.Margin;

            _ONList.ItemsSource = ONList;
            _NNList.ItemsSource = NNList;
            _Rules.ItemsSource = RulesList;
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
            }
            else if ((ListBox)sender == _NNList)
            {
                nnList.Clear();
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

                // Creates a new temporary file
                FileName currentFile =
                    new FileName(
                        fileName.Remove(fileName.LastIndexOf('.')),
                        allfiles[i].Replace(fileName, ""),
                        fileName.Substring(fileName.LastIndexOf('.'))
                        );

                // If the method was called from the original names list
                if ((ListBox)sender == _ONList)
                {
                    ONList.Add(currentFile);

                    if (onList.Count <= nnList.Count)
                    {
                        NNList[onList.Count - 1].Visibility = "Visible";
                        NNList[onList.Count - 1].Format = onList[onList.Count - 1].Format;
                    }
                }
                // If the method was called from the new names list
                else if ((ListBox)sender == _NNList)
                {
                    NNList.Add(currentFile);
                    if (onList.Count >= nnList.Count)
                    {
                        
                        // Update fileFormat to get the matching format from the original item
                        NNList[nnList.Count - 1].Format = onList[nnList.Count - 1].Format;
                    }
                    else
                    {
                        NNList[nnList.Count - 1].Visibility = "Hidden";
                    }
                }
            }
        }

        private void OnAddRule(object sender, RoutedEventArgs e)
        {
            // Needs a check for when we're sending a rule for editing
            rulesWindow = new AddRuleWindow(null);

            rulesWindow.Show();

            rulesWindow.Closing += RuleAdded;
        }

        /// <summary>
        /// When the add rule window closes we check if a rule was created
        /// </summary>
        /// <param name="sender">The object that sent the event</param>
        /// <param name="e">The event</param>
        public void RuleAdded(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (rulesWindow.CurRule != null)
            {
                // If a rule was created add it to the list and set it's ID
                rulesWindow.CurRule.ID = RulesList.Count + 1;
                RulesList.Add(rulesWindow.CurRule);
            }
            
        }

        /// <summary>
        /// Removes the currently selected rule from the list
        /// </summary>
        /// <param name="sender">The object that sent the event</param>
        /// <param name="e">The event</param>
        private void OnRemoveRule(object sender, RoutedEventArgs e)
        {
            _Rules.Items.Remove(_Rules.SelectedItem);
        }
    }
}
