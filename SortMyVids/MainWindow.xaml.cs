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

        public MainWindow()
        {
            InitializeComponent();

            uiResearchControl.uiButtonLaunchAnalysis.Click += uiButtonLaunchAnalysis_Click;
            uiResearchControl.uiButtonLaunchExecution.Click += uiButtonLaunchExecution_Click;
        }

        void uiButtonLaunchExecution_Click(object sender, RoutedEventArgs e)
        {

        }

        void uiButtonLaunchAnalysis_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();

            // define the event handlers, work in other thread
            bw.DoWork += worker_DoWork;
            bw.RunWorkerCompleted += (objesender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork
                {
                    //TODO : MESSAGE ERREUR
                }
                //Work in UI THREAD
                else
                {
                    ListsFromBackWorker listResult = args.Result as ListsFromBackWorker;
                    if (listResult != null)
                    {
                        uiUnknownVideosControl.ListUnknownVideo = listResult.ListUnverified;
                        uiResearchControl.uiListSortMovie.ItemsSource = listResult.ListVerified;
                        uiResearchControl.uiButtonLaunchExecution.IsEnabled = true;
                    }
                }
            };

            
            bw.RunWorkerAsync(uiResearchControl.ListMyVideos.ToList());

        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<VideoFile> list = e.Argument as List<VideoFile>;

            ListsFromBackWorker listsResul = new ListsFromBackWorker();

            foreach (VideoFile v in list)
            {
                v.searchPresumeVideo();

                if (v.IsVerified) 
                {
                    listsResul.ListVerified.Add(v);
                }
                else
                {
                    listsResul.ListUnverified.Add(v);
                }
            }

            e.Result = listsResul;
        }
    }

    public class ListsFromBackWorker
    {
        List<VideoFile> listUnverified;
        List<VideoFile> listVerified;

        internal List<VideoFile> ListUnverified
        {
            get { return listUnverified; }
            set { listUnverified = value; }
        }
        internal List<VideoFile> ListVerified
        {
            get { return listVerified; }
            set { listVerified = value; }
        }


        public ListsFromBackWorker()
        {
            listUnverified = new List<VideoFile>();
            listVerified = new List<VideoFile>();
        }
    }
}
