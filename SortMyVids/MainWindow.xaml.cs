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
            BackgroundWorker bw = new BackgroundWorker();

            // define the event handlers, work in other thread
            bw.DoWork += worker_DoWork;
            bw.RunWorkerCompleted += (objesender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    Console.WriteLine("PAS REUSSI");
                }
                //Work in UI THREAD
                else
                {
                    Console.WriteLine("REUSSI");
                    uiUnknownVideosControl.ListUnknownVideo = args.Result as List<VideoFile>;
                }
            };

            
            bw.RunWorkerAsync(uiResearchControl.ListMyVideos.ToList());

        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Recup de la liste");
            List<VideoFile> list = e.Argument as List<VideoFile>;
            Console.WriteLine("taille : "+list.Count);

            foreach (VideoFile v in list)
            {
                v.searchGenre();
            }
            
            foreach (VideoFile v in list)
            {
                if (!v.IsVerified)
                    listTmp.Add(v);
            }

            e.Result = list;
        }

    }
}
