using System.IO;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace garesTP10
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };
            }
            

        }
        public static void PourcentageParNatureDesGares()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var listeNature = from tabg in query
                                  join tabn in db.natures
                                  on tabg.nature equals tabn.nom_nature
                                  orderby tabn.nom_nature
                                  select new { nomGare = tabg.nom, nature = tabn.nom_nature, numNature = tabn.numero_nature };

                var filtreNatureParGares = listeNature.Distinct();

                int compteur1 = 1;
                int compteur2 = 1;
                int compteur3 = 1;
                int compteur4 = 1;
                int compteur5 = 1;
                int compteur6 = 1;
                int compteur7 = 1;
                int compteur8 = 1;

                int diviseur = filtreNatureParGares.Count();

                foreach (var affiche in filtreNatureParGares)
                {
                    if (affiche.numNature == 1) compteur1++;
                    if (affiche.numNature == 2) compteur2++;
                    if (affiche.numNature == 3) compteur3++;
                    if (affiche.numNature == 4) compteur4++;
                    if (affiche.numNature == 5) compteur5++;
                    if (affiche.numNature == 6) compteur6++;
                    if (affiche.numNature == 7) compteur7++;
                    if (affiche.numNature == 8) compteur8++;
                }
                Console.WriteLine("Pourcentage de gares en 'Desserte Voyageur' est de : {0}%.", ((compteur1 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Desserte Voyageur-Infrastructure' est de : {0}%.", ((compteur2 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Non exploitée' est de : {0}%.", ((compteur3 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Desserte Fret-Desserte Voyageur-Infrastructure' est de : {0}%.", ((compteur4 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Desserte Fret-Infrastructure' est de : {0}%.", ((compteur5 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Infrastructure' est de : {0}%.", ((compteur6 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Desserte Fret-Desserte Voyageur' est de : {0}%.", ((compteur7 * 100) / diviseur));
                Console.WriteLine("Pourcentage de gares en 'Desserte Fret' est de : {0}%.", ((compteur8 * 100) / diviseur));
            }
        }
        public static void ListeDesGaresParNature()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var listeNature = from tabg in query
                                  join tabn in db.natures
                                  on tabg.nature equals tabn.nom_nature
                                  orderby tabn.nom_nature
                                  select new { nomGare = tabg.nom, nature = tabn.nom_nature, numNature = tabn.numero_nature };

                var filtreNatureParGares = listeNature.GroupBy(x => new { x.nature, x.nomGare });

                foreach (var affiche in filtreNatureParGares)
                {
                    Console.WriteLine(affiche.Key);
                }
            }
        }
        public static void ChoixNaturedesGares()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var listeNature = from tabg in query
                                  join tabn in db.natures
                                  on tabg.nature equals tabn.nom_nature

                                  select new { nomGare = tabg.nom, nature = tabn.nom_nature, numNature = tabn.numero_nature };

                var filtreNatureParGares = listeNature.Distinct();

                int compteur = 1;

                Console.WriteLine("entrer un nb entre 1 à 8");
                Console.WriteLine("\t 1 = Desserte Voyageur");
                Console.WriteLine("\t 2 = Desserte Voyageur-Infrastructure");
                Console.WriteLine("\t 3 = Non exploitée");
                Console.WriteLine("\t 4 = Desserte Fret-Desserte Voyageur-Infrastructure");
                Console.WriteLine("\t 5 = Desserte Fret-Infrastructure");
                Console.WriteLine("\t 6 = Infrastructure");
                Console.WriteLine("\t 7 = Desserte Fret-Desserte Voyageur");
                Console.WriteLine("\t 8 = Desserte Fret");

                int choix;
                string nb;
                bool resultat = false;

                do
                {
                    nb = Console.ReadLine();
                    resultat = Int32.TryParse(nb, out choix);
                }
                while (choix < 1 || choix > 8);

                foreach (var affiche in filtreNatureParGares)
                {
                    if (affiche.numNature == choix)
                    {
                        Console.WriteLine(compteur + "\t" + affiche.nomGare);
                        compteur++;
                    }
                }
            }
        }
        public static void ListerLesGaresParVilles()
        {
            using (var db = new garesDataModel())
            {
                var listeGaresParVilles = from tabv in db.villes

                                          join tabg in db.gares
                                          on tabv.numero_ville equals tabg.numero_ville
                                          orderby tabg.nom_gare
                                          select new { nomVille = tabv.nom_ville, nomGare = tabg.nom_gare };

                var final = listeGaresParVilles.GroupBy(x => new { x.nomVille, x.nomGare });

                foreach (var affiche in final)
                {
                    Console.WriteLine(affiche.Key);

                }
            }
        }
        public static void ListerLesGaresParOrdreAlphabetique()
        {
            using (var db = new garesDataModel())
            {
                var listeGare = from tabg in db.gares
                                orderby tabg.nom_gare
                                select tabg.nom_gare;



                foreach (var affiche in listeGare)
                {
                    Console.WriteLine(affiche);
                }

            }
        }
        public static void AjoutLigne()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var filtreSurLigne = from tabLigne in query
                                      select new { ligne = tabLigne.codeLigne, latitude = tabLigne.latitude, longitude = tabLigne.longitude };

                var ligneFiltre = filtreSurLigne.Distinct();

                foreach (var ajout in ligneFiltre)
                {
                    int nombreCodeLigne;
                    bool resultatNombreCodeLigne = Int32.TryParse(ajout.ligne, out nombreCodeLigne);
                    
                    ligne maLigne = new ligne
                    {
                        code_ligne = nombreCodeLigne,
                        latitude = ajout.latitude,
                        longitude = ajout.longitude
                    };

                    try
                    {
                        db.lignes.Add(maLigne);
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                }
                db.SaveChanges();
            }
        }
        public static void AjoutNature()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var filtreSurNature = from tabNature in query
                                    select tabNature.nature;

                var natureFiltre = filtreSurNature.Distinct();

                foreach (var ajout in natureFiltre)
                {
                    nature maNature = new nature
                    {
                        nom_nature = ajout
                    };

                    try
                    {
                        db.natures.Add(maNature);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                db.SaveChanges();
            }
        }
        public static void AjoutGare()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var filtreSurGare = from tabGare in query
                                    select tabGare.nom;

                var filtreSurVille = from tabVille in db.villes
                                     select tabVille;

                var gareFiltre = filtreSurGare.Distinct();

                var numeroVille = from nomVillePourGare in query
                                  join tab in filtreSurVille
                                  on nomVillePourGare.ville equals tab.nom_ville
                                  select new { nomville = nomVillePourGare.ville, numerodeVillePourGare = tab.numero_ville, nomgare = nomVillePourGare.nom };

                var filtrenumeroGare = numeroVille.Distinct();

                foreach (var ajout in filtrenumeroGare)
                {
                    gare maGare = new gare
                    {
                        nom_gare = ajout.nomgare,
                        numero_ville = ajout.numerodeVillePourGare
                    };
                    try
                    {
                        db.gares.Add(maGare);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                db.SaveChanges();
            }
        }
        public static void AjoutVille()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var filtreSurVille = from tabVille in query
                                     select tabVille.ville;

                var villeFiltre = filtreSurVille.Distinct();

                foreach (var ajout in villeFiltre)
                {

                    ville maVille = new ville
                    {
                        nom_ville = ajout
                    };

                    try
                    {
                        db.villes.Add(maVille);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                db.SaveChanges();
            }
        }  
        public static void AjoutCP()
        {
            using (var db = new garesDataModel())
            {
                string[] fichier = File.ReadAllLines("C:\\Users\\34011-14-06\\Desktop\\TP 10 n° 1\\gares_ferroviaires_de_tous_types_exploitees_ou_non.csv");

                var query = from csvline in fichier
                            let data = csvline.Split(';')

                            select new
                            {
                                codeLigne = data[0],
                                nom = data[1],
                                nature = data[2],
                                latitude = data[3],
                                longitude = data[4],
                                gps = data[5],
                                dpt = data[6],
                                cp = data[7],
                                ville = data[8]
                            };

                var filtreSurCP = from tabCP in query
                                  select tabCP.cp;

                var cpFiltre = filtreSurCP.Distinct();

                foreach (var ajout in cpFiltre)
                {
                    int nb;
                    bool resultat = Int32.TryParse(ajout, out nb);

                    if (resultat)
                    {
                        cp cp = new cp
                        {
                            code_postal = nb
                        };
                        try
                        {
                            db.cps.Add(cp);
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}

