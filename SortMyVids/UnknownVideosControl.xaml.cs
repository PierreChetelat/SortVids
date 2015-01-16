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
    public partial class SortVideosControl : UserControl
    {
        private List<VideoFile> listUnknownVideo;

        internal List<VideoFile> ListUnknownVideo
        {
            get { return listUnknownVideo; }
            set { listUnknownVideo = value;
                  
                    if(listUnknownVideo.Count > 0)
                    { 
                        foreach(VideoFile v in listUnknownVideo)
                        { 
                            uiListUnknownVideo.Items.Add(v);
                        }
                        uiListUnknownVideo.SelectedItem = uiListUnknownVideo.Items.GetItemAt(0);
                    }
                }
        }

        List<VideoFile> listVerifiedMyVideos = new List<VideoFile>();
        internal List<VideoFile> ListVerifiedMyVideos
        {
            get { return listVerifiedMyVideos; }
            set { listVerifiedMyVideos = value; }
        }
    
        public SortVideosControl()
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

            foreach (VideoFile tmp in v.ListPresumeVideo)
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

        private void uiComboBoxChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoFile v = uiComboBoxChoice.SelectedItem as VideoFile;
            if(v != null)
            {
                foreach(TypeMovie enumMovie in v.ListPresumeGenre)
                {
                    uiComboBoxChoiceGenre.Items.Add(enumMovie);
                }
            }
        }

        private void uiRadio_Checked(object sender, RoutedEventArgs e)
        {
            if(uiRadioEdit.IsChecked == true)
            {
                uiBoderChoice.IsEnabled = false;
                uiBorderEdit.IsEnabled = true;
            }
            else
            {
                uiBorderEdit.IsEnabled = false;
                uiBoderChoice.IsEnabled = true;
            }
        }

        private void uiListUnknownVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TreeViewItem tmpDeselect = uiTreeSortMovie.SelectedItem as TreeViewItem;
            if(tmpDeselect != null)
            {
                tmpDeselect.IsSelected = false;
            }

            clearTextEdit();
            doSelectedUnknownList();
        }

        private void doSelectedUnknownList()
        {
            VideoFile v = uiListUnknownVideo.SelectedItem as VideoFile;

            if (v != null)
            {
                if(v.ListPresumeVideo.Count > 0)
                {
                    uiRadioChoice.IsEnabled = true;
                }
                else
                {
                    uiRadioEdit.IsEnabled = true;
                }

                initComboBoxChoice(v);
                initComboBoxEdit(v);
            }
        }

        private void uiButtonValidateChoice_Click(object sender, RoutedEventArgs e)
        {
            VideoFile v = getSelectedVideoFile();
            if(v != null)
            {
                VideoFile vNew = uiComboBoxChoice.SelectedItem as VideoFile;

                if(vNew != null)
                {
                    //RAJOUTER GENRE
                    v.VideoName = vNew.VideoName;
                    v.VideoYear = vNew.VideoYear;
                    v.IsVerified = true;
                    nextSelectedUnknownItem(v);
                }

            }
        }

        private void uiButtonValidateEdit_Click(object sender, RoutedEventArgs e)
        {
            VideoFile v = getSelectedVideoFile();

            if(v != null)
            {
                string title = uiTextTitle.Text;
                string year = uiTextTitle.Text;
                TypeMovie t = (TypeMovie)uiComboBoxEdit.SelectedItem;

                if (title.Length > 0 && t != null)
                {
                    //RAJOUTER GENRE
                    v.VideoName = uiTextTitle.Text;
                    v.VideoYear = uiTextYear.Text;
                    v.Genre = t;
                    v.IsVerified = true;
                    nextSelectedUnknownItem(v);
                }
            }

        }    

        private VideoFile getSelectedVideoFile()
        {
            VideoFile v = uiListUnknownVideo.SelectedItem as VideoFile;
            if(v != null)
            {
                int index = listUnknownVideo.IndexOf(v);

                if (index != -1) { 
                    return listUnknownVideo.ElementAt(index);
                }
            }
           
            
            v = uiTreeSortMovie.SelectedItem as VideoFile;
            if (v != null)
            {
                int index = listVerifiedMyVideos.IndexOf(v);

                if (index != -1)
                {
                    return listVerifiedMyVideos.ElementAt(index);
                }
            }
            

            return null;
        }

        private void nextSelectedUnknownItem(VideoFile v)
        {
            if(uiListUnknownVideo.SelectedItem != null)
            { 
                listVerifiedMyVideos.Add(v);
                int index = uiListUnknownVideo.SelectedIndex;
                uiListUnknownVideo.SelectedIndex = uiListUnknownVideo.SelectedIndex + 1;
                uiListUnknownVideo.Items.RemoveAt(index);
            }

            clearTextEdit();
            fillTreeView();
            //?????????????????
            //uiListUnknownVideo.UpdateLayout();
        }

        private void clearTextEdit()
        {
            uiTextTitle.Clear();
            uiTextYear.Clear();
            uiComboBoxChoice.Items.Clear();
            uiComboBoxChoiceGenre.Items.Clear();
        }

        public void fillTreeView()
        {
            List<TreeViewItem> listGenre = new List<TreeViewItem>();

            foreach (VideoFile v in listVerifiedMyVideos)
            {
                TreeViewItem tmp = new TreeViewItem();
                tmp.Header = tmp.Name = v.Genre.ToString();
                TreeViewItem tmp2 = listGenre.Find(x => x.Name == tmp.Name);
                if (tmp2 != null)
                {
                    tmp2.Items.Add(v);
                }
                else
                {
                    tmp.Items.Add(v);
                    listGenre.Add(tmp);
                }
            }

            //uiTreeSortMovie.Items.Clear();
            uiTreeSortMovie.ItemsSource = listGenre;
        }

        private void uiTreeSortMovie_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Console.WriteLine("SELECTED TREEVIEW");
            //Deselect item from listview
            uiListUnknownVideo.SelectedItem = null;
            uiListUnknownVideo.UpdateLayout();

            VideoFile v = uiTreeSortMovie.SelectedItem as VideoFile;

            if (v != null)
            {
                initComboBoxChoice(v);
                initComboBoxEdit(v);
            }
        }

    }
}
