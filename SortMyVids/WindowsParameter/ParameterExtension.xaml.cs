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
using System.Windows.Shapes;

namespace SortMyVids.WindowsParameter
{
    /// <summary>
    /// Logique d'interaction pour ParameterExtension.xaml
    /// </summary>
    public partial class ParameterExtension : Window
    {
        MainWindow owner;
        List<String> listFilterExtension;

        public List<String> ListFilterExtension
        {
            get { return listFilterExtension; }
            set { listFilterExtension = value; }
        }

        public ParameterExtension(MainWindow parent)
        {
            InitializeComponent();
            owner = parent;
            listFilterExtension = owner.uiResearchControl.ListExtensionMediaFilter;
           
            foreach(string s in listFilterExtension)
            {
                uiListViewExtension.Items.Add(s);
            }
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (uiListViewExtension.SelectedItem != null)
            {
                string strToDelete = uiListViewExtension.SelectedItem as String;
                if (strToDelete != null) 
                { 
                    uiListViewExtension.Items.Remove(strToDelete);
                }
            }
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            listFilterExtension.Clear();
            for (int i = 0; i < uiListViewExtension.Items.Count; i++)
            {
                string item = uiListViewExtension.Items.GetItemAt(i) as string;
                if (item != null)
                {
                    listFilterExtension.Add(item);
                }
            }

            owner.uiResearchControl.ListExtensionMediaFilter = listFilterExtension;

            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string strToAdd = uiTextBox.Text;
            if(strToAdd != "")
            {
                if(strToAdd[0] != '.')
                {
                    strToAdd = '.' + strToAdd;
                }
                uiListViewExtension.Items.Insert(0,strToAdd);
                uiTextBox.Text = "";
            }
        }
    }
}
