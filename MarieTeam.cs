using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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
        public BateauVoyageur(int id, string nom, float longueur, float largeur, string pathimage, float vitesse):base(id, nom, longueur, largeur)
        {
            this.pathimage = pathimage;
            this.vitesse = vitesse;
        }
        public override string ToString()
        {
            return $"\r[IMAGE] {pathimage}\n{base.ToString()}\nVitesse : {vitesse} noeuds\n\n";
        }
    }

    class JeuEnregistrement
    {
        public List<BateauVoyageur> bateauVoyageurs { get; set; } = new List<BateauVoyageur>();
    }
}
