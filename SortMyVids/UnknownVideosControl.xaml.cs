using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SortMyVids
{
    public partial class UnknownVideosControl : UserControl
    {
        private List<VideoFile> listUnknownVideo;

        internal List<VideoFile> ListUnknownVideo
        {
            get { return listUnknownVideo; }
            set { listUnknownVideo = value; }
        }
    
        public UnknownVideosControl()
        {
            InitializeComponent();

            listUnknownVideo = new List<VideoFile>();
            //Binding view - list
            uiListUnknownVideo.ItemsSource = listUnknownVideo;

            //Choix de base
            uiRadioChoice.IsChecked = true;

            //initComboBoxChoice();
            initComboBoxEdit();
        }
        
        private void initComboBoxChoice(VideoFile v)
        {
            foreach (VideoFile tmp in v.PresumeVideo)
            {
                uiComboBoxChoice.Items.Add(tmp);
            }
        }

        private void initComboBoxEdit()
        {
            foreach (TypeMovie t in Enum.GetValues(typeof(TypeMovie)) as TypeMovie[])
            {
                uiComboBoxEdit.Items.Add(t);
            }
        }

        private void fillUnknownVideo()
        {
            foreach(VideoFile v in listUnknownVideo)
            {
                if(!v.IsVerified)
                    uiListUnknownVideo.Items.Add(v);
            }
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

        private void uiListUnknownVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoFile v = sender as VideoFile;
            if (v != null)
            {
            }
        }
    
    }
}
