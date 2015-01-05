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

using System.IO;
using System.Windows.Forms;

namespace SortMyVids
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            uiResearchControl.uiButtonLaunch.Click += uiButtonLaunch_Click;
        }

        void uiButtonLaunch_Click(object sender, RoutedEventArgs e)
        {
            List<VideoFile> listTmp = new List<VideoFile>();
            
            foreach(VideoFile v in uiResearchControl.ListMyVideos)
            {
                v.searchGenre();

            }

            foreach (VideoFile v in uiResearchControl.ListMyVideos)
            {
                if (!v.IsVerified)
                    listTmp.Add(v);
            }

            uiUnknownVideosControl.ListUnknownVideo = listTmp;
        }

    }
}
