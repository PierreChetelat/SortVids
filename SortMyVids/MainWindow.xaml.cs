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
using SortMyVids.WindowsParameter;

namespace SortMyVids
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
            //For suppress the error from treeview because adding VideoFile instead of treeviewitem
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            uiResearchControl.uiButtonLaunchAnalysis.Click += uiButtonLaunchAnalysis_Click;
            uiUnknownVideosControl.uiButtonLaunchExecution.Click += uiButtonLaunchExecution_Click;
        }

        void uiButtonLaunchExecution_Click(object sender, RoutedEventArgs e)
        {
            if(uiResearchControl.uiFolderDest.Text != "")
            {
                string folderDest = uiResearchControl.uiFolderDest.Text;
                List<VideoFile> listToMove = uiUnknownVideosControl.ListVerifiedMyVideos;
                foreach(VideoFile v in listToMove)
                {
                    string destinationVideo = folderDest + "\\" + v.Genre.ToString();
                    if (!System.IO.Directory.Exists(destinationVideo))
                    {
                        System.IO.Directory.CreateDirectory(destinationVideo);
                    }
                    string destinationFile = destinationVideo + "\\" + v.VideoName + "." + v.VideoExtension;
                    System.IO.Directory.Move(v.OldPathName, destinationFile);
                }
            }
            else
            {
                uiResearchControl.uiFolderDest.Text = "Indiquer un dossier de destination";
            }
        }

        void uiButtonLaunchAnalysis_Click(object sender, RoutedEventArgs e)
        {
         
            BackgroundWorker bw = new BackgroundWorker();
            
            if(uiResearchControl.ListMyVideos.Count > 0 && bw.IsBusy == false)
            {
                uiResearchControl.uiButtonLaunchAnalysis.Content = "Annuler";
                // define the event handlers, work in other thread
                bw.DoWork += worker_DoWork;
                bw.WorkerReportsProgress = true;
                //TODO : ARRETER LE WORKER
                bw.WorkerSupportsCancellation = true;
                
                uiResearchControl.uiProgressBarAnalyse.Maximum = uiResearchControl.ListMyVideos.Count;

                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerCompleted += (objesender, args) =>
                {
                    if (args.Error != null)  // if an exception occurred during DoWork
                    {
                        uiResearchControl.uiLabelNBVideo.Content = "Un problème est survenue pendant la recherche, recommencer";
                    }
                    //Work in UI THREAD
                    else
                    {
                        uiResearchControl.uiButtonLaunchAnalysis.Content = "Analyser";

                        ListsFromBackWorker listResult = args.Result as ListsFromBackWorker;
                        if (listResult != null)
                        {
                            uiUnknownVideosControl.ListUnknownVideo = listResult.ListUnverified;
                            uiUnknownVideosControl.ListVerifiedMyVideos = listResult.ListVerified;

                            uiUnknownVideosControl.fillTreeView();

                            uiUnknownVideosControl.uiButtonLaunchExecution.IsEnabled = true;
                        }
                    }
                };

                bw.RunWorkerAsync(uiResearchControl.ListMyVideos.ToList());
            }
            else if(bw.IsBusy == true)
            {
                //??
                //bw.CancellationPending = true;

                uiResearchControl.uiButtonLaunchAnalysis.Content = "Analyser";
            }

        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            uiResearchControl.uiProgressBarAnalyse.Value = (double)e.ProgressPercentage;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<VideoFile> list = e.Argument as List<VideoFile>;

            ListsFromBackWorker listsResul = new ListsFromBackWorker();

            //To indicate progress
            BackgroundWorker bw = sender as BackgroundWorker;
            int progress = 0;

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
                progress++;


                bw.ReportProgress(progress);
            }

            e.Result = listsResul;
        }

        private List<TreeViewItem> getTreeViewItemGenre()
        {
            List<TreeViewItem> listGenre = new List<TreeViewItem>();

            foreach (TypeMovie t in Enum.GetValues(typeof(TypeMovie)) as TypeMovie[])
            {
                TreeViewItem tmp = new TreeViewItem();
                tmp.Header = t.ToString();
                listGenre.Add(tmp);
            }

            return listGenre;
        }

        private void MenuItemExtension_Click(object sender, RoutedEventArgs e)
        {
            ParameterExtension windowExtension = new ParameterExtension(this,uiResearchControl.ListExtensionMediaFilter);
            windowExtension.ShowDialog();
        }

        private void MenuItemName_Click(object sender, RoutedEventArgs e)
        {
            ParameterName windowName = new ParameterName(this,uiResearchControl.ListNameMediaFilter);
            windowName.ShowDialog();
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
