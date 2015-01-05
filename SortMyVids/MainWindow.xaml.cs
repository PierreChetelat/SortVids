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
using System.ComponentModel;

namespace SortMyVids
{
    public partial class MainWindow : Window
    {
        List<VideoFile> listTmp = new List<VideoFile>();

        public MainWindow()
        {
            InitializeComponent();

            uiResearchControl.uiButtonLaunch.Click += uiButtonLaunch_Click;
        }

        void uiButtonLaunch_Click(object sender, RoutedEventArgs e)
        {
            listTmp = new List<VideoFile>();
       
            BackgroundWorker bw = new BackgroundWorker();

            // define the event handlers, work in other thread
            bw.DoWork += (objesender, args) => { getGenre(); };
            bw.RunWorkerCompleted += (objesender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                }
                //Work in UI THREAD
                else
                {
                    uiUnknownVideosControl.ListUnknownVideo = listTmp;
                }
            };

            
            bw.RunWorkerAsync();


        }

        public void getGenre()
        {
            Console.WriteLine("1");
            foreach(VideoFile v in uiResearchControl.ListMyVideos)
            {
                v.searchGenre();
            }
            Console.WriteLine("2");

            foreach (VideoFile v in uiResearchControl.ListMyVideos)
            {
                if (!v.IsVerified)
                    listTmp.Add(v);
            }
            Console.WriteLine("3");
        }
    }
}
