using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReNam
{
    /// <summary>
    /// Interaction logic for AddRuleWindow.xaml
    /// </summary>
    public partial class AddRuleWindow : Window
    {
        private IRule curRule;
        public IRule CurRule
        {
            get => curRule;
        }

        private InsertPosition insertPos;

        public AddRuleWindow(IRule rule)
        {
            InitializeComponent();

            curRule = rule;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnAddRule(object sender, RoutedEventArgs e)
        {
            switch ((_RulesList.SelectedItem as TextBlock).Text)
            {
                default:
                case "Insert":
                    switch (insertPos)
                    {
                        case InsertPosition.Position:
                            curRule = new InsertRule(_InsertText.Text, insertPos,
                            (int)_InsertPositionNum.Value, _InsertPositionLRCheck.IsChecked ?? false);
                            break;
                        case InsertPosition.AfterText:
                            curRule = new InsertRule(_InsertText.Text, insertPos,
                            _InsertATTextBox.Text, _InsertPositionLRCheck.IsChecked ?? false);
                            break;
                        case InsertPosition.BeforeText:
                            curRule = new InsertRule(_InsertText.Text, insertPos,
                            _InsertBTTextBox.Text, _InsertPositionLRCheck.IsChecked ?? false);
                            break;
                        default:
                            curRule = new InsertRule(_InsertText.Text, insertPos);
                            break;
                    }

                    break;
                case "Delete":
                    break;
                case "Remove":
                    break;
                case "Replace":
                    break;
                case "Rearrange":
                    break;
                case "Extension":
                    break;
                case "Strip":
                    break;
                case "Case":
                    break;
                case "Serialize":
                    break;
                case "Padding":
                    break;
                case "Clean Up":
                    break;
                case "Reformat Date":
                    break;
                case "User Input":
                    break;
            }

            this.Close();
        }

        /// <summary>
        /// Converts the content of the checked RadioButton to a usable Enum
        /// </summary>
        /// <param name="sender">The Radio button that sent the event</param>
        /// <param name="e">The event</param>
        private void OnChecked(object sender, RoutedEventArgs e) =>
            Enum.TryParse<InsertPosition>(
                (sender as RadioButton).Content.ToString().Replace(" ", "").Replace(":", ""),
                out insertPos);
    }
}
