﻿using System;
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
        Action, Adventure, Animation, Comedy, Crime, Documentary,
        Drama, Family, Fantasy, History, Horror, Musical, Music, Mystery, 
        Romance, Sci_Fi, Sport, Thriller, War, Western, Undefined
    }

   
    class VideoFile
    {
        static string[] langAbb = { "VF", "VOSTFR", "VO", "MULTI", "FANSUB" };

        String oldPathName;
        List<String> listFilterName;

        public List<String> ListFilterName
        {
            get { return listFilterName; }
            set { listFilterName = value; }
        }

        String videoName, videoYear, videoExtension;
        List<String> videoLangAbb;

        //Utiliser pour rechercher les noms de films possibles
        HashSet<String> setPresumeVideoName;
        //Remplis par les noms présumé, contient les infos sur les films possibles
        List<VideoFile> listPresumeVideo;
        List<TypeMovie> listPresumeGenre;
        Boolean isVerified;

        TypeMovie genre;

        public String OldPathName
        {
            get { return oldPathName; }
            set { oldPathName = value; }
        }
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

            listFilterName = new List<string>();
            listPresumeVideo = new List<VideoFile>();
            listPresumeGenre = new List<TypeMovie>();
            setPresumeVideoName = new HashSet<string>();
            videoLangAbb = new List<string>();
        }

        public VideoFile(string path) : this()
        {
            setPath(path);
        }

        public void setPath(string path)
        {
            oldPathName = path;
            videoName = System.IO.Path.GetFileNameWithoutExtension(path);
            videoExtension = System.IO.Path.GetExtension(path);
            videoYear = "";
        }

        private string getNumberFromString(string s, int sizeNumber)
        {
            int tmpSize = sizeNumber, startIndex = 0;

            foreach(char c in s)
            {
                int tmpNb;
                bool result = Int32.TryParse(c+"", out tmpNb);
                if (result == true)
                {
                    try
                    {
                        string sub = s.Substring(startIndex, tmpSize);
                        if(!sub.Contains(" "))
                        {
                            result = Int32.TryParse(sub, out tmpNb);
                            if(result == true)
                            {
                                return s.Substring(startIndex, tmpSize);
                            }
                        }
                    }
                    catch(System.ArgumentOutOfRangeException e)
                    {
                        return "";
                    }
                }
                startIndex++;
            }
        
            return "";
        }

        public void cleanTitle()
        {
            setPresumeVideoName = new HashSet<String>();

            string tmpName = videoName;


            foreach(string s in langAbb)
            {
                if(tmpName.Contains(s))
                {
                    videoLangAbb.Add(s);
                }
            }

            //First step : eliminate false name with filter
            foreach (string s in listFilterName)
            {
                if(s != "")
                    tmpName = tmpName.Replace(s, "");
            }

            //Seconde step : eliminate '.','-'
            tmpName = tmpName.Replace("."," ");
            tmpName = tmpName.Replace("-", " ");
            tmpName = tmpName.Trim();

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
                try
                {
                    Match test = Regex.Match(v.VideoName.ToUpper(), "[" + videoName.ToUpper() + "]");
                    if (test.Success)
                    {
                        this.videoName = v.VideoName;
                        this.videoYear = v.VideoYear;
                        this.genre = v.Genre;
                        this.isVerified = true;
                        break;
                    }
                }
                catch(System.ArgumentException e)
                {

                }

            }
        }

        private void doResearchOnOMDB()
        {
            listPresumeVideo = new List<VideoFile>();

            foreach(string s in setPresumeVideoName)
            {
                try
                {
                    string nameForRequest = s.Replace(" ", "+");
                    string requete = "http://www.omdbapi.com/?t=" + nameForRequest + "&y=" + videoYear + "&plot=short&r=json";

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

                    if(titreTrouve != null && anneTrouve != null && genreTrouve != null)
                    {
                        VideoFile v = new VideoFile();
                        //
                        //v.listPresumeGenre = new List<TypeMovie>();
                        //
                        v.VideoName = titreTrouve;
                        v.VideoYear = anneTrouve;
                        string[] splitGenre = genreTrouve.Split(',');
                        if(splitGenre.Length > 1)
                        {
                            foreach(string genre in splitGenre)
                            {
                                string tmpGenre = genre.Replace(" ","");
                                v.listPresumeGenre.Add(getEnumFromString(tmpGenre));
                            }
                        }
                        else
                        {
                            v.listPresumeGenre.Add(getEnumFromString(genreTrouve));
                        }

                        if(v.listPresumeGenre.First() == TypeMovie.Undefined && v.listPresumeGenre.Count > 1)
                        {
                            v.Genre = v.listPresumeGenre.ElementAt(1);
                        }
                        else
                        {
                            v.Genre = v.ListPresumeGenre.First();
                        }

                        listPresumeVideo.Add(v);
                    }

                    objReader.Close();
                    r.Close();
                    
                }
                catch(System.ArgumentException e)
                {
                    Console.WriteLine("ERRRRROOOOOOOORRRRRRRR");
                    Console.WriteLine("FILM " + s);
                }
            }

        }

        private TypeMovie getEnumFromString(string enumString)
        {
            try
            {
                if(enumString == "Sci-Fi")
                {
                    return TypeMovie.Sci_Fi;
                }

                TypeMovie genreValue = (TypeMovie)Enum.Parse(typeof(TypeMovie), enumString);
                if (Enum.IsDefined(typeof(TypeMovie), genreValue))
                {
                    return genreValue;
                }
                return TypeMovie.Undefined;
            }
            catch
            {
                return TypeMovie.Undefined;
            }
        }

        public override string ToString()
        {
            string tmp = videoName;

            if (videoLangAbb.Count > 0)
            {
                foreach (string s in videoLangAbb)
                {
                    tmp += " " + s;
                }
            }
            
            if(videoYear != "")
            {
                if (videoYear.Length > 0)
                    tmp += " (" + videoYear + ")";
            }


            return tmp;
        }
    }
}
