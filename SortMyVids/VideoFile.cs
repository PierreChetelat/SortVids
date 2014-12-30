using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace SortMyVids
{
    public enum TypeMovie
    {
        ACTION, HORROR, ADVENTURE, SCI_FI, FANTASTIQUE
    }
    class VideoFile
    {
        String oldPathName, newPathName;
        String videoName;
        String[] presumeVideoName;

        TypeMovie genre;

        public String VideoName
        {
            get { return videoName; }
            set { videoName = value; }
        }

        public VideoFile(string path)
        {
            oldPathName = path;
            videoName = System.IO.Path.GetFileNameWithoutExtension(path);
            searchGenre();
        }

        private void cleanTitle()
        {

        }

        private void searchGenre()
        {
            var t = Task.Factory.StartNew<String>(() => doAction());
            string result = t.Result;
            Console.WriteLine("Resultat :" + result);
            string[] tab = { result };
            //ComboBoxDialog.ShowDialog(tab);
        }

        private string doAction()
        {
            string title = "matrix";
            string annee = "";
            string requete = "http://www.omdbapi.com/?t=" + title + "&y=" + annee + "&plot=short&r=json";

            WebRequest request = WebRequest.Create(requete);
            WebResponse r = request.GetResponse();

            StreamReader objReader = new StreamReader(r.GetResponseStream());

            string sLine = "", result = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    result += sLine;
            }

            Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(result);

            string titreTrouve = (string)token.SelectToken("Title");
            string anneTrouve = (string)token.SelectToken("Year");

            return titreTrouve + " : " + anneTrouve;
            throw new NotImplementedException();
        }
    }
}
