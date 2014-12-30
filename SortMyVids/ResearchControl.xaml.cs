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
    
    public partial class ResearchControl : System.Windows.Controls.UserControl
    {

        static string[] mediaExtensions = {
            ".AVI", ".MP4", ".DIVX", ".WMV"
        };

        List<VideoFile> myVideos = new List<VideoFile>();

        string directorySrc, directoryDest;

        public ResearchControl()
        {
            InitializeComponent();
        }

        private void uiButtonSrc_Click(object sender, RoutedEventArgs e)
        {
            directorySrc = showAndGetFolder();
            uiFolderSrc.Text = directorySrc;
            getVideoFile(uiFolderSrc.Text);
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

        private void getVideoFile(string pathDirectory)
        {
            foreach (string path in Directory.GetFiles(pathDirectory, "*", SearchOption.AllDirectories))
            {
                if (Array.IndexOf(mediaExtensions, System.IO.Path.GetExtension(path).ToUpperInvariant()) != -1)
                {
                    myVideos.Add(new VideoFile(path));
                }
            }
        }
    }
}
