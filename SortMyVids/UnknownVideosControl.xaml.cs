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
            set { listUnknownVideo = value;
                  //Binding view - list
                  uiListUnknownVideo.ItemsSource = listUnknownVideo;
                  uiListUnknownVideo.SelectedItem = uiListUnknownVideo.Items.GetItemAt(0);
                }
        }
    
        public UnknownVideosControl()
        {
            InitializeComponent();

            listUnknownVideo = new List<VideoFile>();

            //Choix de base
            uiRadioChoice.IsChecked = true;

            //initComboBoxChoice();
            initComboBoxEdit();

            
        }
        
        private void initComboBoxChoice(VideoFile v)
        {
            uiComboBoxChoice.Items.Clear();

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

        private void initComboBoxEdit(VideoFile v)
        {
            if(v.IsVerified)
            {
                uiTextTitle.Text = v.VideoName;
                uiComboBoxEdit.SelectedItem = v.Genre;
                uiTextYear.Text = v.VideoYear;
            }
        }

        private void uiRadio_Checked(object sender, RoutedEventArgs e)
        {
            if(uiRadioEdit.IsChecked == true)
            {
                uiComboBoxChoice.IsEnabled = false;
                uiButtonValidateChoice.IsEnabled = false;
                uiTextTitle.IsEnabled = true;
                uiComboBoxEdit.IsEnabled = true;
                uiButtonValidateEdit.IsEnabled = true;
            }
            else
            {
                uiTextTitle.IsEnabled = false;
                uiComboBoxEdit.IsEnabled = false;
                uiButtonValidateEdit.IsEnabled = false;
                uiComboBoxChoice.IsEnabled = true;
                uiButtonValidateChoice.IsEnabled = true;
            }
        }

        private void uiListUnknownVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoFile v = uiListUnknownVideo.SelectedItem as VideoFile;

            if (v != null)
            {
                initComboBoxChoice(v);
                initComboBoxEdit(v);
            }
        }

        private void uiButtonValidateChoice_Click(object sender, RoutedEventArgs e)
        {
            VideoFile v = getSelectedVideoFile();
            VideoFile vNew = uiComboBoxChoice.SelectedItem as VideoFile;

            if(vNew != null)
            {
                v.VideoName = vNew.VideoName;
                v.VideoYear = vNew.VideoYear;
                v.IsVerified = true;
                nextSelectedUnknownItem();
            } 
        }

        private void uiButtonValidateEdit_Click(object sender, RoutedEventArgs e)
        {
            VideoFile v = getSelectedVideoFile();

            string title = uiTextTitle.Text;
            string year = uiTextTitle.Text;

            if (title.Length > 0) 
            { 
                v.VideoName = uiTextTitle.Text;
                v.VideoYear = uiTextYear.Text;
                v.IsVerified = true;
                nextSelectedUnknownItem();
            }
        }    

        private VideoFile getSelectedVideoFile()
        {
            int index = listUnknownVideo.IndexOf(uiListUnknownVideo.SelectedItem as VideoFile);
            return listUnknownVideo.ElementAt(index);
        }

        private void nextSelectedUnknownItem()
        {
            uiListUnknownVideo.SelectedIndex = uiListUnknownVideo.SelectedIndex + 1; 
            clearTextEdit();
            



            //?????????????????
            uiListUnknownVideo.UpdateLayout();
        }

        private void clearTextEdit()
        {
            uiTextTitle.Clear();
            uiTextYear.Clear();
        }
    }
}
