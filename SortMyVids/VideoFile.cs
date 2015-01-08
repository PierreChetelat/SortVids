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
        ACTION, ANIMATION, COMEDY, DOCUMENTARY,
        FAMILY, FILM_NOIR, HORROR, MUSICAL, ROMANCE,
        SPORT, WAR, ADVENTURE, BIOGRAPHY, CRIME,
        DRAMA, FANTASY, HISTORY, MUSIC, MYSTERY,
        SCI_FI, THRILLER, WESTERN
    }

    class VideoFile
    {
        String oldPathName, newPathName;
        String videoName, videoYear;
        //Utiliser pour rechercher les noms de films possibles
        List<String> presumeVideoName;
        //Remplis par les noms présumé, contient les infos sur les films possibles
        List<VideoFile> presumeVideo;
        Boolean isVerified;

        TypeMovie genre;

        public TypeMovie Genre
        {
            get { return genre; }
            set { genre = value; }
        }

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
            isVerified = false;
        }

        public VideoFile(VideoFile v)
        {
            this.videoName = v.videoName;
            this.oldPathName = v.oldPathName;
            this.newPathName = v.newPathName;
            this.genre = v.genre;
            this.presumeVideoName = v.presumeVideoName.ToList();
            this.presumeVideo = v.presumeVideo.ToList();
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
            presumeVideoName = new List<String>();

            string tmpName = videoName;

            //First step : eliminate false name with filter
            foreach (string s in ResearchControl.NameMediaFilter)
                tmpName = tmpName.Replace(s, "");

            //Seconde step : eliminate '.','-'
            tmpName = tmpName.Replace("."," ");
            tmpName = tmpName.Replace("-", " ");
            
            videoName = tmpName;

            //Maybe the right title ?
            presumeVideoName.Add(videoName);
            
            //title sometimes delimited by year
            string[] resultName = Regex.Split(tmpName, "[0-9]{4}");
            if (resultName != null)
            {
                //Console.WriteLine("FILM " + videoName + " ||||| RECHERCHE DE " + resultName[0]);
                presumeVideoName.Add(resultName[0]);
            }
            //or by the number of film
            resultName = Regex.Split(tmpName, "[0-9]{1}");
            if (resultName != null)
            {
                //Console.WriteLine("FILM " + videoName + " ||||| RECHERCHE DE " + resultName[0]);
                presumeVideoName.Add(resultName[0]);
            }

            //Suppression des doublons
            //presumeVideoName = presumeVideoName.Distinct() as List<String>;
        }

        private void addInPresumeName(string[] tab)
        {
            foreach (String s in tab)
            {
                if(s != null)
                    presumeVideoName.Add(s);
            }
        }

        public void searchPresumeVideo()
        {
            doResearchOnOMDB();

            foreach(VideoFile v in presumeVideo)
            {
                Match test = Regex.Match(v.VideoName.ToUpper(), "["+videoName.ToUpper()+"]");
                if (test.Success)
                {
                    this.videoName = v.VideoName;
                    this.videoYear = v.VideoYear;
                    this.isVerified = true;
                    break;
                }
            }
        }

        private void doResearchOnOMDB()
        {
            presumeVideo = new List<VideoFile>();

            //Console.WriteLine("DEBUT RECHERCHE VIDEO " + videoName);
            foreach(string s in presumeVideoName)
            {
                Console.WriteLine("RECHERCHE DE "+s);
                
                string requete = "http://www.omdbapi.com/?t=" + s + "&y="+videoYear+"&plot=short&r=json";

                try
                {
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
                    string genreTrouve = (string)token.SelectToken("Genre");

                    if(titreTrouve != null && anneTrouve != null)
                    {
                        //Console.WriteLine("FILM TROUVE : " + titreTrouve + " " + anneTrouve);
                        VideoFile v = new VideoFile();
                        v.VideoName = titreTrouve;
                        v.VideoYear = anneTrouve;
                        Console.WriteLine("FILM " + titreTrouve + " | GENRE : "+genreTrouve);
                        presumeVideo.Add(v);
                    }
                    objReader.Close();
                    r.Close();
                    
                }
                catch
                {
                    Console.WriteLine("ERRRRROOOOOOOORRRRRRRR");
                }
            }

            //Console.WriteLine("FIN RECHERCHE VIDEO " + videoName);
        }

        public override string ToString()
        {
            string tmp = videoName;
            if(videoYear.Length > 0)
                tmp += " ("+videoYear+")";

            return tmp;
        }
    }
}
