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
using System.Threading;
using System.ComponentModel;

namespace SortMyVids
{
    
    public partial class ResearchControl : System.Windows.Controls.UserControl
    {

        static string[] mediaExtensions = {
            ".AVI", ".MP4", ".DIVX", ".WMV", ".MKV"
        };

        static string[] mediaName = {
            "720p","TRUEFRENCH","DVDRip","XviD","DTS","UTT"
        };

        static List<string> extensionMediaFilter = new List<string>();
        static List<string> nameMediaFilter = new List<string>();

        public static List<string> NameMediaFilter
        {
            get { return ResearchControl.nameMediaFilter; }
        }

        List<VideoFile> listMyVideos = new List<VideoFile>();
        

        internal List<VideoFile> ListMyVideos
        {
            get { return listMyVideos; }
            set { listMyVideos = value; }
        }


        string directorySrc, directoryDest;

        public ResearchControl()
        {
            InitializeComponent();
            updateFilters();
        }

        private void uiButtonSrc_Click(object sender, RoutedEventArgs e)
        {
            directorySrc = showAndGetFolder();
            uiFolderSrc.Text = directorySrc;

            if(directorySrc != "Choisir un dossier")
            {
                BackgroundWorker bw = new BackgroundWorker();

                // define the event handlers, work in other thread
                bw.DoWork += (objesender, args) => { getVideoFile(); };
                bw.RunWorkerCompleted += (objesender, args) =>
                {
                    if (args.Error != null)  // if an exception occurred during DoWork,
                    {
                        uiLabelNBVideo.Content = "Erreur lors de la recherche de vidéos";
                    }
                    //Work in UI THREAD
                    else
                    {
                        fillListVideoFile();
                        uiLabelNBVideo.Content = listMyVideos.Count + " vidéo trouvés";
                    }
                };

                uiLabelNBVideo.Content = "Recherche de vidéos...";

                bw.RunWorkerAsync();
            }
        }

        private void uiButtonDest_Click(object sender, RoutedEventArgs e)
        {
            directoryDest = showAndGetFolder();
            uiFolderDest.Text = directoryDest;
        }

        private String showAndGetFolder()
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();

            fBD.ShowNewFolderButton = false;
            fBD.RootFolder = System.Environment.SpecialFolder.MyComputer;

            if (fBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return fBD.SelectedPath;
            }

            return "Choisir un dossier";
        }

        private void getVideoFile()
        {
            if(directorySrc != null)
            { 
                foreach (string path in Directory.GetFiles(directorySrc, "*", SearchOption.AllDirectories))
                {
                    if (Array.IndexOf(extensionMediaFilter.ToArray(), System.IO.Path.GetExtension(path).ToUpperInvariant()) != -1)
                    {
                        VideoFile v = new VideoFile();
                        v.setPath(path);
                        listMyVideos.Add(v);
                    }
                }
            }
        }

        private void fillListVideoFile()
        {
            foreach(VideoFile v in listMyVideos)
            {
                uiListMovie.Items.Add(v);
            }
        }

        private void uiButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            updateFilters();
        }

        private void updateFilters()
        {
            string line;
            //Lecture des filtres de noms
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("./nameFilters.txt");

                while ((line = file.ReadLine()) != null)
                {
                    nameMediaFilter.Add(line);
                }

                file.Close();
            }
            catch
            {
                //Valeurs par défauts
                foreach (string s in mediaName)
                    nameMediaFilter.Add(s);
            }

            //Lecture des filtres d'extensions
            try
            {

                System.IO.StreamReader file = new System.IO.StreamReader("./extensionsFilters.txt");

                while ((line = file.ReadLine()) != null)
                {
                    extensionMediaFilter.Add(line);
                    Console.WriteLine(line);
                }

                file.Close();
            }
            catch
            {
                //Valeurs par défauts
                foreach (string s in mediaExtensions)
                    extensionMediaFilter.Add(s);
            }
        }
    }
}
