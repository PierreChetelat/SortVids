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
    /// Logique d'interaction pour ParameterName.xaml
    /// </summary>
    public partial class ParameterName : Window
    {
        MainWindow owner;
        List<String> listFilterName;

        public List<String> ListFilterName
        {
            get { return listFilterName; }
            set { listFilterName = value; }
        }

        public ParameterName(MainWindow parent)
        {
            InitializeComponent();
            owner = parent;

            listFilterName = owner.uiResearchControl.ListNameMediaFilter;

            foreach(string s in listFilterName)
            {
                uiListViewName.Items.Add(s);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if(uiListViewName.SelectedItem != null)
            {
                string strToDelete = uiListViewName.SelectedItem as String;
                if(strToDelete != null)
                {
                    uiListViewName.Items.Remove(strToDelete);
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            listFilterName.Clear();
            for (int i = 0; i < uiListViewName.Items.Count; i++)
            {
                string item = uiListViewName.Items.GetItemAt(i) as string;
                if (item != null)
                {
                    listFilterName.Add(item);
                }
            }

            owner.uiResearchControl.ListNameMediaFilter = listFilterName;

            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string strToAdd = uiTextBox.Text;
            if (strToAdd != "")
            {
                uiListViewName.Items.Insert(0,strToAdd);
            }
            uiTextBox.Text = "";
        }

    }
}
