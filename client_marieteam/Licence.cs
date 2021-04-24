using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
namespace client_marieteam
{
    public class Licence
    {
        string apiTest = "0";
        string apiAdress = "http://localhost:8080/api/";
        string key { get; set; }
        public Licence(string key)
        {
            this.key = key; 
        }
        public static readonly string path = $"{Directory.GetCurrentDirectory()}\\licence.json";

        private bool ApiWorking()
        {
            bool result;
            try
            {
                Uri uri = new Uri(apiAdress + apiTest);
                WebRequest request = WebRequest.Create(uri);
                WebResponse response = request.GetResponse();
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(response.GetResponseStream());
                    result = bool.Parse(sr.ReadToEnd());
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    if (sr != null) sr.Close();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool Exist()
        {
            if (ApiWorking())
            {
                bool result;
                try
                {
                    Uri uri = new Uri(apiAdress + key);
                    WebRequest request = WebRequest.Create(uri);
                    WebResponse response = request.GetResponse();
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(response.GetResponseStream());
                        result = bool.Parse(sr.ReadToEnd());
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        if (sr != null) sr.Close();
                    }
                }
                catch
                {
                    result = false;
                }
                return result;
            }
            else return false;
        }
    }
}