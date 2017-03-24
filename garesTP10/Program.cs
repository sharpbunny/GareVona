using System.IO;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace garesTP10
{
    class Program
    {
        public static gare maGare { get; private set; }
        public static ville maVille { get; private set; }
        public static nature maNature { get; private set; }
        public static ligne maLigne { get; private set; }
        public static cp monCp { get; private set; }

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


                var relationVilleCP = from tabV in query
                                      select tabV;

                var filtreRelationVilleCP = relationVilleCP.Distinct();

                foreach(var affiche in filtreRelationVilleCP)
                {
                    //Console.WriteLine(affiche.cp + " " + affiche.ville);
                    db.Database.ExecuteSqlCommand($"INSERT INTO[dbo].[possede]([id_gare], [numero_nature]) VALUES({affiche.cp},{affiche.ville})");
                }
                                      
                
            }
        }
        public static void RemplirClesEtrangeresDansTablesAssociations()
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
                //-----------------------------------------------------------------------------------
                //Remplissage de la table association "possede" (gare et nature)
                //-----------------------------------------------------------------------------------

                var relationDBNature = from tabg1 in query
                                          join tabn in db.natures
                                          on tabg1.nature equals tabn.nom_nature

                                          select new { numNature = tabn.numero_nature, nomNature = tabg1.nature, nomGare = tabg1.nom };

                var relationGareNature = from tabnatg in relationDBNature
                            join tabng in db.gares
                            on tabnatg.nomGare equals tabng.nom_gare
                            select new { numg = tabng.id_gare, nomg = tabnatg.nomGare, numn = tabnatg.numNature };


                var filtreRelationGareNature = relationGareNature.Distinct();

                foreach (var affiche in filtreRelationGareNature)
                {
                    db.Database.ExecuteSqlCommand($"INSERT INTO[dbo].[possede]([id_gare], [numero_nature]) VALUES({affiche.numg},{ affiche.numn})");


                }
                //-----------------------------------------------------------------------------------
                //Remplissage de la table association "peut_contenir" (gare et ligne)
                //-----------------------------------------------------------------------------------
                var relationGare = from tab in query
                                   join tabg in db.gares
                                   on tab.nom equals tabg.nom_gare
                                   select new { numgare = tabg.id_gare, nomgare = tabg.nom_gare, codeLigne = tab.codeLigne, gps = tab.latitude };

                var relationGareLigne = from tabgare in relationGare
                                        join tabl in db.lignes
                                        on tabgare.gps equals tabl.latitude
                                        select new { nomGare = tabgare.nomgare, numGare = tabgare.numgare, numLigne = tabl.numero_ligne };

                var filtreRelationGareLigne = relationGareLigne.Distinct();

                foreach (var affiche in filtreRelationGareLigne)
                {
                    db.Database.ExecuteSqlCommand($"INSERT INTO [dbo].[peut_contenir] ([numero_ligne], [id_gare]) VALUES ({affiche.numLigne},{affiche.numGare})");
                }
                //-----------------------------------------------------------------------------------
                //Remplissage de la table association "inclus" (ville et cp)
                //-----------------------------------------------------------------------------------
            }
        }
        public static void RemplirBDD()
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

                foreach (var ajoutCP in cpFiltre)
                {
                    int nb;
                    bool resultat = Int32.TryParse(ajoutCP, out nb);

                    if (resultat)
                    {
                        monCp = new cp
                        {
                            code_postal = nb
                        };
                        try
                        {
                            db.cps.Add(monCp);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                db.SaveChanges();

                var filtreSurVille = from tabVille in query
                                     select tabVille.ville;

                var villeFiltre = filtreSurVille.Distinct();

                List<ville> listeVille = new List<ville> { };

                foreach (var ajoutVille in villeFiltre)
                {

                    maVille = new ville
                    {
                        nom_ville = ajoutVille
                       
                    };

                    try
                    {
                        db.villes.Add(maVille);
                        listeVille.Add(maVille);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                db.SaveChanges();


                var filtreSurVilleGare = from tabVille in db.villes
                                         select tabVille;

                var numeroVille = from nomVillePourGare in query
                                  join tab in filtreSurVilleGare
                                  on nomVillePourGare.ville equals tab.nom_ville
                                  select new { nomville = nomVillePourGare.ville, numerodeVillePourGare = tab.numero_ville, nomgare = nomVillePourGare.nom };

                var filtrenumeroGare = numeroVille.Distinct();

                List<gare> listeGare = new List<gare> { };
                foreach (var ajoutGare in filtrenumeroGare)
                {

                    maGare = new gare
                    {
                        nom_gare = ajoutGare.nomgare,
                        numero_ville = ajoutGare.numerodeVillePourGare
                    };

                    try
                    {
                        db.gares.Add(maGare);
                        listeGare.Add(maGare);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                db.SaveChanges();

                var filtreSurNature = from tabNature in query
                                      
                                      select tabNature.nature;

                var natureFiltre = filtreSurNature.Distinct();

                List<nature> listeNature = new List<nature> { };

                foreach (var ajoutNature in natureFiltre)
                {
                    maNature = new nature
                    {
                        nom_nature = ajoutNature
                    };

                    try
                    {
                        db.natures.Add(maNature);
                        listeNature.Add(maNature);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                db.SaveChanges();

                var filtreSurLigne = from tabLigne in query
                                     select new { ligne = tabLigne.codeLigne, latitude = tabLigne.latitude, longitude = tabLigne.longitude };

                var ligneFiltre = filtreSurLigne.Distinct();

                List<ligne> listeLigne = new List<ligne> { };

                foreach (var ajout in ligneFiltre)
                {
                    int nombreCodeLigne;
                    bool resultatNombreCodeLigne = Int32.TryParse(ajout.ligne, out nombreCodeLigne);


                    maLigne = new ligne
                    {
                        code_ligne = nombreCodeLigne,
                        latitude = ajout.latitude,
                        longitude = ajout.longitude,
                    };

                    try
                    {
                        db.lignes.Add(maLigne);
                        listeLigne.Add(maLigne);


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                db.SaveChanges();

                //monCp.villes = listeVille; //table relationnelle 'inclus'
                //maLigne.gares = listeGare; //table relationnelle 'peut_contenir'
                //maNature.gares = listeGare; //table relationnelle 'possède' 
                //db.SaveChanges();

            }
        }
        public static void ListeDeTypeDesserteFretDesGaresEtLignes()
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

                var garesLigneFret = from tabg in query
                                     join tabn in db.natures
                                     on tabg.nature equals tabn.nom_nature
                                     where tabn.numero_nature == 8
                                     orderby tabg.codeLigne
                                     select new { nomGare = tabg.nom, numLigne = tabg.codeLigne };


                Console.WriteLine("Liste des gares de type 'Desserte-fret':");
                foreach (var affiche in garesLigneFret)
                {
                    Console.WriteLine(affiche.numLigne + "    " + affiche.nomGare);
                }
            }
        }
        public static void ListeGroupeParCodeLigneDesGares()
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

                var garesParLigne = from tabl in query
                                    join tabg in db.gares
                                    on tabl.nom equals tabg.nom_gare
                                    orderby tabl.codeLigne
                                    select new { nomGare = tabg.nom_gare, codeLigne = tabl.codeLigne };

                var ligneGareGroupe = garesParLigne.GroupBy(x => new { x.codeLigne, x.nomGare });

                foreach (var affiche in ligneGareGroupe)
                {
                    Console.WriteLine(affiche.Key);
                }
            }
        }
        public static void TopDepartementSurLesGaresNonExploitees()
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

                var garesNonExploitées = from tab in query
                                         join tabn in db.natures
                                         on tab.nature equals tabn.nom_nature
                                         where tabn.numero_nature == 3
                                         orderby tab.dpt
                                         select new { codeDpt = tab.dpt, codeNature = tabn.numero_nature, nomGares = tab.nom };

                //Calcul du nombres de gares non exploitées par département.
                var nbGaresNonExploitéesParDepartement = garesNonExploitées.GroupBy(x => x.codeDpt).Select(xg => new { dpt = xg.Key, nb = xg.Count() });
                //Classement du nombre de gares par ordre décroissant.
                var classementNbGares = nbGaresNonExploitéesParDepartement.OrderByDescending(y => y.nb);
                //sélection de la premiere ligne
                var selectionNbGaresTop = classementNbGares.Take(1);

                Console.WriteLine("Le département ayant le plus de gares non exploitées\n");

                foreach (var affiche in selectionNbGaresTop)
                {
                    Console.WriteLine("Département :" + affiche.dpt + "           " + "Nb :" + affiche.nb + "\n");
                }
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
                        longitude = ajout.longitude,
                        

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
                        numero_ville = ajout.numerodeVillePourGare,
                        
                        
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

