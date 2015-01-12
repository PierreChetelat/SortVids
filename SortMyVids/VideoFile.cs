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
        Action, Animation, Comedy, Documentary,
        Family, Horror, Musical, Romance,
        Sport, War, Adventure, Crime,
        Drama, Fantasy, History, Music, Mystery,
        Science_Fiction, Thriller, Western, Undefined
    }

    class VideoFile
    {
        String oldPathName;

        public String OldPathName
        {
            get { return oldPathName; }
            set { oldPathName = value; }
        }

        String videoName, videoYear, videoExtension;

        //Utiliser pour rechercher les noms de films possibles
        HashSet<String> setPresumeVideoName;
        //Remplis par les noms présumé, contient les infos sur les films possibles
        List<VideoFile> listPresumeVideo;
        List<TypeMovie> listPresumeGenre;
        Boolean isVerified;

        TypeMovie genre;
        public List<TypeMovie> ListPresumeGenre
        {
            get { return listPresumeGenre; }
            set { listPresumeGenre = value; }
        }
        public String VideoExtension
        {
            get { return videoExtension; }
            set { videoExtension = value; }
        }
        public TypeMovie Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public List<VideoFile> ListPresumeVideo
        {
            get { return listPresumeVideo; }
        }
        public Boolean IsVerified
        {
            get { return isVerified; }
            set { isVerified = value;}
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
            listPresumeGenre = new List<TypeMovie>();
        }

        public void setPath(string path)
        {
            oldPathName = path;
            videoName = System.IO.Path.GetFileNameWithoutExtension(path);
            videoExtension = System.IO.Path.GetExtension(path);
            videoYear = "";
            cleanTitle();
        }

        private string getNumberFromString(string s, int sizeNumber)
        {
            string tmpNumber = "";
            int tmpSize = sizeNumber;

            foreach(char c in s)
            {
                int tmpNb;
                bool result = Int32.TryParse(c+"", out tmpNb);
                if (result == true)
                {
                    tmpNumber += Convert.ToString(tmpNb);
                    tmpSize--;
                    if (tmpSize <= 0)
                        break;
                }
            }
            if (tmpSize > 0)
                return "";
            else
                return tmpNumber;
        }

        private void cleanTitle()
        {
            setPresumeVideoName = new HashSet<String>();

            string tmpName = videoName;

            //First step : eliminate false name with filter
            foreach (string s in ResearchControl.NameMediaFilter)
                tmpName = tmpName.Replace(s, "");

            //Seconde step : eliminate '.','-'
            tmpName = tmpName.Replace("."," ");
            tmpName = tmpName.Replace("-", " ");
            
            videoName = tmpName;

            //Maybe the right title ?
            setPresumeVideoName.Add(videoName);
            
            //title sometimes delimited by year
            string[] resultName = Regex.Split(tmpName, "[0-9]{4}");
            if (resultName != null)
            {
                videoYear = getNumberFromString(tmpName, 4);
                setPresumeVideoName.Add(resultName[0]);
            }
            //or by the number of film
            resultName = Regex.Split(tmpName, "[0-9]{1}");
            if (resultName != null)
            {
                string numFilm = getNumberFromString(tmpName, 1);
                setPresumeVideoName.Add(resultName[0]+numFilm);
            }
        }

        private void addInPresumeName(string[] tab)
        {
            foreach (String s in tab)
            {
                if(s != null)
                    setPresumeVideoName.Add(s);
            }
        }

        public void searchPresumeVideo()
        {
            doResearchOnOMDB();

            foreach(VideoFile v in listPresumeVideo)
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
            listPresumeVideo = new List<VideoFile>();

            foreach(string s in setPresumeVideoName)
            {
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
                        string[] splitGenre = genreTrouve.Split(',');
                        if(splitGenre.Length > 1)
                        {
                            foreach(string genre in splitGenre)
                            {
                                string tmpGenre = genre.Replace(" ","");
                                v.listPresumeGenre.Add(getEnumFromString(tmpGenre));
                                Console.WriteLine("GENRE TROUVE1 " + tmpGenre);
                                Console.WriteLine("GENRE TROUVE1 enUM " + getEnumFromString(tmpGenre));
                            }
                        }
                        else
                        {
                            Console.WriteLine("GENRE TROUVE2 " + genreTrouve);
                            Console.WriteLine("GENRE TROUVE2 enUM " + getEnumFromString(genreTrouve));
                            v.listPresumeGenre.Add(getEnumFromString(genreTrouve));
                        }
                        listPresumeVideo.Add(v);
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

        private TypeMovie getEnumFromString(string enumString)
        {
            TypeMovie genreValue = (TypeMovie)Enum.Parse(typeof(TypeMovie), enumString);
            if (Enum.IsDefined(typeof(TypeMovie), genreValue))
            {
                return genreValue;
            }
            return TypeMovie.Undefined;
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
