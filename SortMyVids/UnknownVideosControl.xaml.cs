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


namespace SortMyVids
{
    /// <summary>
    /// Logique d'interaction pour UnknownVideosControl.xaml
    /// </summary>
    public partial class UnknownVideosControl : UserControl
    {
        public UnknownVideosControl()
        {
            InitializeComponent();

            uiRadioChoice.IsChecked = true;

            initComboBoxEdit();
        }

        private void initComboBoxEdit()
        {

        }

        private void uiRadio_Checked(object sender, RoutedEventArgs e)
        {
            if(uiRadioEdit.IsChecked == true)
            {
                uiComboBoxChoice.IsEnabled = false;
                uiTextTitle.IsEnabled = true;
                uiComboBoxEdit.IsEnabled = true;
            }
            else
            {
                uiTextTitle.IsEnabled = false;
                uiComboBoxEdit.IsEnabled = false;
                uiComboBoxChoice.IsEnabled = true;
            }
        }
    
    }
}
