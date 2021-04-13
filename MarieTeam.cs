using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using PdfSharp.Drawing.Layout;


namespace client_marieteam
{
    class Bateau
    {
        int id { get; set; }
        string nom { get; set; }
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
        string pathimage { get; set; }
        float vitesse { get; set; }
        public BateauVoyageur(int id, string nom, float longueur, float largeur, string pathimage, float vitesse) : base(id, nom, longueur, largeur)
        {
            this.pathimage = pathimage;
            this.vitesse = vitesse;
        }
        public override string ToString()
        {
            return $"\r[IMAGE] {pathimage}\n{base.ToString()}\nVitesse : {vitesse} noeuds\n\n";
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
        public PDF(string output, string editorText){
            this.editorText = editorText;
            this.output = output;
        }
        public void Generate()
        {
            var document = new PdfDocument();
            var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var font = new XFont("Arial", 15, XFontStyle.Regular, options);

            foreach (var pagePDF in ParsingPage(editorText))
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var tf = new XTextFormatter(gfx);
                tf.Alignment = XParagraphAlignment.Center;

                tf.DrawString(pagePDF, font, XBrushes.Black, new XRect(100, 100, page.Width - 200, 600), XStringFormats.TopLeft);
            }
            document.Save(output);
            Process.Start(output);
        }
        public static List<string> ParsingPage(string res)
        {
            string parser = "#NEWPAGE";
            List<string> listparsed = new List<string>();
            while (res.Contains(parser))
            {
                int idx = res.IndexOf(parser);
                listparsed.Add(res.Substring(0, idx));
                res = res.Substring(idx + parser.Length);
            }
            return listparsed;
        }
    }
}
