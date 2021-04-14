using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using PdfSharp.Drawing.Layout;
using System.Windows;
using System.Net;

namespace client_marieteam
{
    class Bateau
    {
        public int id { get; set; }
        public string nom { get; set; }
        float longueur { get; set; }
        float largeur { get; set; }
        public Bateau(int id, string nom, float longueur, float largeur)
        {
            this.id = id;
            this.nom = nom;
            this.longueur = longueur;
            this.largeur = largeur;
        }
        public override string ToString()
        {
            return $"\rNom du bateau : {nom} \nLongueur : {longueur} mètres \nLargeur : {largeur} mètres";
        }
    }

    class BateauVoyageur : Bateau
    {
        public string pathimage { get; set; }
        float vitesse { get; set; }
        List<string> equipements { get; set; } = new List<string>();
        public BateauVoyageur(int id, string nom, float longueur, float largeur, string urlimage, float vitesse) : base(id, nom, longueur, largeur)
        {
            this.vitesse = vitesse;
            pathimage = $"C:/Users/33756/Desktop/client_marieteam/images/{id}.jpg";
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(urlimage), pathimage);
                }
            } 
            catch {}

            getEquipments();
        }

        public override string ToString()
        {
            string equipments = $"\nListe des équipements du bateau {base.nom} : \n ";
            foreach (var item in equipements) equipments += "- " + item + "\n";
            return $"[{pathimage}]\n{base.ToString()}\nVitesse : {vitesse} noeuds\n {equipments} \n\n";
        }

        public void getEquipments()
        {   // bdd (Accès Handicapé.Bar.Pont promenade.Baby foot.Dansoir.) => List<string> {Accès Handicapé, Bar, Pont promenade, Baby foot, Dansoir}
            ClientSQL client = new ClientSQL();
            if (client.OpenConnection())
            {
                var sql = $"SELECT equipements FROM bateau WHERE id_bateau = {base.id}";
                var cmd = new MySqlCommand(sql, client.Client);
                MySqlDataReader rdr = cmd.ExecuteReader();
                string tmp = "";
                while (rdr.Read()) tmp = rdr.GetString(0);
                client.CloseConnection();
                while (tmp.Contains("."))
                {
                    int stopIndex = tmp.IndexOf(".");
                    equipements.Add(tmp.Substring(0, stopIndex));
                    tmp = tmp.Substring(stopIndex + 1);
                }
            }
        }
    }

    class Passerelle
    {
        public List<BateauVoyageur> bateauVoyageurs { get; set; } = new List<BateauVoyageur>();
    }

    class PDF
    {
        public string output { get; set; }
        public string editorText { get; set; }
        public PDF(string output, string editorText)
        {
            this.editorText = editorText;
            this.output = output;
        }
        public void Generate()
        {
            try
            {
                var document = new PdfDocument();
                var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
                var font = new XFont("Arial", 15, XFontStyle.Regular, options);
                int i = 0;
                string text = editorText;
                List<string> images = ParsingImage(text);
                editorText = ParsingDeleteImage(editorText);
                foreach (var pagePDF in ParsingPage(editorText))
                {
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);
                    if (i < images.Count())
                    {
                        XImage image = XImage.FromFile(images[i]);
                        gfx.DrawImage(image, 50, 50, 500, 250);
                    }
                    var tf = new XTextFormatter(gfx);
                    tf.Alignment = XParagraphAlignment.Center;
                    tf.DrawString(pagePDF, font, XBrushes.Black, new XRect(100, 350, page.Width - 200, 600), XStringFormats.TopLeft);
                    i++;
                }
                document.Save(output);
                Process.Start(output);
            }
            catch
            {
                MessageBox.Show("Erreur durant la génération du PDF");
            }
        }
        public List<string> ParsingPage(string text)
        {
            string varParser = "#NEWPAGE";
            List<string> listParsed = new List<string>();
            while (text.Contains(varParser))
            {
                int stopIndex = text.IndexOf(varParser);
                listParsed.Add(text.Substring(0, stopIndex));
                text = text.Substring(stopIndex + varParser.Length);
            }
            return listParsed;
        }

        public List<string> ParsingImage(string text)
        {
            string varParserStart = "[";
            string varParserEnd = "]";
            List<string> listParsed = new List<string>();
            while (text.Contains(varParserStart))
            {
                int startIndex = text.IndexOf(varParserStart);
                int endIndex = text.IndexOf(varParserEnd);
                listParsed.Add(text.Substring(startIndex + varParserStart.Length, endIndex - startIndex - varParserEnd.Length));
                text = text.Substring(endIndex + varParserEnd.Length);
            }
            return listParsed;
        }
        public string ParsingDeleteImage(string text)
        {
            string varParserStart = "[";
            string varParserEnd = "]";
            while (text.Contains(varParserEnd))
            {
                int startIndex = text.IndexOf(varParserStart);
                int endIndex = text.IndexOf(varParserEnd);
                text = text.Remove(startIndex, endIndex - startIndex + varParserEnd.Length);
            }
            return text;
        }

    }
}
