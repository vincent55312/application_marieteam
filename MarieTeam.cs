using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;

namespace client_marieteam
{
    public class Bateau
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

    public class BateauVoyageur : Bateau
    {
        public string pathimage { get; set; }
        float vitesse { get; set; }
        List<string> equipements { get; set; } = new List<string>();
        public BateauVoyageur(int id, string nom, float longueur, float largeur, string urlimage, float vitesse, List<string> equipements) : base(id, nom, longueur, largeur)
        {
            this.equipements = equipements;
            this.vitesse = vitesse;
            pathimage = $"{Directory.GetCurrentDirectory()}/{id}.jpg";
            try
            {
                new WebClient().DownloadFile(new Uri(urlimage), pathimage);
            } 
            catch {}
        }

        public override string ToString()
        {
            string equipments = $"\nListe des équipements du bateau {base.nom} : \n ";
            foreach (var item in equipements) equipments += "- " + item + "\n";
            return $"[{pathimage}]\n{base.ToString()}\nVitesse : {vitesse} noeuds\n {equipments} \n\n";
        }

    }

    public class Passerelle
    {
        public List<BateauVoyageur> bateauVoyageurs { get; set; } = new List<BateauVoyageur>();

        public static List<string> chargerLesEquipements(string eq)
        {
            List<string> tmp = new List<string>();
            while (eq.Contains("."))
            {
                int stopIndex = eq.IndexOf(".");
                tmp.Add(eq.Substring(0, stopIndex));
                eq = eq.Substring(stopIndex + 1);
            }
            return tmp;
        }
    }

    public class PDF
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

        // put string sentences into a list before each string varParser
        // static for tests

        public static List<string> ParsingPage(string text)
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

        // put string paths images into a list : between parserStart and parserEnd
        // static for tests

        public static List<string> ParsingImage(string text)
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

        // remove images paths from text
        // static for tests

        public static string ParsingDeleteImage(string text)
        {
            string varParserStart = "[";
            string varParserEnd = "]";
            while (text.Contains(varParserEnd))
            {
                int startIndex = text.IndexOf(varParserStart);
                int endIndex = text.IndexOf(varParserEnd);
                if (startIndex >= 0) text = text.Remove(startIndex, endIndex - startIndex + varParserEnd.Length);
                else break;
            }
            return text;
        }
    }
}
  