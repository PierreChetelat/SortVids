using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SortMyVids
{
    public enum TypeMovie
    {
        ACTION, HORROR, ADVENTURE, SCI_FI, FANTASTIQUE
    }

    class VideoFile
    {
        String oldPathName, newPathName;
        String videoName, videoYear;
        //Utiliser pour rechercher les noms de films possibles
        List<string> presumeVideoName;
        //Remplis par les noms présumé, contient les infos sur les films possibles
        List<VideoFile> presumeVideo;
        Boolean isVerified;

        TypeMovie genre;

        public List<VideoFile> PresumeVideo
        {
            get { return presumeVideo; }
        }
        public Boolean IsVerified
        {
            get { return isVerified; }
            set { isVerified = value; }
        }

        public String VideoName
        {
            get { return videoName; }
            set { videoName = value; }
        }
        public String VideoYear
        {
            get { return videoYear; }
            set { videoYear = value; }
        }

        public VideoFile()
        {
            presumeVideoName = new List<string>();
            isVerified = false;
        }

        public void setPath(string path)
        {
            oldPathName = path;
            videoName = System.IO.Path.GetFileNameWithoutExtension(path);
            videoYear = "";
            cleanTitle();
        }

        private void cleanTitle()
        {
            Console.WriteLine("AJOUT DU TITRE");
            string tmpName = videoName;

            //First step : eliminate false name with filter
            foreach (string s in ResearchControl.NameMediaFilter)
                tmpName.Replace(s, "");

            videoName = tmpName;

            //Maybe the right title ?
            presumeVideoName.Add(videoName);
            
            //title sometimes delimited by year
            string[] resultAll = Regex.Split(tmpName, "[0-9]{4}");
            if(resultAll != null)
            {
                //if there is date, get the name separate by '.'
                string[] resultName = resultAll[0].Split('.');

                addInPresumeName(resultName);
            }
            //If there is no date, maybe delimited by '-'
            else
            {
                string[] resultName = tmpName.Split('-');
                
                if(resultName != null)
                {
                    addInPresumeName(resultName);
                }
                //Or by '.'
                else
                {
                    resultName = tmpName.Split('.');

                    addInPresumeName(resultName);
                }
            }
        }

        private void addInPresumeName(string[] tab)
        {
            foreach (string s in tab)
            {
                Console.WriteLine("AJOUT DU TITRE " + s);
                presumeVideoName.Add(s);
            }
        }

        public void searchGenre()
        {
            Console.WriteLine("ON LANCE");
            Task.Factory.StartNew(() => doAction());
        }

        private void doAction()
        {
            foreach(string s in presumeVideoName)
            {
                string requete = "http://www.omdbapi.com/?t=" + s + "&y="+videoYear+"&plot=short&r=json";

                WebRequest request = WebRequest.Create(requete);
                WebResponse r = request.GetResponse();

                StreamReader objReader = new StreamReader(r.GetResponseStream());

                string sLine = "", result = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        result += sLine;
                }

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(result);

                string titreTrouve = (string)token.SelectToken("Title");
                string anneTrouve = (string)token.SelectToken("Year");

                if(titreTrouve != null && anneTrouve != null)
                { 
                    VideoFile v = new VideoFile();
                    v.VideoName = titreTrouve;
                    v.VideoYear = anneTrouve;

                    presumeVideo.Add(v);
                    Console.WriteLine("TITRE TROUVE");
                }
                Console.WriteLine("PARCOURS");
            }
            
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return videoName;
        }
    }
}
