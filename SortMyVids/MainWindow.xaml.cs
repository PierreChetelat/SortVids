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

        /*
         * Le clic sur le bouton de trie des films instancie un backgroundworker, et lui fournit la liste trié ainsi que le dossier de destination
         * */
        void uiButtonLaunchExecution_Click(object sender, RoutedEventArgs e)
        {
            if (uiResearchControl.uiFolderDest.Text != "Dossier des films triés" && backWorker.IsBusy == false)
            {
                backWorker = new BackgroundWorker();
                backWorker.DoWork += worker_DoSortVideo;
                backWorker.WorkerReportsProgress = true;
                backWorker.WorkerSupportsCancellation = true;
                backWorker.ProgressChanged += worker_ProgressChangedSortVideo;
                backWorker.RunWorkerCompleted += (objesender, args) =>
                {
                    if(args.Error != null)
                    {
                        System.Windows.MessageBox.Show(this, "Une erreur s'est produite pendant la copie !", "Information");
                    }
                    else if(args.Cancelled)
                    {
                        System.Windows.MessageBox.Show(this, "Vous avez annulé l'opération !", "Information");
                    }
                    else
                    {
                        uiResearchControl.ListMyVideos.Clear();
                        uiUnknownVideosControl.ListVerifiedMyVideos.Clear();
                        uiUnknownVideosControl.ListUnknownVideo.Clear();
                        uiUnknownVideosControl.uiButtonLaunchExecution.Content = "Trier mes films !!!";
                        uiUnknownVideosControl.uiButtonLaunchExecution.IsEnabled = false;

                        System.Windows.MessageBox.Show(this, "Tous les films ont été triés !", "Information");
                        uiUnknownVideosControl.uiProgressBarMove.Value = 0;
                    }
                };

                uiUnknownVideosControl.uiButtonLaunchExecution.Content = "Veuillez patienter";

                ListToBackWorker listAndFolder = new ListToBackWorker();
                listAndFolder.DestinationFolder = uiResearchControl.uiFolderDest.Text;
                listAndFolder.ListToMove = uiUnknownVideosControl.ListVerifiedMyVideos.ToList();

                uiUnknownVideosControl.uiProgressBarMove.Maximum = listAndFolder.ListToMove.Count;

                backWorker.RunWorkerAsync(listAndFolder);
            }
            else if(backWorker.IsBusy)
            {
                backWorker.CancelAsync();
            }
            else
            {
                uiTabControl.SelectedIndex = 0;

                uiResearchControl.askForFolderDest();
            }
        }

        /*
         * Méthode du backgroundworker de trie des vidéos
         * */
        private void worker_DoSortVideo(object sender, DoWorkEventArgs e)
        {
            ListToBackWorker listAndFolder = e.Argument as ListToBackWorker;

            string folderDest = listAndFolder.DestinationFolder;
            List<VideoFile> listToMove = listAndFolder.ListToMove;

            BackgroundWorker bw = sender as BackgroundWorker;
            int progress = 0;

            foreach (VideoFile v in listToMove)
            {
                try
                {
                    string destinationVideo = System.IO.Path.Combine(folderDest, v.Genre.ToString());
                    if (!System.IO.Directory.Exists(destinationVideo))
                    {
                        System.IO.Directory.CreateDirectory(destinationVideo);
                    }
                    string destinationFile = System.IO.Path.Combine(destinationVideo, (v.ToString() + v.VideoExtension));
                    System.IO.Directory.Move(v.OldPathName, destinationFile);

                }
                catch (System.NotSupportedException)
                {
                }
                progress++;

                bw.ReportProgress(progress);
            }
        }

        void worker_ProgressChangedSortVideo(object sender, ProgressChangedEventArgs e)
        {
            uiUnknownVideosControl.uiProgressBarMove.Value = (double)e.ProgressPercentage;
        }


        /*
         * Un clic sur le bouton analyser lance un backgroundworker et lance les recherches sur internet
         * */
        void uiButtonLaunchAnalysis_Click(object sender, RoutedEventArgs e)
        {

            if (uiResearchControl.ListMyVideos.Count > 0 && backWorker.IsBusy == false)
            {
                uiResearchControl.uiButtonLaunchAnalysis.Content = "Annuler";
                // define the event handlers, work in other thread

                backWorker = new BackgroundWorker();
                backWorker.DoWork += worker_DoSearchVideo;
                backWorker.WorkerReportsProgress = true;
                backWorker.WorkerSupportsCancellation = true;
                
                uiResearchControl.uiProgressBarAnalyse.Maximum = uiResearchControl.ListMyVideos.Count;

                backWorker.ProgressChanged += worker_ProgressChangedSearchVideo;
                backWorker.RunWorkerCompleted += (objesender, args) =>
                {
                    //Work in UI THREAD
                    if (args.Error != null)
                    {
                        System.Windows.MessageBox.Show(this, "Une erreur s'est produite pendant la recherche !", "Information");
                    }
                    else if(args.Cancelled)
                    {
                        System.Windows.MessageBox.Show(this, "Vous avez annulé l'opération de recherche !", "Information");                        
                    }
                    else
                    {
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

                    uiResearchControl.uiButtonLaunchAnalysis.Content = "Analyser";
                    uiResearchControl.uiProgressBarAnalyse.Value = 0;
                };

                backWorker.RunWorkerAsync(uiResearchControl.ListMyVideos.ToList());
            }
            else if (backWorker.IsBusy == true)
            {
                backWorker.CancelAsync();
            }

        }

        void worker_ProgressChangedSearchVideo(object sender, ProgressChangedEventArgs e)
        {
            uiResearchControl.uiProgressBarAnalyse.Value = (double)e.ProgressPercentage;
        }

        private void worker_DoSearchVideo(object sender, DoWorkEventArgs e)
        {
            List<VideoFile> list = e.Argument as List<VideoFile>;

            ListsFromBackWorker listsResul = new ListsFromBackWorker();

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

        //Fenetres de configuration de nom et d'extension
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

    //Classe utilisé pour passer et recevoir des informations du backgroundworker
    public class ListToBackWorker
    {
        List<VideoFile> listToMove;
        string destinationFolder;

        internal List<VideoFile> ListToMove
        {
            get { return listToMove; }
            set { listToMove = value; }
        }
  

        public string DestinationFolder
        {
            get { return destinationFolder; }
            set { destinationFolder = value; }
        }

        public ListToBackWorker()
        {
            listToMove = new List<VideoFile>();
            destinationFolder = "";
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
