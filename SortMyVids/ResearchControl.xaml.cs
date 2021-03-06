﻿using System;
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

        List<VideoFile> listMyVideos = new List<VideoFile>();
  
        List<string> listExtensionMediaFilter = new List<string>();

        List<string> listNameMediaFilter = new List<string>();

        string directorySrc, directoryDest;

        public List<string> ListNameMediaFilter
        {
            get { return listNameMediaFilter; }
            set { listNameMediaFilter = value;
                  addFiltersName();
                  if (ListMyVideos.Count > 0)
                  {
                      launchResearchVideoFile();
                  }
            }
        }

        public List<string> ListExtensionMediaFilter
        {
            get { return listExtensionMediaFilter; }
            set { listExtensionMediaFilter = value;
                  addFiltersExtension();
            }
        }
        

        internal List<VideoFile> ListMyVideos
        {
            get { return listMyVideos; }
            set { listMyVideos = value; }
        }



        public ResearchControl()
        {
            InitializeComponent();
            updateFilters();
        }

        private void uiButtonSrc_Click(object sender, RoutedEventArgs e)
        {
        
            directorySrc = showAndGetFolder(true);
            
            if(directorySrc != "")
            {
                uiFolderSrc.Text = directorySrc;

                launchResearchVideoFile();
            }
        }

        private void launchResearchVideoFile()
        {
            ListMyVideos.Clear();
            uiListMovie.Items.Clear();

            BackgroundWorker bw = new BackgroundWorker();
            // define the event handlers, work in other thread
            bw.DoWork += getVideoFile;
            bw.RunWorkerCompleted += (objesender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    System.Windows.MessageBox.Show("Une erreur s'est produite pendant la recherche !", "Information");
                }
                //Work in UI THREAD
                else
                {
                    fillListVideoFile();
                    uiLabelNBVideo.Content = "Vidéos trouvés : " + listMyVideos.Count;
                }
            };

            uiLabelNBVideo.Content = "Recherche de vidéos...";

            bw.RunWorkerAsync(listNameMediaFilter.ToList());
        }

        private void uiButtonDest_Click(object sender, RoutedEventArgs e)
        {
            askForFolderDest();
        }

        public void askForFolderDest()
        {
            directoryDest = showAndGetFolder(false);
            if(directoryDest != "")
                uiFolderDest.Text = directoryDest;
        }

        private String showAndGetFolder(Boolean isFolderSrc)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();

            fBD.ShowNewFolderButton = false;
            fBD.RootFolder = System.Environment.SpecialFolder.MyComputer;
            if(isFolderSrc)
            {
                fBD.Description = "Choisir un dossier source";
            }
            else
            {
                fBD.Description = "Choisir un dossier de destination";
            }

            if (fBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return fBD.SelectedPath;
            }

            return "";
        }

        private void getVideoFile(object sender, DoWorkEventArgs e)
        {
            List<string> list = e.Argument as List<string>;

            if(directorySrc != null)
            { 
                foreach (string path in Directory.GetFiles(directorySrc, "*", SearchOption.AllDirectories))
                {
                    if (Array.IndexOf(listExtensionMediaFilter.ToArray(), System.IO.Path.GetExtension(path).ToUpperInvariant()) != -1)
                    {
                        VideoFile v = new VideoFile(path);
                        v.ListFilterName = listNameMediaFilter;
                        v.cleanTitle();
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

        private void addFiltersName()
        {
            List<string> listTmpToWrite = new List<string>();

            foreach(string s in listNameMediaFilter)
            {
                if(!mediaName.Contains(s))
                {
                    listTmpToWrite.Add(s);
                }
            }

            if (listTmpToWrite.Count > 0)
                System.IO.File.WriteAllLines("./nameFilters.txt", listTmpToWrite);
        }

        private void addFiltersExtension()
        {
            List<string> listTmpToWrite = new List<string>();

            foreach (string s in listExtensionMediaFilter)
            {
                if (!mediaExtensions.Contains(s))
                {
                    listTmpToWrite.Add(s);
                }
            }

            if(listTmpToWrite.Count > 0)
                System.IO.File.WriteAllLines("./extensionsFilters.txt", listTmpToWrite);
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
                    if(line != "")
                        listNameMediaFilter.Add(line);
                }

                file.Close();
            }
            finally
            {
                //Valeurs par défauts
                foreach (string s in mediaName)
                    listNameMediaFilter.Add(s);

            }

            //Lecture des filtres d'extensions
            try
            {

                System.IO.StreamReader file = new System.IO.StreamReader("./extensionsFilters.txt");

                while ((line = file.ReadLine()) != null)
                {
                    if(line != "")
                        listExtensionMediaFilter.Add(line);
                }

                file.Close();
            }
            finally
            {
                //Valeurs par défauts
                foreach (string s in mediaExtensions)
                    listExtensionMediaFilter.Add(s);
            }
        }
    }
}
