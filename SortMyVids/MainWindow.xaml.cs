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

        private BackgroundWorker backWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            
            //For suppress the error from treeview because adding VideoFile instead of treeviewitem
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            //Add event to buttons Analysis and Launch Sort
            uiResearchControl.uiButtonLaunchAnalysis.Click += uiButtonLaunchAnalysis_Click;
            uiUnknownVideosControl.uiButtonLaunchExecution.Click += uiButtonLaunchExecution_Click;
        }

        void uiButtonLaunchExecution_Click(object sender, RoutedEventArgs e)
        {
            if (uiResearchControl.uiFolderDest.Text != "Dossier des films triés")
            {
                uiUnknownVideosControl.uiButtonLaunchExecution.Content = "Veuillez patienter";
                string folderDest = uiResearchControl.uiFolderDest.Text;
                List<VideoFile> listToMove = uiUnknownVideosControl.ListVerifiedMyVideos;

                uiUnknownVideosControl.uiProgressBarMove.Maximum = listToMove.Count;
                int progress = 0;

                foreach(VideoFile v in listToMove)
                {
                    string destinationVideo = folderDest + "\\" + v.Genre.ToString();
                    if (!System.IO.Directory.Exists(destinationVideo))
                    {
                        System.IO.Directory.CreateDirectory(destinationVideo);
                    }
                    string destinationFile = destinationVideo + "\\" + v.VideoName + v.VideoExtension;
                    System.IO.Directory.Move(v.OldPathName, destinationFile);
                    progress++;

                    uiUnknownVideosControl.uiProgressBarMove.Value = progress;
                }

                uiResearchControl.ListMyVideos.Clear();
                uiUnknownVideosControl.ListVerifiedMyVideos.Clear();
                uiUnknownVideosControl.ListUnknownVideo.Clear();

                uiUnknownVideosControl.uiButtonLaunchExecution.Content = "Trier mes films";
            }
            else
            {
                uiTabControl.SelectedIndex = 0;

                uiResearchControl.askForFolderDest();
            }
        }

        void uiButtonLaunchAnalysis_Click(object sender, RoutedEventArgs e)
        {

            if (uiResearchControl.ListMyVideos.Count > 0 && backWorker.IsBusy == false)
            {
                uiResearchControl.uiButtonLaunchAnalysis.Content = "Annuler";
                // define the event handlers, work in other thread
                backWorker.DoWork += worker_DoWork;
                backWorker.WorkerReportsProgress = true;
                backWorker.WorkerSupportsCancellation = true;
                
                uiResearchControl.uiProgressBarAnalyse.Maximum = uiResearchControl.ListMyVideos.Count;

                backWorker.ProgressChanged += bw_ProgressChanged;
                backWorker.RunWorkerCompleted += (objesender, args) =>
                {
                    //Work in UI THREAD
                    if (args.Error != null)
                    {
                        uiResearchControl.uiLabelNBVideo.Content = "Un problème est survenue pendant la recherche, recommencer";
                    }
                    else if(args.Cancelled)
                    {
                        uiResearchControl.uiProgressBarAnalyse.Value = 0;
                    }
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

                            uiUnknownVideosControl.uiListUnknownVideo.SelectedIndex = 0;
                            uiTabControl.SelectedIndex = 1;
                        }
                    }
                };

                backWorker.RunWorkerAsync(uiResearchControl.ListMyVideos.ToList());
            }
            else if (backWorker.IsBusy == true)
            {
                backWorker.CancelAsync();

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
                if (bw.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

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
            ParameterExtension windowExtension = new ParameterExtension(this);
            windowExtension.ShowDialog();
        }

        private void MenuItemName_Click(object sender, RoutedEventArgs e)
        {
            ParameterName windowName = new ParameterName(this);
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
