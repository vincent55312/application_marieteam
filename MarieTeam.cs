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
using System.Windows;


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
        string pathimage { get; set; }
        float vitesse { get; set; }
        List<string> equipements { get; set; } = new List<string>();
        public BateauVoyageur(int id, string nom, float longueur, float largeur, string pathimage, float vitesse) : base(id, nom, longueur, largeur)
        {
            this.pathimage = pathimage;
            this.vitesse = vitesse;
            getEquipments();
        }
        public override string ToString()
        {
            string equipments = $"\nListe des équipements du bateau {base.nom} : \n ";
            foreach (var item in equipements) equipments += "- " + item + "\n";
            return $"\r[IMAGE] {pathimage}\n{base.ToString()}\nVitesse : {vitesse} noeuds\n {equipments} \n\n";
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
        public PDF(string output, string editorText){
            this.editorText = editorText;
            this.output = output;
        }
        public void Generate()
        {
            var document = new PdfDocument();
            var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var font = new XFont("Arial", 15, XFontStyle.Regular, options);

            foreach (var pagePDF in ParsingPage())
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
        public List<string> ParsingPage()
        {
            string varParser = "#NEWPAGE";
            List<string> listParsed = new List<string>();
            while (editorText.Contains(varParser))
            {
                int stopIndex = editorText.IndexOf(varParser);
                listParsed.Add(editorText.Substring(0, stopIndex));
                editorText = editorText.Substring(stopIndex + varParser.Length);
            }
            return listParsed;
        }
    }
}
