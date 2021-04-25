using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Windows;

namespace client_marieteam
{
    public class Licence
    {
        public static readonly string path = $"{Directory.GetCurrentDirectory()}\\licence.json";
        string apiTest = "0";
        string apiAdress = "http://93.113.207.251:8080/api/";
        public bool isValid { get; set; }
        public bool APIisWorking { get; set; }

        public Licence(string key)
        {
            ApiWorking();
            if (APIisWorking) Exist(key);
            else MessageBox.Show("API de licence n'est pas en fonctionnement");
        }
        private void Exist(string key)
        {
            try
            {
                Uri uri = new Uri(apiAdress + key);
                WebRequest request = WebRequest.Create(uri);
                WebResponse response = request.GetResponse();
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(response.GetResponseStream());
                    isValid = bool.Parse(sr.ReadToEnd());
                }
                catch
                {
                    isValid = false;
                }
                finally
                {
                    if (sr != null) sr.Close();
                }
            }
            catch
            {
                isValid = false;
            }
        }

        private void ApiWorking()
        {
            try
            {
                Uri uri = new Uri(apiAdress + apiTest);
                WebRequest request = WebRequest.Create(uri);
                WebResponse response = request.GetResponse();
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(response.GetResponseStream());
                    APIisWorking = bool.Parse(sr.ReadToEnd());
                }
                catch
                {
                    APIisWorking = false;
                }
                finally
                {
                    if (sr != null) sr.Close();
                }
            }
            catch
            {
                APIisWorking = false;
            }
        }

    }
}