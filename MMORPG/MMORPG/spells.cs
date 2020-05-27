using MELHARFI.Gfx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MELHARFI;
using MMORPG.Enums;

namespace MMORPG
{
    public static class spells
    {
        public static List<int> sort_d_invocation = new List<int>() { 3 };          // lister tous les sorts d'invocation pour ajouter le controle qui calcule le nombre de points d'invoc avant de lancer le sort en question
        public static List<int> sort_de_bonnus = new List<int>() { 7, 8, 9, 10 };       // lister tous les sorts qui ajoutes un bonnus ou malus ou autre ET qui utilises BonusRoundLeft pour decrementer
        public static List<int> sort_need_etat_sennin = new List<int>() { 2, 11 };  // lister tous les sorts qui dépand du mode sennin pour etre executé
        public static sort_Stats sort(int _sortID)
        {
            #region liste de tous les sorts et leurs states
            // liste de tous les sorts disponible dans le jeux
            // ATTENTION, S'IL SAGIT D'SORT D'INVOCATION, IL FAUT AJOUTER SON SortID DANS LA LISTE sort_d_invocation EN HAUT
            // sorts de class naruto
            if (_sortID == 0)
            {
                #region sort rasengan
                sort_Stats ss = new sort_Stats();
                ss.title = "Rasengan";
                ss.spellID = 0;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.futon;
                ss.positionPlayer = 0;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 5;
                isbllvl1.domMax = 8;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort
                isbllvl1.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire
                isbllvl1.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour
                isbllvl1.ligneDeVue = true;
                isbllvl1.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = false;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort
                isbllvl2.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire
                isbllvl2.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour
                isbllvl2.ligneDeVue = true;
                isbllvl2.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = false;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort
                isbllvl3.relanceParJoueur = 2;					// combien de fois le sort peux être lancé sur le même adversaire
                isbllvl3.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour
                isbllvl3.ligneDeVue = true;
                isbllvl3.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = false;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort
                isbllvl4.relanceParJoueur = 2;					// combien de fois le sort peux être lancé sur le même adversaire
                isbllvl4.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour
                isbllvl4.ligneDeVue = true;
                isbllvl4.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = false;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort
                isbllvl5.relanceParJoueur = 2;					// combien de fois le sort peux être lancé sur le même adversaire
                isbllvl5.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour
                isbllvl5.ligneDeVue = true;
                isbllvl5.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 1)
            {
                #region sort shuriken
                sort_Stats ss = new sort_Stats();
                ss.title = "Shuriken";
                ss.spellID = 1;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.neutral;
                ss.positionPlayer = 0;

                // initialisation des stats du sorts selon le lvl
                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.domMin = 5;
                isbllvl1.domMax = 8;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort
                isbllvl1.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire
                isbllvl1.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour
                isbllvl1.ligneDeVue = true;
                isbllvl1.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.ligneDeVue = true;
                isbllvl2.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 2;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.ligneDeVue = true;
                isbllvl3.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 2;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.ligneDeVue = true;
                isbllvl4.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 3;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 3;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.ligneDeVue = true;
                isbllvl5.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 2)
            {
                #region sort futon rasen shuriken
                sort_Stats ss = new sort_Stats();
                ss.title = "Rasen shuriken";
                ss.spellID = 2;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.futon;
                ss.positionPlayer = 0;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.domMin = 5;
                isbllvl1.domMax = 8;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.ligneDeVue = true;
                isbllvl1.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.ligneDeVue = true;
                isbllvl2.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.ligneDeVue = true;
                isbllvl3.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.ligneDeVue = true;
                isbllvl4.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 2;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.ligneDeVue = true;
                isbllvl5.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 3)
            {
                #region kage bunshin no jutsu
                sort_Stats ss = new sort_Stats();
                ss.title = "Kage bunshin no jutsu";
                ss.spellID = 3;
                ss.technique = "invocation";
                ss.rang = "b";
                ss.element = Enums.Chakra.Element.neutral;

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 1;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 10;
                isbllvl1.domMax = 12;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 4;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.ligneDeVue = false;
                isbllvl1.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 2;
                isbllvl2.etenduModifiable = false;
                isbllvl2.domMin = 15;
                isbllvl2.domMax = 17;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 4;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.ligneDeVue = false;
                isbllvl2.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 3;
                isbllvl3.etenduModifiable = false;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 4;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.ligneDeVue = false;
                isbllvl3.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 4;
                isbllvl4.etenduModifiable = false;
                isbllvl4.domMin = 19;
                isbllvl4.domMax = 21;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 4;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.ligneDeVue = false;
                isbllvl4.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 5;
                isbllvl5.etenduModifiable = false;
                isbllvl5.domMin = 23;
                isbllvl5.domMax = 25;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 4;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 0;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.ligneDeVue = false;
                isbllvl5.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl5);
                ///////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 4)
            {
                #region Pounch
                // sort dom terre de l'invocation clone naruto
                sort_Stats ss = new sort_Stats();
                ss.title = "Pounch";
                ss.spellID = 4;
                ss.technique = "taijutsu";
                ss.rang = "d";
                ss.element = Enums.Chakra.Element.doton;

                // l'invoc ne peux utiliser que le lvl 1 de ses sorts, pour le moment
                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 1;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 4;
                isbllvl1.domMax = 6;
                isbllvl1.cd = 40;
                isbllvl1.ligneDeVue = false;
                isbllvl1.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 1;
                isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = false;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 9;
                isbllvl2.cd = 35;
                isbllvl2.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 1;
                isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = false;
                isbllvl3.domMin = 10;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                isbllvl3.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 1;
                isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = false;
                isbllvl4.domMin = 13;
                isbllvl4.domMax = 15;
                isbllvl4.cd = 25;
                isbllvl4.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 1;
                isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = false;
                isbllvl5.domMin = 16;
                isbllvl5.domMax = 18;
                isbllvl5.cd = 20;
                isbllvl5.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl5);

                return ss;
                #endregion
            }
            else if (_sortID == 5)
            {
                #region gamabunta
                // sort dom terre qui invoc le crappeau Gamabunta qui tap en zone de 3 cases
                sort_Stats ss = new sort_Stats();
                ss.title = "Gamabunta";
                ss.spellID = 5;
                ss.technique = "ninjutsu";
                ss.rang = "d";
                ss.element = Enums.Chakra.Element.katon;

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 1;
                isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = true;
                isbllvl1.domMin = 4;
                isbllvl1.domMax = 6;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 1;
                isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = true;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 9;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 1;
                isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = true;
                isbllvl3.domMin = 10;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 1;
                isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = true;
                isbllvl4.domMin = 13;
                isbllvl4.domMax = 15;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 1;
                isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = true;
                isbllvl5.domMin = 16;
                isbllvl5.domMax = 18;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 3;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new Actor() { originalPc = 4 };
                ss.isbl.Add(isbllvl5);

                return ss;
                #endregion
            }
            else if (_sortID == 6)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "Transfert de vie";
                ss.spellID = 6;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.neutral;
                ss.positionPlayer = 0;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = true;
                isbllvl1.domMin = 0;
                isbllvl1.domMax = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = true;
                isbllvl2.domMin = 0;
                isbllvl2.domMax = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new Actor() { originalPc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = true;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 4;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new Actor() { originalPc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = true;
                isbllvl4.domMin = 0;
                isbllvl4.domMax = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 3;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new Actor() { originalPc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = true;
                isbllvl5.domMin = 0;
                isbllvl5.domMax = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 2;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new Actor() { originalPc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 7)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "Transfert de pc";
                ss.spellID = 7;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.neutral;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = true;
                isbllvl1.domMin = 0;
                isbllvl1.domMax = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = true;
                isbllvl2.domMin = 0;
                isbllvl2.domMax = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new Actor() { originalPc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = true;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new Actor() { originalPc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = true;
                isbllvl4.domMin = 0;
                isbllvl4.domMax = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new Actor() { originalPc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = true;
                isbllvl5.domMin = 0;
                isbllvl5.domMax = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new Actor() { originalPc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.originalPc = 2;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 8)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "Transfert de pm";
                ss.spellID = 8;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.neutral;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = true;
                isbllvl1.domMin = 0;
                isbllvl1.domMax = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.originalPm = 2;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = true;
                isbllvl2.domMin = 0;
                isbllvl2.domMax = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new Actor() { originalPc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.originalPm = 2;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = true;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new Actor() { originalPc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.originalPm = 2;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = true;
                isbllvl4.domMin = 0;
                isbllvl4.domMax = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new Actor() { originalPc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.originalPm = 2;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = true;
                isbllvl5.domMin = 0;
                isbllvl5.domMax = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new Actor() { originalPc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.originalPm = 2;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 9)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "Transfert de puissance";
                ss.spellID = 9;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.neutral;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = true;
                isbllvl1.domMin = 0;
                isbllvl1.domMax = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.power = 50;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = true;
                isbllvl2.domMin = 0;
                isbllvl2.domMax = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new Actor() { originalPc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.power = 100;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = true;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new Actor() { originalPc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.power = 150;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = true;
                isbllvl4.domMin = 0;
                isbllvl4.domMax = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new Actor() { originalPc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.power = 200;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = true;
                isbllvl5.domMin = 0;
                isbllvl5.domMax = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new Actor() { originalPc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.power = 250;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 10)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "Etat Sennin";
                ss.spellID = 10;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.neutral;
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 0;
                isbllvl1.etenduModifiable = false;
                isbllvl1.domMin = 0;
                isbllvl1.domMax = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 4 };
                isbllvl1.BonusRoundLeft = 3;
                isbllvl1.piBonus.doton = 50;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 0;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = false;
                isbllvl2.domMin = 0;
                isbllvl2.domMax = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new Actor() { originalPc = 4 };
                isbllvl2.BonusRoundLeft = 3;
                isbllvl2.piBonus.doton = 100;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 0;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = false;
                isbllvl3.domMin = 0;
                isbllvl3.domMax = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new Actor() { originalPc = 4 };
                isbllvl3.BonusRoundLeft = 3;
                isbllvl3.piBonus.doton = 150;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 0;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = false;
                isbllvl4.domMin = 0;
                isbllvl4.domMax = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new Actor() { originalPc = 4 };
                isbllvl4.BonusRoundLeft = 3;
                isbllvl4.piBonus.doton = 200;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 0;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = false;
                isbllvl5.domMin = 0;
                isbllvl5.domMax = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new Actor() { originalPc = 4 };
                isbllvl5.BonusRoundLeft = 3;
                isbllvl5.piBonus.doton = 250;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 11)
            {
                #region katas des crapauds
                sort_Stats ss = new sort_Stats();
                ss.title = "katas des crapauds";
                ss.spellID = 11;
                ss.technique = "Taijutsu";
                ss.rang = "a";
                ss.element = Enums.Chakra.Element.doton;

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 0;
                isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = false;
                isbllvl1.domMin = 5;
                isbllvl1.domMax = 8;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 0;
                isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = false;
                isbllvl2.domMin = 7;
                isbllvl2.domMax = 10;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;
                isbllvl2.relanceParJoueur = 1;
                isbllvl2.relanceParTour = 2;
                isbllvl2.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 0;
                isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = false;
                isbllvl3.domMin = 9;
                isbllvl3.domMax = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;
                isbllvl3.relanceParJoueur = 2;
                isbllvl3.relanceParTour = 2;
                isbllvl3.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 0;
                isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = false;
                isbllvl4.domMin = 11;
                isbllvl4.domMax = 14;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;
                isbllvl4.relanceParJoueur = 2;
                isbllvl4.relanceParTour = 3;
                isbllvl4.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 0;
                isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = false;
                isbllvl5.domMin = 13;
                isbllvl5.domMax = 16;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;
                isbllvl5.relanceParJoueur = 2;
                isbllvl5.relanceParTour = 3;
                isbllvl5.pi = new Actor() { originalPc = 3 };
                ss.isbl.Add(isbllvl5);
                return ss;
                #endregion
            }
            else
                throw new Exception("nan");
            #endregion
        }
        public static List<Point> spellPositions = new List<Point>();
        static spells()
        {
            // CONSTRUCTEUR qui contiens la position des sort pour qu'ils sois biens allignés
            // ligne 1
            spellPositions.Add(new Point(4, 4));        // case 1
            spellPositions.Add(new Point(39, 4));       // case 2
            spellPositions.Add(new Point(74, 4));       // case 3
            spellPositions.Add(new Point(109, 4));      // case 4
            spellPositions.Add(new Point(144, 4));      // case 5
            spellPositions.Add(new Point(179, 4));      // case 6
            spellPositions.Add(new Point(214, 4));      // case 7
            spellPositions.Add(new Point(249, 4));      // case 8
            spellPositions.Add(new Point(284, 4));      // case 9
            spellPositions.Add(new Point(319, 4));      // case 10

            // ligne 2
            spellPositions.Add(new Point(4, 39));        // case 11
            spellPositions.Add(new Point(39, 39));       // case 12
            spellPositions.Add(new Point(74, 39));       // case 13
            spellPositions.Add(new Point(109, 39));      // case 14
            spellPositions.Add(new Point(144, 39));      // case 15
            spellPositions.Add(new Point(179, 39));      // case 16
            spellPositions.Add(new Point(214, 39));      // case 17
            spellPositions.Add(new Point(249, 39));      // case 18
            spellPositions.Add(new Point(284, 39));      // case 19
            spellPositions.Add(new Point(319, 39));      // case 20
        }
        public static void animSpellAction(string player, Point TargetPoint, int sortID, string colorID, short sortLvl, string rawData, string UsedPoint)
        {
            Actor spellCaster = Battle.AllPlayersByOrder.Find(f => f.pseudo == player);
            // calculateDom(typeRox:rox ou heal|jet:x|cd:true ou false|chakra:futon...|dom:x|deadList:joueurMort1:joueurMort2...|roxed    séparé par # s'il sagit de plusieurs
            // methode qui anime les sort, leurs deplacement / animation / changement de l'image du joueur ...
            // la variable UsedPoint est sous la forme PcUsed:X|???:X
            // on dissocie les infos du UsedPoint
            
            List<string[]> usedPointL = new List<string[]>();
            for (int cnt = 0; cnt < UsedPoint.Split('|').Count(); cnt++)
            {
                string[] usedPointData = UsedPoint.Split('|')[cnt].Split(':');
                usedPointL.Add(usedPointData);
            }

            if (spellCaster == null || TargetPoint == null)
                return;

            CommonCode.blockNetFlow = true;
            CommonCode.ChatMsgFormat("S", "null", "blockNetFlow18 = true");
            if (sortID == 0)
            {
                #region sort rasengan
                // sort rasengan, class naruto
                // changement de l'image du joueur pour la position du lancemnt du sort
                // determination de la direction du sort
                #region redirection du personnage
                string directionOfSpell = redirectPlayer(spellCaster, TargetPoint, sortID);
                #endregion

                #region Mise en envoutement
                //////////////////////// mise en envoutement du sort
                // check si c'est un cd ou pas
                string data3 = rawData.Split('#')[0];
                string[] DomString3 = data3.Split('|');
                bool cd3 = Convert.ToBoolean(DomString3[2].Split(':')[1]);
                string _roxed = rawData.Split('|')[6];
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd3, _roxed);
                ////////////////////////////////////////////////////////////////////////
                #endregion

                #region // diminution des pv du joueur roxé
                downgradPlayerLife(player, rawData, TargetPoint, sortID);
                #endregion
                
                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[2].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion        

                #region // thread affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_Spell_Thread";
                    
                    bool createSpellOnce = false; // variable de controle pour la création du spell 1 seul fois dans la boucle while
                    bool moveSpell = true; // variable de controle pour faire avancer le sort jusqu'au joueur attaqu;
                    System.Media.SoundPlayer __hit;

                    ///////////////////
                    #region lancement de sort rasengan_concentration.wav
                    //rasengan_concentration = new System.Media.SoundPlayer(@"sfx\spell\rasengan_concentration.dat");
                    //rasengan_concentration.Play();

                    // dessin du sort
                    Point pp = spellCaster.realPosition;

                    // dessin d'une animation de rasengan
                    //Bmp __rasengan = new Bmp(@"gfx\general\sorts\" + sortID + @"\0.dat", Point.Empty, "__rasengan", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet(playerOfSpell.ClasseName + "_spell" + sortID + colorID, sortLvl - 1));
                    Bmp __rasengan = new Bmp(@"gfx\general\sorts\" + 0 + @"\0.dat", Point.Empty, "__rasengan", Manager.TypeGfx.Obj, true, 1, SpriteSheet.GetSpriteSheet("naruto_spell00", 5 - 1));

                    // mise en position du sort
                    if (directionOfSpell == "up")
                        __rasengan.point = new Point((pp.X * 30) + 15 - (__rasengan.rectangle.Width / 2), (pp.Y * 30) - 40);
                    else if (directionOfSpell == "down")
                        __rasengan.point = new Point((pp.X * 30) + 15 - (__rasengan.rectangle.Width / 2), (pp.Y * 30) + 20);
                    else if (directionOfSpell == "right")
                        __rasengan.point = new Point(((pp.X + 1) * 30) + 15 - (__rasengan.rectangle.Width / 2), (pp.Y * 30) - (__rasengan.rectangle.Height / 2) - 10);
                    else
                        __rasengan.point = new Point(((pp.X - 1) * 30) + 15 - (__rasengan.rectangle.Width / 2), (pp.Y * 30) - (__rasengan.rectangle.Height / 2) - 10);
                    Manager.manager.GfxObjList.Add(__rasengan);
                    CommonCode.VerticalSyncZindex(__rasengan);

                    if (directionOfSpell == "up")
                        __rasengan.zindex--;
                    Thread.Sleep(500);
                    createSpellOnce = true;
                    #endregion
                    ///////////////////

                    while (!Manager.manager.mainForm.IsDisposed)
                    {   
                        if (moveSpell)
                        {
                            #region // mouvement de sort jusqu'a atteindre l'adversaire en question
                            if (directionOfSpell == "left")
                            {
                                __rasengan.point.X -= 10;
                                if ((__rasengan.point.X / 30) <= TargetPoint.X)
                                {
                                    //arrivé du sort pret du joueur
                                    //centrage et mise en place du sort sur la case
                                    if (__rasengan.point.X + (__rasengan.rectangle.Width / 2) <= ((TargetPoint.X) * 30) + 15)
                                        moveSpell = false;
                                }
                            }
                            else if (directionOfSpell == "right")
                            {
                                __rasengan.point.X += 10;
                                if ((__rasengan.point.X / 30) + 1 >= TargetPoint.X)
                                {
                                    //arrivé du sort pret du joueur
                                    //centrage et mise en place du sort sur la case
                                    if (__rasengan.point.X + (__rasengan.rectangle.Width / 2) >= ((TargetPoint.X) * 30) + 15)
                                        moveSpell = false;
                                }
                            }
                            else if (directionOfSpell == "up")
                            {
                                __rasengan.point.Y -= 10;
                                if ((__rasengan.point.Y / 30) + 1 <= TargetPoint.Y)
                                    moveSpell = false;
                            }
                            else if (directionOfSpell == "down")
                            {
                                __rasengan.point.Y += 10;
                                if ((__rasengan.point.Y / 30) >= TargetPoint.Y)
                                    moveSpell = false;
                            }
                            #endregion
                        }
                        else if (createSpellOnce && !moveSpell)
                        {
                            #region changement de l'image du joueur roxé pour faire semblement de recevoir des dom
                            // faire disparaitre le resengan
                            __rasengan.visible = false;
                            Manager.manager.GfxObjList.Remove(__rasengan);

                            // affichage de l'animation de recovoir des dom
                            //////////////////////
                            // affichage des dom en haut de personnage
                            for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                            {
                                string[] data = rawData.Split('#');
                                string tmp = data[cnt].ToString();

                                string roxed = tmp.Split('|')[6];
                                if (roxed != "null") // si le sort est lancé sur une case libre
                                {
                                    Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);
                                    if (playerTargeted2 != null)
                                    {
                                        Point playerTargetedPoint = playerTargeted2.realPosition;
                                        Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                                        int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                                        new Thread(new ThreadStart(() =>
                                        {
                                            Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                            CommonCode.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                                        })).Start();
                                    }
                                }

                                string[] DomString = tmp.Split('|');
                                List<string> deadList = DomString[5].Split(':').ToList();
                                deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                    deadList.Clear();

                                // check si un ou plusieurs joueurs sont mort
                                if (deadList.Count > 0)
                                {
                                    for (int i = 0; i < deadList.Count; i++)
                                    {
                                        Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                                        if (ibPlayer != null)
                                            CommonCode.animDeadPlayer(ibPlayer);
                                    }
                                }

                                // supprimer notre joueur si la liste deadList est plus grande que 0
                                if (deadList.Count > 0)
                                {
                                    for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                    {
                                        Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                                        Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                                    }

                                    // on actualise la timeline
                                    Battle.RefreshTimeLine();
                                }
                                Thread.Sleep(100);
                            }
                            CommonCode.blockNetFlow = false;
                            CommonCode.ChatMsgFormat("S", "null", "blockNetFlow19 = false");
                            // pc utilisés
                            // on cherche si UsedPoint contiens la valeur PcUsed:PC
                            Point playerOfSpellPoint = spellCaster.realPosition;
                            Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                            int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                            if (usedPointL.Exists(f => f[0] == "PcUsed"))
                            {
                                string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                                new Thread(new ThreadStart(() =>
                                {
                                    CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                                })).Start();
                            }

                            // playe rasengan_hit.wav
                            //rasengan_hit = new System.Media.SoundPlayer(@"sfx\spell\rasengan_hit.dat");
                            __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                            __hit.Play();

                            Anim __rasengan_dom_effect = new Anim(60, 1);
                            for (int cnt = 0; cnt < 6; cnt++)
                                __rasengan_dom_effect.AddCell(@"gfx\general\obj\3\ora\2\color" + colorID + @"\" + sortLvl + @"\" + (cnt + 1) + ".dat", cnt + 1, 100, 100);

                            __rasengan_dom_effect.Ini(Manager.TypeGfx.Obj, "__rasengan_dom_effect", true);
                            __rasengan_dom_effect.img.point.X = (TargetPoint.X * 30) - (__rasengan_dom_effect.img.rectangle.Width / 2);
                            __rasengan_dom_effect.img.point.Y = (TargetPoint.Y * 30) - (__rasengan_dom_effect.img.rectangle.Height - 30) + 5 + sortLvl;
                            __rasengan_dom_effect.img.zindex = spellCaster.ibPlayer.zindex + 1;
                            __rasengan_dom_effect.PointOfParent = true;
                            __rasengan_dom_effect.AutoResetAnim = false;
                            __rasengan_dom_effect.HideAtLastFrame = true;
                            __rasengan_dom_effect.DestroyAfterLastFrame = true;
                            __rasengan_dom_effect.Start();
                            Manager.manager.GfxObjList.Add(__rasengan_dom_effect);
                            //////////////////////
                            break;
                            #endregion
                        }
                        Thread.Sleep(20);
                    }
                })).Start();
                #endregion

                replacePlayerInCorrecteCoordinate(spellCaster);
                #endregion
            }
            else if (sortID == 1)
            {
                #region sort shuriken
                #region reorientation du joueur vers l'adversaire
                string directionOfSpell = redirectPlayer(spellCaster, TargetPoint, sortID);
                #endregion
                // sort shuriken
                int xDistance = spellCaster.realPosition.X - TargetPoint.X;
                int yDistance = spellCaster.realPosition.Y - TargetPoint.Y;
                
                #region mise en envoutement du sort
                // check si c'est un cd ou pas, a assigner les envoutement systeme et non systeme comem bonnus / malus
                string data3 = rawData.Split('#')[0];
                string[] DomString3 = data3.Split('|');
                bool cd3 = Convert.ToBoolean(DomString3[2].Split(':')[1]);
                string _roxed = rawData.Split('|')[6];
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd3, _roxed);
                #endregion
                
                #region diminution des pv du joueur roxé
                downgradPlayerLife(player, rawData, TargetPoint, sortID);
                #endregion
                
                #region affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[2].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                #region // thread affichage du sort
                new Thread((() =>
                {
                    Anim __shurikenSpell = new Anim(15, 1);
                    for(int cnt = 0; cnt < 6; cnt++)
                        __shurikenSpell.AddCell(@"gfx\general\sorts\" + sortID + @"\0.dat", cnt, 0, 0, SpriteSheet.GetSpriteSheet("shuriken", cnt));

                    __shurikenSpell.Ini(Manager.TypeGfx.Obj, SpriteSheet.GetSpriteSheet("shuriken", 0), "__shuriken", true);
                    __shurikenSpell.visible(true);
                    __shurikenSpell.img.point.X = (spellCaster.realPosition.X * 30) + 15 - (__shurikenSpell.img.rectangle.Width / 2);
                    __shurikenSpell.img.point.Y = (spellCaster.realPosition.Y * 30) - (__shurikenSpell.img.rectangle.Height - 30);
                    CommonCode.VerticalSyncZindex(__shurikenSpell.img);
                    __shurikenSpell.PointOfParent = true;
                    __shurikenSpell.AutoResetAnim = true;
                    __shurikenSpell.Start();
                    Manager.manager.GfxObjList.Add(__shurikenSpell);

                    Point start = new Point((spellCaster.realPosition.X * 30) + 15 - (__shurikenSpell.img.rectangle.Width / 2), spellCaster.ibPlayer.point.Y + (spellCaster.ibPlayer.rectangle.Height / 2) - (__shurikenSpell.img.rectangle.Height / 2));
                    Point end = new Point((TargetPoint.X * 30) + 15 - (__shurikenSpell.img.rectangle.Width / 2), (TargetPoint.Y * 30) + 15 - (__shurikenSpell.img.rectangle.Height / 2));

                    int speed = (int)Math.Sqrt(Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) * 30);
                    List<PointF> waypoint = CommonCode.calculeTrajectoire(start, end, speed);
                    waypoint.Add(end);

                    System.Media.SoundPlayer __shuriken, __hit;

                    // lancement de sort shuriken.wav
                    __shuriken = new System.Media.SoundPlayer(@"sfx\spell\shuriken1.dat");
                    __shuriken.PlayLooping();

                    for (int cnt = 0; cnt < waypoint.Count && !Manager.manager.mainForm.IsDisposed; cnt++)
                    {
                        __shurikenSpell.img.point.X = (int)Math.Round(waypoint[cnt].X);
                        __shurikenSpell.img.point.Y = (int)Math.Round(waypoint[cnt].Y);
                        CommonCode.VerticalSyncZindex(__shurikenSpell.img);
                        Thread.Sleep(50);
                    }

                    __shuriken.Stop();
                    __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                    __hit.Play();

                    // supression de l'image
                    __shurikenSpell.visible(false);
                    __shurikenSpell.Close();
                    Manager.manager.GfxObjList.Remove(__shurikenSpell);

                    // affichage des dom en haut de personnage
                    for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                    {
                        string[] data = rawData.Split('#');
                        string tmp = data[cnt].ToString();

                        string roxed = tmp.Split('|')[6];
                        Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);

                        if (playerTargeted2 != null)
                        {
                            Point playerTargetedPoint = playerTargeted2.realPosition;
                            Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                            int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                            new Thread(new ThreadStart(() =>
                            {
                                Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                CommonCode.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                            })).Start();

                            string[] DomString = tmp.Split('|');
                            List<string> deadList = DomString[5].Split(':').ToList();
                            deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                            if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                deadList.Clear();

                            // check si un ou plusieurs joueurs sont mort
                            if (deadList.Count > 0)
                            {
                                for (int i = 0; i < deadList.Count; i++)
                                {
                                    Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                                    if (ibPlayer != null)
                                        CommonCode.animDeadPlayer(ibPlayer);
                                }
                            }

                            // supprimer notre joueur si la liste deadList est plus grande que 0
                            if (deadList.Count > 0)
                            {
                                for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                {
                                    // supprimer le joueur des listes
                                    Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                                    Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                    Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                    Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                    CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                                }

                                // on actualise la timeline
                                Battle.RefreshTimeLine();
                            }
                        }
                        Thread.Sleep(100);
                    }
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow20 = false");
                    // pc utilisés
                    // on cherche si UsedPoint contiens la valeur PcUsed:PC

                    Point playerOfSpellPoint = spellCaster.realPosition;
                    Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                    int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                    if (usedPointL.Exists(f => f[0] == "PcUsed"))
                    {
                        string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                        new Thread(new ThreadStart(() =>
                        {
                            CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                        })).Start();
                    }

                    // annimation sur le personnage qui recois les dom
                    Anim __animPlayerDom = new Anim(20, 1);
                    for (int cnt = 1; cnt <= 40; cnt++)
                        __animPlayerDom.AddCell(@"gfx\general\obj\3\ora\1\" + cnt + ".dat", cnt, 100, 100);
                    __animPlayerDom.Ini(Manager.TypeGfx.Obj, "__animPlayerDom", true);
                    __animPlayerDom.img.point.X = (TargetPoint.X * 30) + 15 - (__animPlayerDom.img.rectangle.Width / 2);
                    __animPlayerDom.img.point.Y = (TargetPoint.Y * 30) - (__animPlayerDom.img.rectangle.Height - 30);
                    __animPlayerDom.img.zindex = (TargetPoint.Y * 100) + 99;
                    __animPlayerDom.PointOfParent = true;
                    __animPlayerDom.AutoResetAnim = false;
                    __animPlayerDom.HideAtLastFrame = true;
                    __animPlayerDom.DestroyAfterLastFrame = true;
                    __animPlayerDom.Start();
                    Manager.manager.GfxObjList.Add(__animPlayerDom);

                    // invocation du thread principale pour modifier l'image du joueur
                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                    {
                        Point pp = new Point(spellCaster.realPosition.X, spellCaster.realPosition.Y);
                        spellCaster.ibPlayer.point.X = (pp.X * 30) + 15 - (spellCaster.ibPlayer.rectangle.Width / 2);
                        spellCaster.ibPlayer.ChangeBmp(@"gfx\general\classes\" + spellCaster.className + ".dat", SpriteSheet.GetSpriteSheet(spellCaster.className.ToString(), spellCaster.directionLook * 4));
                    }));
                })).Start();
                #endregion

                #endregion
            }
            else if (sortID == 2)
            {
                #region sort rasen shuriken
                #region changement de position du joueur
                string directionOfSpell = redirectPlayer(spellCaster, TargetPoint, sortID);
                #endregion
                #region Mise en envoutement
                // check si c'est un cd ou pas, a assigner les envoutement systeme et non systeme comem bonnus / malus
                string data3 = rawData.Split('#')[0];
                string[] DomString3 = data3.Split('|');
                bool cd3 = Convert.ToBoolean(DomString3[2].Split(':')[1]);
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd3, "null");
                #endregion

                // sort shuriken
                int xDistance = spellCaster.realPosition.X - TargetPoint.X;
                int yDistance = spellCaster.realPosition.Y - TargetPoint.Y;

                #region diminution des pv du joueur roxé
                downgradPlayerLife(player, rawData, TargetPoint, sortID);
                #endregion
                
                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[2].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                #region // thread affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_rasen_shuriken_Spell_Thread";
                    long startTime = Environment.TickCount;                     // contien le temps pour le timer créer avec une la boucle whiel

                    Anim __rasen_shuriken_Spell = new Anim(15, 1);
                    for(int cnt = 0; cnt < 14; cnt++)
                        __rasen_shuriken_Spell.AddCell(@"gfx\general\sorts\" + sortID + @"\" + sortLvl + @"\0.dat", cnt, 0, 0);
                    
                    __rasen_shuriken_Spell.Ini(Manager.TypeGfx.Obj, true);
                    __rasen_shuriken_Spell.img.zindex = spellCaster.ibPlayer.zindex;
                    __rasen_shuriken_Spell.TypeGfx = Manager.TypeGfx.Obj;
                    __rasen_shuriken_Spell.PointOfParent = true;
                    __rasen_shuriken_Spell.AutoResetAnim = true;
                    __rasen_shuriken_Spell.Start();
                    Manager.manager.GfxObjList.Add(__rasen_shuriken_Spell);

                    Point start = new Point((spellCaster.realPosition.X * 30) + 30 - (__rasen_shuriken_Spell.img.rectangle.Width / 2), spellCaster.ibPlayer.point.Y - __rasen_shuriken_Spell.img.rectangle.Height - 10);
                    Point end = new Point((TargetPoint.X * 30) + 15 - (__rasen_shuriken_Spell.img.rectangle.Width / 2), (TargetPoint.Y * 30) + 15 - (__rasen_shuriken_Spell.img.rectangle.Height / 2));
                    __rasen_shuriken_Spell.img.point = start;

                    int speed = (int)Math.Sqrt(Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) * 30) / 2;
                    List<PointF> waypoint = CommonCode.calculeTrajectoire(start, end, speed);
                    waypoint.Add(end);

                    for (int cnt = 0; cnt < waypoint.Count && !Manager.manager.mainForm.IsDisposed; cnt++)
                    {
                        __rasen_shuriken_Spell.img.point.X = (int)Math.Round(waypoint[cnt].X);
                        __rasen_shuriken_Spell.img.point.Y = (int)Math.Round(waypoint[cnt].Y);
                        CommonCode.VerticalSyncZindex(__rasen_shuriken_Spell.img);
                        Thread.Sleep(50);
                    }

                    System.Media.SoundPlayer __hit1;
                    __hit1 = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                    __hit1.Play();

                    // supression de l'image
                    __rasen_shuriken_Spell.visible(false);
                    __rasen_shuriken_Spell.Close();
                    Manager.manager.GfxObjList.Remove(__rasen_shuriken_Spell);

                    // affichage des dom en haut de personnage
                    for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                    {
                        string[] data = rawData.Split('#');
                        string tmp = data[cnt].ToString();

                        string roxed = tmp.Split('|')[6];
                        Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);

                        if (playerTargeted2 != null)
                        {
                            Point playerTargetedPoint = playerTargeted2.realPosition;
                            Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                            int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                            new Thread(new ThreadStart(() =>
                            {
                                Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                CommonCode.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                            })).Start();

                            string[] DomString = tmp.Split('|');
                            List<string> deadList = DomString[5].Split(':').ToList();
                            deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                            if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                deadList.Clear();

                            // check si un ou plusieurs joueurs sont mort
                            if (deadList.Count > 0)
                            {
                                for (int i = 0; i < deadList.Count; i++)
                                {
                                    Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                                    if (ibPlayer != null)
                                        CommonCode.animDeadPlayer(ibPlayer);
                                }
                            }

                            // supprimer notre joueur si la liste deadList est plus grande que 0
                            if (deadList.Count > 0)
                            {
                                for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                {
                                    Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                                    Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                    Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                    Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                    CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                                }

                                // on actualise la timeline
                                Battle.RefreshTimeLine();
                            }
                        }
                        Thread.Sleep(100);
                    }
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow21 = false");
                    // pc utilisés
                    // on cherche si UsedPoint contiens la valeur PcUsed:PC

                    Point playerOfSpellPoint = spellCaster.realPosition;
                    Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                    int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                    if (usedPointL.Exists(f => f[0] == "PcUsed"))
                    {
                        string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                        new Thread(new ThreadStart(() =>
                        {
                            CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                        })).Start();
                    }

                    // annimation sur le personnage qui recois les dom
                    Anim __animPlayerDom = new Anim(50, 1);
                    for (int cnt = 1; cnt < 13; cnt++)
                        __animPlayerDom.AddCell(@"gfx\general\obj\3\ora\3\" + cnt + ".dat", cnt, 100, 100);
                    __animPlayerDom.Ini(Manager.TypeGfx.Obj, "__animPlayerDom", true);
                    __animPlayerDom.img.point.X = (TargetPoint.X * 30) + 15 - (__animPlayerDom.img.rectangle.Width / 2);
                    __animPlayerDom.img.point.Y = (TargetPoint.Y * 30) - (__animPlayerDom.img.rectangle.Height - 30) + 10;

                    CommonCode.VerticalSyncZindex(__animPlayerDom.img);

                    __animPlayerDom.PointOfParent = true;
                    __animPlayerDom.AutoResetAnim = false;
                    __animPlayerDom.HideAtLastFrame = true;
                    __animPlayerDom.DestroyAfterLastFrame = true;
                    __animPlayerDom.Start();
                    Manager.manager.GfxObjList.Add(__animPlayerDom);

                    // invocation du thread principale pour modifier l'image du joueur
                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                    {
                        Point pp = new Point(spellCaster.realPosition.X, spellCaster.realPosition.Y);
                        spellCaster.ibPlayer.point.X = (pp.X * 30) + 15 - (spellCaster.ibPlayer.rectangle.Width / 2);
                        spellCaster.ibPlayer.ChangeBmp(@"gfx\general\classes\" + spellCaster.className + ".dat", SpriteSheet.GetSpriteSheet(spellCaster.className.ToString(), spellCaster.directionLook * 4));
                    }));
                })).Start();
                #endregion

                #endregion
            }
            else if (sortID == 3)
            {
            #region invocation 1 de naruto
            //buffer = "typeRox:addInvoc|" + piRaw + "|cd:" + cd;
            //string piRaw = Pseudo  :  ClasseName : Spirit : SpiritLvl : Pvp : village : MaskColors : Orientation : Level : map : rang
            // : currentHealth : totalHealth : doton : katon : futon : raiton : suiton : chakralvl2 : chakralvl3 : chakralvl4 : 
            //chakralvl5 : chakralvl6 : usingDoton : usingKaton : usingFuton : usingRaiton : usingSuiton : equipedDoton : equipedKaton : 
            //equipedFuton : equipedRaiton : suitonEquiped : original_Pc : original_Pm : pe : cd : invoc : Initiative. : resiDotonPercent
            //: resiKatonPercent : resiFutonPercent : resiRaitonPercent : resiSuitonPercent : dodgePC : dodgePM : dodgePE : dodgeCD :
            //removePC : removePM : removePE : removeCD : escape : blocage : encryptedSpellsRaw : resiDotonFix : resiKatonFix : resiFutonFix
            //: resiRaitonFix : resiSuitonFix : resiFix : domDotonFix : domKatonFix : domFutonFix : domRaitonFix : domSuitonFix : domFix
            //: power : powerEquiped

                string[] states = rawData.Split('|')[1].Split(':');

                // normalement nameOfInvoc = "name" puisque piRaw[0] = name, or nameOfInvoc dois egale piRaw[1]
                string summonName = states[0];
                Enums.ActorClass.ClassName className = (Enums.ActorClass.ClassName)Enum.Parse(typeof(Enums.ActorClass.ClassName), states[1]);
                Enums.Spirit.Name spirit = (Enums.Spirit.Name)Enum.Parse(typeof(Enums.Spirit), states[2]);
                int spiritLevel = int.Parse(states[3]);
                bool pvpEnabled = bool.Parse(states[4]);
                Enums.HiddenVillage.Names hiddenVillage = (Enums.HiddenVillage.Names)Enum.Parse(typeof(Enums.HiddenVillage.Names), states[5]);
                string maskColorsString = states[6];
                string[] maskColors = states[6].Split('/');
                int orientation = int.Parse(states[7]);
                int level = int.Parse(states[8]);
                string map = states[9];
                Enums.Rang.official officialRang = (Enums.Rang.official)Enum.Parse(typeof(Enums.Rang.official), states[10]);
                int currentHealth = int.Parse(states[11]);
                int maxHealth = int.Parse(states[12]);
                int doton = int.Parse(states[13]);
                int katon = int.Parse(states[14]);
                int futon = int.Parse(states[15]);
                int raiton = int.Parse(states[16]);
                int suiton = int.Parse(states[17]);
                int chakraDotonLevel = int.Parse(states[18]);
                int chakraKatonLevel = int.Parse(states[19]);
                int chakraFutonLevel = int.Parse(states[20]);
                int chakraRaitonLevel = int.Parse(states[21]);
                int chakraSuitonLevel = int.Parse(states[22]);
                int usingDoton = int.Parse(states[23]);
                int usingKaton = int.Parse(states[24]);
                int usingFuton = int.Parse(states[25]);
                int usingRaiton = int.Parse(states[26]);
                int usingSuiton = int.Parse(states[27]);
                int equipedDoton = int.Parse(states[28]);
                int equipedKaton = int.Parse(states[29]);
                int equipedFuton = int.Parse(states[30]);
                int equipedRaiton = int.Parse(states[31]);
                int equipedSuiton = int.Parse(states[32]);
                int originalPc = int.Parse(states[33]);
                int originalPm = int.Parse(states[34]);
                int pe = int.Parse(states[35]);
                int cd = int.Parse(states[36]);
                int summons = int.Parse(states[37]);
                int initiative = int.Parse(states[38]);
                int resiDotonPercent = int.Parse(states[39]);
                int resiKatonPercent = int.Parse(states[40]);
                int resiFutonPercent = int.Parse(states[41]);
                int resiRaitonPercent = int.Parse(states[42]);
                int resiSuitonPercent = int.Parse(states[43]);
                int dodgePC = int.Parse(states[44]);
                int dodgePM = int.Parse(states[45]);
                int dodgePE = int.Parse(states[46]);
                int dodgeCD = int.Parse(states[47]);
                int removePC = int.Parse(states[48]);
                int removePM = int.Parse(states[49]);
                int removePE = int.Parse(states[50]);
                int removeCD = int.Parse(states[51]);
                int escape = int.Parse(states[52]);
                int blocage = int.Parse(states[53]);
                string encryptedSpellRaw = states[54];
                int resiDotonFix = int.Parse(states[55]);
                int resiKatonFix = int.Parse(states[56]);
                int resiFutonFix = int.Parse(states[57]);
                int resiRaitonFix = int.Parse(states[58]);
                int resiSuitonFix = int.Parse(states[59]);
                int resiFix = int.Parse(states[60]);
                int domDotonFix = int.Parse(states[61]);
                int domKatonFix = int.Parse(states[62]);
                int domFutonFix = int.Parse(states[63]);
                int domRaitonFix = int.Parse(states[64]);
                int domSuitonFix = int.Parse(states[65]);
                int domFix = int.Parse(states[66]);
                int power = int.Parse(states[67]);
                int equipedPower = int.Parse(states[68]);

                bool cdHitTry = Convert.ToBoolean(rawData.Split('|')[2].Split(':')[1]);

                // création d'une invocation
                // orientation
                // determination de l'orientation selon les coordonées x et y
                int x = Math.Abs(TargetPoint.X - spellCaster.realPosition.X);
                int y = Math.Abs(TargetPoint.Y - spellCaster.realPosition.Y);
                //short orientation = 0;

                // longeur horizontal plus grande que longeur verticale
                if (x > y)
                {
                    // determination de l'orientation horizontal, à droite ou à gauche
                    if (TargetPoint.X > spellCaster.realPosition.X)
                        orientation = 1;    // orientation à droite
                    else
                        orientation = 3;    // orientation à gauche
                }
                else
                {
                    // determination de l'orientation horizontal, à droite ou à gauche
                    if (TargetPoint.Y > spellCaster.realPosition.Y)
                        orientation = 2;    // orientation en bas
                    else
                        orientation = 0;    // orientation en haut
                }

                System.Media.SoundPlayer __kagebunshin_no_jutsu;
                // lancement de sort kagebunshin_no_jutsu.wav
                __kagebunshin_no_jutsu = new System.Media.SoundPlayer(@"sfx\spell\kagebunshin_no_jutsu.dat");
                __kagebunshin_no_jutsu.Play();

                // animation sur le personnage qui recois les dom
                Anim __animInvoc = new Anim(70, 1);
                for (int cnt = 1; cnt <= 10; cnt++)
                    __animInvoc.AddCell(@"gfx\general\obj\3\ora\4\" + colorID + @"\" + cnt + ".dat", cnt, 100, 100);
                __animInvoc.Ini(Manager.TypeGfx.Obj, "__animInvoc", true);
                __animInvoc.img.point.X = (TargetPoint.X * 30) + 15 - (__animInvoc.img.rectangle.Width / 2);
                __animInvoc.img.point.Y = (TargetPoint.Y * 30) - (__animInvoc.img.rectangle.Height - 30);

                CommonCode.VerticalSyncZindex(__animInvoc.img);

                __animInvoc.PointOfParent = true;
                __animInvoc.AutoResetAnim = false;
                __animInvoc.HideAtLastFrame = true;
                __animInvoc.DestroyAfterLastFrame = true;
                __animInvoc.Start();
                Manager.manager.GfxObjList.Add(__animInvoc);

                //////
                Bmp ibPlayer = new Bmp(@"gfx\general\classes\" + spellCaster.className + ".dat", Point.Empty, summonName, 0, true, 1, SpriteSheet.GetSpriteSheet(spellCaster.className.ToString(), CommonCode.ConvertToClockWizeOrientation(orientation)));
                ibPlayer.MouseOver += CommonCode.ibPlayers_MouseOver;
                ibPlayer.MouseOut += CommonCode.ibPlayers_MouseOut;
                ibPlayer.MouseMove += CommonCode.CursorHand_MouseMove;
                ibPlayer.MouseClic += CommonCode.ibPlayers_MouseClic;
                Manager.manager.GfxObjList.Add(ibPlayer);

                Actor summon = new Actor();
                summon.pseudo = summonName;
                summon.className = className;
                summon.spirit = spirit;
                summon.spiritLevel = spiritLevel;
                summon.pvpEnabled = pvpEnabled;
                summon.hiddenVillage = hiddenVillage;
                summon.maskColorString = maskColorsString;
                summon.directionLook = orientation;
                summon.level = level;
                summon.map = map;
                summon.officialRang = officialRang;
                summon.currentHealth = currentHealth;
                summon.maxHealth = maxHealth;
                summon.doton = doton;
                summon.katon = katon;
                summon.futon = futon;
                summon.raiton = raiton;
                summon.suiton = suiton;
                summon.dotonChakraLevel = chakraDotonLevel;
                summon.katonChakraLevel = chakraKatonLevel;
                summon.futonChakraLevel = chakraFutonLevel;
                summon.suitonChakraLevel = chakraRaitonLevel;
                summon.raitonChakraLevel = chakraSuitonLevel;
                summon.usingDoton = usingDoton;
                summon.usingKaton = usingKaton;
                summon.usingFuton = usingFuton;
                summon.usingRaiton = usingRaiton;
                summon.usingSuiton = usingSuiton;
                summon.equipedDoton = equipedDoton;
                summon.equipedKaton = equipedKaton;
                summon.equipedFuton = equipedFuton;
                summon.equipedRaiton = equipedRaiton;
                summon.equipedSuiton = equipedSuiton;
                summon.originalPc = originalPc;
                summon.originalPm = originalPm;
                summon.pe = pe;
                summon.cd = cd;
                summon.summons = summons;
                summon.initiative = initiative;
                summon.resiDotonPercent = resiDotonPercent;
                summon.resiKatonPercent = resiKatonPercent;
                summon.resiFutonPercent = resiFutonPercent;
                summon.resiRaitonPercent = resiRaitonPercent;
                summon.resiSuitonPercent = resiSuitonPercent;
                summon.dodgePc = dodgePC;
                summon.dodgePm = dodgePM;
                summon.dodgePe = dodgePE;
                summon.dodgeCd = dodgeCD;
                summon.removePc = removePC;
                summon.removePm = removePM;
                summon.removePe = removePE;
                summon.removeCd = removeCD;
                summon.escape = escape;
                summon.blocage = blocage;
                string spellsDecoded = Cryptography.Algo.Encoding.Base64Decode(encryptedSpellRaw);
                summon.spells.Clear();
                if (spellsDecoded != "")
                {
                    for (int cnt = 0; cnt < spellsDecoded.Split('/').Length; cnt++)
                    {
                        string tmp_data = spellsDecoded.Split('/')[cnt];
                        Actor.SpellsInformations _info_sorts = new Actor.SpellsInformations();
                        _info_sorts.sortID = Convert.ToInt32(tmp_data.Split(':')[0].ToString());
                        _info_sorts.emplacement = Convert.ToInt32(tmp_data.Split(':')[1].ToString());
                        _info_sorts.level = Convert.ToInt32(tmp_data.Split(':')[2]);
                        _info_sorts.colorSort = Convert.ToInt32(tmp_data.Split(':')[3]);
                        summon.spells.Add(_info_sorts);
                    }
                }

                summon.resiDotonFix = Convert.ToInt32(states[55]);
                summon.resiKatonFix = Convert.ToInt32(states[56]);
                summon.resiFutonFix = Convert.ToInt32(states[57]);
                summon.resiRaitonFix = Convert.ToInt32(states[58]);
                summon.resiSuitonFix = Convert.ToInt32(states[59]);
                summon.resiFix = Convert.ToInt32(states[60]);
                summon.domDotonFix = Convert.ToInt32(states[61]);
                summon.domKatonFix = Convert.ToInt32(states[62]);
                summon.domFutonFix = Convert.ToInt32(states[63]);
                summon.domRaitonFix = Convert.ToInt32(states[64]);
                summon.domSuitonFix = Convert.ToInt32(states[65]);
                summon.domFix = Convert.ToInt32(states[66]);
                summon.power = Convert.ToInt32(states[67]);
                summon.equipedPower = Convert.ToInt32(states[68]);
                summon.species = Enums.Species.Name.Summon;
                summon.realPosition = TargetPoint;
                summon.ibPlayer = ibPlayer;
                ibPlayer.tag = summon;
                CommonCode.AdjustPositionAndDirection(ibPlayer, new Point(TargetPoint.X * 30, TargetPoint.Y * 30));
                CommonCode.VerticalSyncZindex(ibPlayer);

                // affichage des ailles
                if ((ibPlayer.tag as Actor).pvpEnabled == true)
                {
                    if ((ibPlayer.tag as Actor).spirit != Enums.Spirit.Name.neutral)
                    {
                        Bmp spiritBmp = new Bmp(@"gfx\general\obj\2\" + (ibPlayer.tag as Actor).spirit + @"\" + (ibPlayer.tag as Actor).spiritLevel + ".dat", Point.Empty, "spirit_" + ibPlayer.name, Manager.TypeGfx.Obj, false, 1);
                        spiritBmp.point = new Point((ibPlayer.rectangle.Width / 2) - (spiritBmp.rectangle.Width / 2), -spiritBmp.rectangle.Height);
                        ibPlayer.Child.Add(spiritBmp);

                        Txt lPseudo = new Txt("", Point.Empty, "lPseudo_" + ibPlayer.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                        // check si c'est une invoc pour afficher que la 1ere partie de son nom qui est séparé par '$' comme naruto$5d11d
                        if ((ibPlayer.tag as Actor).species == Enums.Species.Name.Summon)
                            lPseudo.Text = (ibPlayer.tag as Actor).pseudo.Split('$')[0];
                        else
                            lPseudo.Text = (ibPlayer.tag as Actor).pseudo;
                        lPseudo.point = new Point((ibPlayer.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -spiritBmp.rectangle.Height - 15);
                        ibPlayer.Child.Add(lPseudo);

                        Txt lLvlSpirit = new Txt((ibPlayer.tag as Actor).spiritLevel.ToString(), Point.Empty, "lLvlSpirit_" + ibPlayer.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Bold), Brushes.Red);
                        lLvlSpirit.point = new Point((ibPlayer.rectangle.Width / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Width / 2) + 2, -spiritBmp.rectangle.Y - (spiritBmp.rectangle.Height / 2) - (TextRenderer.MeasureText(lLvlSpirit.Text, lLvlSpirit.font).Height / 2));
                        ibPlayer.Child.Add(lLvlSpirit);

                        Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + (ibPlayer.tag as Actor).hiddenVillage, Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet("pays_" + (ibPlayer.tag as Actor).hiddenVillage + "_thumbs", 0));
                        village.point = new Point((ibPlayer.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                        ibPlayer.Child.Add(village);
                    }
                }
                else
                {
                    Txt lPseudo = new Txt((ibPlayer.tag as Actor).pseudo, Point.Empty, "lPseudo_" + ibPlayer.name, Manager.TypeGfx.Obj, false, new Font("Verdana", 10, FontStyle.Regular), Brushes.Red);
                    lPseudo.point = new Point((ibPlayer.rectangle.Width / 2) - (TextRenderer.MeasureText(lPseudo.Text, lPseudo.font).Width / 2) + 5, -15);
                    ibPlayer.Child.Add(lPseudo);

                    Bmp village = new Bmp(@"gfx\general\obj\1\pays_thumbs.dat", Point.Empty, "village_" + (ibPlayer.tag as Actor).hiddenVillage, Manager.TypeGfx.Obj, false, 1, SpriteSheet.GetSpriteSheet("pays_" + (ibPlayer.tag as Actor).hiddenVillage + "_thumbs", 0));
                    village.point = new Point((ibPlayer.rectangle.Width / 2) - (village.rectangle.Width / 2), lPseudo.point.Y - village.rectangle.Height + 2);
                    ibPlayer.Child.Add(village);
                }

                // coloriage selon le MaskColors
                if (maskColors[0] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt32(maskColors[0].Split('-')[0]), Convert.ToInt32(maskColors[0].Split('-')[1]), Convert.ToInt32(maskColors[0].Split('-')[2]));
                    CommonCode.SetPixelToClass(spellCaster.className, tmpColor, 1, ibPlayer);
                }

                if (maskColors[1] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt32(maskColors[1].Split('-')[0]), Convert.ToInt32(maskColors[1].Split('-')[1]), Convert.ToInt32(maskColors[1].Split('-')[2]));
                    CommonCode.SetPixelToClass(spellCaster.className, tmpColor, 2, ibPlayer);
                }

                if (maskColors[2] != "null")
                {
                    Color tmpColor = Color.FromArgb(Convert.ToInt32(maskColors[2].Split('-')[0]), Convert.ToInt32(maskColors[2].Split('-')[1]), Convert.ToInt32(maskColors[2].Split('-')[2]));
                    CommonCode.SetPixelToClass(spellCaster.className, tmpColor, 3, ibPlayer);
                }

                // ajout du joueur dans la liste des joueurs
                CommonCode.ApplyMaskColorToClasse(ibPlayer);
                CommonCode.AllActorsInMap.Add(ibPlayer);

                // ajouter le joueurs dans la liste de combat
                /*PlayerInfo piibt = (PlayerInfo)playerOfSpell.Clone();
                // modification des données du clone comme le nom, vita, isInvoc = false ...
                piibt.Pseudo = nameOfInvoc;
                piibt.species = PlayerInfo.Species.summon;
                piibt.current_Pm = piibt.original_Pm;
                piibt.current_Pc = piibt.original_Pc;
                //piibt.TotalPdv = lvlOfSpell;
                //piibt.CurrentPdv = lvlOfSpell;
                piibt.ibPlayer = common1.AllPlayers.Find(f => (f.tag as PlayerInfo).Pseudo == nameOfInvoc);
                piibt.ibPlayer.name = nameOfInvoc;
                piibt.realPosition = TargetPoint;
                piibt.EnvoutementsList = new List<PlayerInfo.Envoutements>();
                (piibt.ibPlayer.tag as PlayerInfo).Pseudo = nameOfInvoc;*/

                if (spellCaster.teamSide == Enums.Team.Side.A)
                {
                    Battle.SideA.Add(summon);
                }
                else if (spellCaster.teamSide == Enums.Team.Side.B)
                {
                    Battle.SideB.Add(summon);
                }
                else
                    MessageBox.Show("impossible de ne pas appartenir a une team tout de meme");

                // ajouter l'invocation a la liste des joueurs en ordre
                // determination de la position de notre joueur
                int index = Battle.AllPlayersByOrder.FindIndex(f => f.pseudo == spellCaster.pseudo);
                Battle.AllPlayersByOrder.Insert(index + 1, summon);

                // pc utilisés
                // on cherche si UsedPoint contiens la valeur PcUsed:PC

                /*Point playerOfSpellPoint = playerOfSpell.realPosition;
                Rectangle playerOfSpellRectangle = playerOfSpell.ibPlayer.rectangle;
                int playerOfSpellZindex = playerOfSpell.ibPlayer.zindex;*/

                if (usedPointL.Exists(f => f[0] == "PcUsed"))
                {
                    string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                    new Thread(new ThreadStart(() =>
                    {
                        CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), spellCaster.realPosition, spellCaster.ibPlayer.rectangle, spellCaster.ibPlayer.zindex);
                    })).Start();
                }

                #region Mise en envoutement
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cdHitTry, "null");
                #endregion
                CommonCode.ChatMsgBattleFormat("spell", player, rawData, TargetPoint, sortID);

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                ShowCDabovePlayer(spellCaster, cdHitTry);
                #endregion

                // effacement des cadront des joueurs dans la timeline
                Battle.RefreshTimeLine();
                // libération et purgage de la liste des cmd
                CommonCode.blockNetFlow = false;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow10 = false");
                #endregion
            }
            else if (sortID == 4)
            {
                #region sort pounch de l'invoc 1 de naruto kagebunshin no jutsu
                #region changement de position du joueur
                string directionOfSpell = redirectPlayer(spellCaster, TargetPoint, sortID);
                #endregion
                // sort pounch
                int xDistance = spellCaster.realPosition.X - TargetPoint.X;
                int yDistance = spellCaster.realPosition.Y - TargetPoint.Y;

                #region diminution des pv du joueur roxé
                downgradPlayerLife(player, rawData, TargetPoint, sortID);
                #endregion
                //
                #region affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__draw_ID4_Spell_Thread";
                    long startTime = Environment.TickCount;                     // contien le temps pour le timer créer avec une la boucle whiel

                    // affichage des dom en haut de personnage
                    for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                    {
                        string[] data = rawData.Split('#');
                        string tmp = data[cnt].ToString();

                        // on initialise les variables avec quoi le thread va treater, parsque l'objet du joueur en question peux etre suprimé parsqu'il est mort
                        string roxed = tmp.Split('|')[6];
                        Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);

                        Point playerTargetedPoint = playerTargeted2.realPosition;
                        Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                        int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                        new Thread(new ThreadStart(() =>
                        {
                            Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                            CommonCode.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                        })).Start();

                        #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                        string data2 = rawData.Split('#')[0];
                        string[] DomString2 = data2.Split('|');
                        bool cd2 = Convert.ToBoolean(DomString2[2].Split(':')[1]);
                        ShowCDabovePlayer(spellCaster, cd2);
                        #endregion

                        #region // son hit
                        System.Media.SoundPlayer _hit1;
                        // lancement de sort shuriken.wav
                        _hit1 = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                        _hit1.Play();
                        #endregion

                        string[] DomString = tmp.Split('|');
                        List<string> deadList = DomString[5].Split(':').ToList();
                        deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                        if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                            deadList.Clear();
                        if (deadList.Count == 1 && deadList[0] == "")
                            deadList.Clear();

                        // check si un ou plusieurs joueurs sont mort
                        if (deadList.Count > 0)
                        {
                            for (int i = 0; i < deadList.Count; i++)
                            {
                                Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                                if (ibPlayer != null)
                                    CommonCode.animDeadPlayer(ibPlayer);
                            }
                        }

                        // supprimer notre joueur si la liste deadList est plus grande que 0
                        if (deadList.Count > 0)
                        {
                            for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                            {
                                Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                                Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                            }

                            // on actualise la timeline
                            Battle.RefreshTimeLine();
                        }

                        Thread.Sleep(100);
                    }
                    CommonCode.blockNetFlow = false;
                    CommonCode.ChatMsgFormat("S", "null", "blockNetFlow11 = false");
                    // invocation du thread principale pour modifier l'image du joueur afn de changer son orientation
                    Manager.manager.mainForm.BeginInvoke((Action)(() =>
                    {
                        Point pp = new Point(spellCaster.realPosition.X, spellCaster.realPosition.Y);
                        spellCaster.ibPlayer.point.X = (pp.X * 30) + 15 - (spellCaster.ibPlayer.rectangle.Width / 2);
                        spellCaster.ibPlayer.ChangeBmp(@"gfx\general\classes\" + spellCaster.className + ".dat", SpriteSheet.GetSpriteSheet(spellCaster.className.ToString(), spellCaster.directionLook * 4));
                    }));

                    // pc utilisés
                    // on cherche si UsedPoint contiens la valeur PcUsed:PC

                    Point playerOfSpellPoint = spellCaster.realPosition;
                    Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                    int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                    if (usedPointL.Exists(f => f[0] == "PcUsed"))
                    {
                        string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                        new Thread(new ThreadStart(() =>
                        {
                            CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                        })).Start();
                    }

                    // annimation sur le personnage qui recois les dom
                    if (rawData != "null")
                    {
                        Anim __animPlayerDom = new Anim(20, 1);
                        for (int cnt = 1; cnt <= 40; cnt++)
                            __animPlayerDom.AddCell(@"gfx\general\obj\3\ora\1\" + cnt + ".dat", cnt, 100, 100);
                        __animPlayerDom.Ini(Manager.TypeGfx.Obj, "__animPlayerDom", true);
                        __animPlayerDom.img.point.X = (TargetPoint.X * 30) + 15 - (__animPlayerDom.img.rectangle.Width / 2);
                        __animPlayerDom.img.point.Y = (TargetPoint.Y * 30) - (__animPlayerDom.img.rectangle.Height - 30);
                        __animPlayerDom.img.zindex = (TargetPoint.Y * 100) + 99;
                        __animPlayerDom.PointOfParent = true;
                        __animPlayerDom.AutoResetAnim = false;
                        __animPlayerDom.HideAtLastFrame = true;
                        __animPlayerDom.DestroyAfterLastFrame = true;
                        __animPlayerDom.Start();
                        Manager.manager.GfxObjList.Add(__animPlayerDom);
                    }
                })).Start();
                #endregion
                #endregion
            }
            else if (sortID == 5)
            {
                #region sort gamabunta
                #region determination de la direction du sort
                string directionOfSpell = redirectPlayer(spellCaster, TargetPoint, sortID);
                #endregion

                #region Mise en envoutement
                // check si c'est un cd ou pas, a assigner les envoutement systeme et non systeme comem bonnus / malus
                string data3 = rawData.Split('#')[0];
                string[] DomString3 = data3.Split('|');
                // CD se trouve a l'occurance DomString3[2] lorsqu'il s'agit de typeRox:rox, et quand c'est typeRox:Invoc ou Desinvoc Cd se trouve à DomString3[1]
                bool cd3 = Convert.ToBoolean(DomString3[2].Split(':')[1]);
                //////////////////////// mise en envoutement du sort
                string _roxed = rawData.Split('|')[6];
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd3, _roxed);
                #endregion
                
                #region diminution des pv du joueur roxé
                downgradPlayerLife(player, rawData, TargetPoint, sortID);
                #endregion
                
                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[2].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                #region // thread affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_Spell_Thread";
                    Anim __rasengan = new Anim(30, 1);
                    long startTime = Environment.TickCount; // contien le temps pour le timer créer avec une la boucle whiel
                    System.Media.SoundPlayer __hit;

                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        #region changement de l'image du joueur roxé pour faire semblement de recevoir des dom

                        // affichage des dom en haut de personnage
                        for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                        {
                            string[] data = rawData.Split('#');
                            string tmp = data[cnt].ToString();
                                
                            // animation dom
                            Anim __gamabunta_dom_effect = new Anim(15, 1);
                            for (int cnt2 = 0; cnt2 < 30; cnt2++ )
                                __gamabunta_dom_effect.AddCell(@"gfx\general\sorts\5\color" + colorID + @"\lvl" + sortLvl + @"\" + cnt2 + ".dat", 0, 100, 100);

                            __gamabunta_dom_effect.Ini(Manager.TypeGfx.Obj, "__gamabunta_dom_effect", true);
                            __gamabunta_dom_effect.img.point.X = (TargetPoint.X * 30) + 15 - (__gamabunta_dom_effect.img.rectangle.Width / 2);
                            __gamabunta_dom_effect.img.point.Y = (TargetPoint.Y * 30) + 30 - __gamabunta_dom_effect.img.rectangle.Height;
                            __gamabunta_dom_effect.img.zindex = CommonCode.VerticalSyncZindex(TargetPoint.Y, Manager.TypeGfx.Obj);
                            __gamabunta_dom_effect.PointOfParent = true;
                            __gamabunta_dom_effect.AutoResetAnim = false;
                            __gamabunta_dom_effect.HideAtLastFrame = true;
                            __gamabunta_dom_effect.DestroyAfterLastFrame = true;
                            __gamabunta_dom_effect.Start();
                            Manager.manager.GfxObjList.Add(__gamabunta_dom_effect);
                            ////////////////////////////////////////////////////////

                            string roxed = tmp.Split('|')[6];
                            Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);

                            if (playerTargeted2 != null)
                            {
                                Point playerTargetedPoint = playerTargeted2.realPosition;
                                Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                                int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                                new Thread(new ThreadStart(() =>
                                {
                                    Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                    CommonCode.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                                })).Start();

                                string[] DomString = tmp.Split('|');
                                List<string> deadList = DomString[5].Split(':').ToList();
                                deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                    deadList.Clear();

                                // check si un ou plusieurs joueurs sont mort
                                if (deadList.Count > 0)
                                {
                                    for (int i = 0; i < deadList.Count; i++)
                                    {
                                        Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                                        if (ibPlayer != null)
                                            CommonCode.animDeadPlayer(ibPlayer);
                                    }
                                }

                                // supprimer notre joueur si la liste deadList est plus grande que 0
                                if (deadList.Count > 0)
                                {
                                    for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                    {
                                        Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                                        Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                                    }

                                    // on actualise la timeline
                                    Battle.RefreshTimeLine();
                                }
                            }
                            Thread.Sleep(100);
                        }
                        CommonCode.blockNetFlow = false;
                        CommonCode.ChatMsgFormat("S", "null", "blockNetFlow12 = false");
                        // pc utilisés
                        // on cherche si UsedPoint contiens la valeur PcUsed:PC
                        Point playerOfSpellPoint = spellCaster.realPosition;
                        Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                        int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                        if (usedPointL.Exists(f => f[0] == "PcUsed"))
                        {
                            string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                            new Thread(new ThreadStart(() =>
                            {
                                CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                            })).Start();
                        }

                        __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                        __hit.Play();

                        break;
                        #endregion
                    }
                })).Start();
                #endregion

                replacePlayerInCorrecteCoordinate(spellCaster);

                #endregion
            }
            else if (sortID == 6)
            {
                #region transfert de vie
                // "typeRox:desinvocation|cd:" + cdAllowed + "|chakra:neutral|deadList:" + playerDead + "|" + healedString
                //cmd•spellTileGranted•narutox•6•19•8•0•5•typeRox:desinvocation|cd:False|chakra:neutral|deadList:narutox$ddir8|narutox$hi9ct:125:236:244;health|narutox:0:500:500•PcUsed:4
                // healedString = "null" ou playersInArea[cnt].Pseudo + ":" + reliquat + ":" + playersInArea[cnt].CurrentPdv + ":" + playersInArea[cnt].totalPdv + ":health|";
                // CD se trouve a l'occurance DomString3[2] lorsqu'il s'agit de typeRox:rox, et quand c'est typeRox:Invoc ou Desinvoc Cd se trouve à DomString3[1]
                bool cd = Convert.ToBoolean(rawData.Split('|')[1].Split(':')[1]);

                System.Media.SoundPlayer __hit;
                // lancement de sort kagebunshin_no_jutsu.wav
                __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                __hit.Play();

                // desinvocation du clone
                // affichage de l'animation de recovoir des dom
                //////////////////////
                #region affichage des dom en haut de personnage
                for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                {
                    string[] data = rawData.Split('#');
                    string tmp = data[cnt].ToString();

                    string[] DomString = tmp.Split('|');
                    List<string> deadList = DomString[3].Split(':').ToList();
                    deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                    if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                        deadList.Clear();

                    // check si un ou plusieurs joueurs sont mort
                    if (deadList.Count > 0)
                    {
                        for (int i = 0; i < deadList.Count; i++)
                        {
                            Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                            if (ibPlayer != null)
                                CommonCode.animDeadPlayer(ibPlayer);
                        }
                    }

                    // supprimer notre joueur si la liste deadList est plus grande que 0
                    if (deadList.Count > 0)
                    {
                        for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                        {
                            Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                            Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                        }

                        // on actualise la timeline
                        Battle.RefreshTimeLine();
                    }
                    
                    // transfert de vie
                    string healedString = tmp.Split('|')[4];
                    if (healedString != "null")
                    {
                        for (int cnt1 = 0; cnt1 < healedString.Split('/').Count(); cnt1++)
                        {
                            string tmpHealedString = healedString.Split('/')[cnt1];
                            Actor healed = Battle.AllPlayersByOrder.Find(f => f.pseudo == tmpHealedString.Split(':')[0]);
                            int heal = Convert.ToInt32(tmpHealedString.Split(':')[1]);
                            int currentPdv = Convert.ToInt32(tmpHealedString.Split(':')[2]);
                            int totalPdv = Convert.ToInt32(tmpHealedString.Split(':')[3]);

                            Point playerOfSpellPoint1 = healed.realPosition;
                            Rectangle playerOfSpellRectangle1 = healed.ibPlayer.rectangle;
                            int playerOfSpellZindex1 = healed.ibPlayer.zindex;

                            healed.currentHealth = currentPdv;
                            healed.maxHealth = totalPdv;

                            new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(500);  // pour que l'ecriture ne sois pas aligné horizontalement avec les pc utilisé, du coup on vois pas
                                CommonCode.showAnimUpgradeVita(heal, playerOfSpellPoint1, playerOfSpellRectangle1, playerOfSpellZindex1);
                            })).Start();
                            HudHandle.UpdateHealth();
                        }
                    }

                    Thread.Sleep(100);
                }
                #endregion
                // pc utilisés
                // on cherche si UsedPoint contiens la valeur PcUsed:PC

                Point playerOfSpellPoint = spellCaster.realPosition;
                Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                if (usedPointL.Exists(f => f[0] == "PcUsed"))
                {
                    string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                    new Thread(new ThreadStart(() =>
                    {
                        CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                    })).Start();
                }

                #region Mise en envoutement
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd, "null");
                #endregion
                CommonCode.ChatMsgBattleFormat("spell", player, rawData, TargetPoint, sortID);

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[1].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                // effacement des cadront des joueurs dans la timeline
                Battle.RefreshTimeLine();
                // libération et purgage de la liste des cmd
                CommonCode.blockNetFlow = false;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow13 = false");
                #endregion
            }
            else if (sortID == 7)
            {
                #region transfert de pc
                // "typeRox:desinvocation|cd:" + cdAllowed + "|chakra:neutral|deadList:" + playerDead + "|" + healedString
                // healedString = "null" ou playersInArea[cnt].Pseudo + ":" + pc + ":" + playersInArea[cnt].pc + ":" + "|";

                bool cd = Convert.ToBoolean(rawData.Split('|')[1].Split(':')[1]);

                System.Media.SoundPlayer __hit;
                // lancement de sort kagebunshin_no_jutsu.wav
                __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                __hit.Play();

                // desinvocation du clone
                // affichage de l'animation de recovoir des dom
                //////////////////////
                #region affichage des dom en haut de personnage
                for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                {
                    string[] data = rawData.Split('#');
                    string tmp = data[cnt].ToString();

                    string[] DomString = tmp.Split('|');
                    List<string> deadList = DomString[3].Split(':').ToList();
                    deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                    if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                        deadList.Clear();

                    // check si un ou plusieurs joueurs sont mort
                    if (deadList.Count > 0)
                    {
                        for (int i = 0; i < deadList.Count; i++)
                        {
                            Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                            if (ibPlayer != null)
                                CommonCode.animDeadPlayer(ibPlayer);
                        }
                    }

                    // supprimer notre joueur si la liste deadList est plus grande que 0
                    if (deadList.Count > 0)
                    {
                        for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                        {
                            Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                            Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                        }

                        // on actualise la timeline
                        Battle.RefreshTimeLine();
                    }
                    //cmd•spellTileGranted•narutox•6•19•8•0•5•typeRox:desinvocation|cd:False|chakra:neutral|deadList:narutox$ddir8|narutox$hi9ct:125:236:244|narutox:0:500:500•PcUsed:4
                    // transfert de vie
                    string PcString = tmp.Split('|')[4];
                    if (PcString != "null")
                    {
                        for (int cnt1 = 0; cnt1 < PcString.Split('/').Count(); cnt1++)
                        {
                            string tmpPcString = PcString.Split('/')[cnt1];
                            Actor PlayerGotPc = Battle.AllPlayersByOrder.Find(f => f.pseudo == tmpPcString.Split(':')[0]);
                            int PcAdded = Convert.ToInt32(tmpPcString.Split(':')[1]);
                            int currentPc = Convert.ToInt32(tmpPcString.Split(':')[2]);

                            Point playerOfSpellPoint1 = PlayerGotPc.realPosition;
                            Rectangle playerOfSpellRectangle1 = PlayerGotPc.ibPlayer.rectangle;
                            int playerOfSpellZindex1 = PlayerGotPc.ibPlayer.zindex;

                            // ajout des PC
                            PlayerGotPc.originalPc += PcAdded;
                            PlayerGotPc.currentPc += PcAdded;
                            if (PlayerGotPc.originalPc != currentPc)
                                MessageBox.Show("pc client different du pc serveur,[client = " + PlayerGotPc.originalPc + "][Server = " + currentPc + "]");
                            new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(500);  // pour que l'ecriture ne sois pas aligné horizontalement avec les pc utilisé, du coup on vois pas
                                CommonCode.showAnimUpgrade(PcAdded, playerOfSpellPoint1, playerOfSpellRectangle1, playerOfSpellZindex1, "pc");
                            })).Start();
                        }

                        #region Mise en envoutement visible non systeme pour ajout des PC au joueurs concernés
                        for (int cnt2 = 0; cnt2 < PcString.Split('/').Count(); cnt2++)
                        {
                            string gotPcPseudo = PcString.Split('/')[cnt2].Split(':')[0];
                            int bonnusPc = Convert.ToInt32(PcString.Split('/')[cnt2].Split(':')[1]);
                            int totalPc = Convert.ToInt32(PcString.Split('/')[cnt2].Split(':')[2]);
                            Actor piGotPc = Battle.AllPlayersByOrder.Find(f => f.pseudo == gotPcPseudo);

                            if (piGotPc.BuffsList.Exists(f => f.SortID == sortID && !f.systeme))
                            {
                                Actor.Buff piEnv = piGotPc.BuffsList.Find(f => f.SortID == sortID && !f.systeme);
                                piEnv.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv.playerRoxed.Add("null");      // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                            }
                            else
                            {
                                // ajout du sort dans les envoutements
                                Actor.Buff piEnv1 = new Actor.Buff();
                                piEnv1.SortID = sortID;
                                piEnv1.title = sort(sortID).title;
                                piEnv1.Debuffable = true;
                                piEnv1.visibleToPlayers = true;
                                piEnv1.playerRoxed.Add("null");     // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                                piEnv1.relanceInterval = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;     // relanceInterval egale le bonus, pour que l'envoutement s'expire a la fin du bonus et non a la fin du vrais relanceInterval du sort vus que ce dernier est plus grand que le 1er
                                piEnv1.BuffState = Buff.State.Fin;
                                piEnv1.relanceParTour = 1;
                                piEnv1.systeme = false;
                                piEnv1.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv1.Bonus.originalPc = sort(sortID).isbl[sortLvl - 1].piBonus.originalPc;
                                piEnv1.Bonus.currentPc = sort(sortID).isbl[sortLvl - 1].piBonus.currentPc;
                                piEnv1.Cd = cd;
                                piEnv1.player = player;
                                piGotPc.BuffsList.Add(piEnv1);
                            }
                        }
                        #endregion
                    }

                    Thread.Sleep(100);
                }
                #endregion
                // pc utilisés
                // on cherche si UsedPoint contiens la valeur PcUsed:PC

                Point playerOfSpellPoint = spellCaster.realPosition;
                Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                if (usedPointL.Exists(f => f[0] == "PcUsed"))
                {
                    string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                    new Thread(new ThreadStart(() =>
                    {
                        CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                    })).Start();
                }

                #region Mise en envoutement system
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd, "null");
                #endregion
                // envoutement visible a été fait avant puisqu'il ya une possibilité que plusieurs joueurs sois affecté par cet envoutement de zone

                // BONUS +2PC
                CommonCode.ChatMsgBattleFormat("spell", player, rawData, TargetPoint, sortID);

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[1].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                // effacement des cadront des joueurs dans la timeline
                Battle.RefreshTimeLine();
                // libération et purgage de la liste des cmd
                CommonCode.blockNetFlow = false;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow14 = false");
                #endregion
            }
            else if (sortID == 8)
            {
                #region transfert de pm
                // "typeRox:desinvocation|cd:" + cdAllowed + "|chakra:neutral|deadList:" + playerDead + "|" + healedString
                // healedString = "null" ou playersInArea[cnt].Pseudo + ":" + pc + ":" + playersInArea[cnt].pc + ":" + "|";

                bool cd = Convert.ToBoolean(rawData.Split('|')[1].Split(':')[1]);

                System.Media.SoundPlayer __hit;
                // lancement de sort kagebunshin_no_jutsu.wav
                __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                __hit.Play();

                // desinvocation du clone
                // affichage de l'animation de recovoir des dom
                //////////////////////
                #region affichage des dom en haut de personnage
                for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                {
                    string[] data = rawData.Split('#');
                    string tmp = data[cnt].ToString();

                    string[] DomString = tmp.Split('|');
                    List<string> deadList = DomString[3].Split(':').ToList();
                    deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                    if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                        deadList.Clear();

                    // check si un ou plusieurs joueurs sont mort
                    if (deadList.Count > 0)
                    {
                        for (int i = 0; i < deadList.Count; i++)
                        {
                            Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                            if (ibPlayer != null)
                                CommonCode.animDeadPlayer(ibPlayer);
                        }
                    }

                    // supprimer notre joueur si la liste deadList est plus grande que 0
                    if (deadList.Count > 0)
                    {
                        for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                        {
                            Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                            Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                        }

                        // on actualise la timeline
                        Battle.RefreshTimeLine();
                    }
                    //cmd•spellTileGranted•narutox•6•19•8•0•5•typeRox:desinvocation|cd:False|chakra:neutral|deadList:narutox$ddir8|narutox$hi9ct:125:236:244|narutox:0:500:500•PcUsed:4
                    // transfert de pm
                    string PmString = tmp.Split('|')[4];
                    if (PmString != "null")
                    {
                        for (int cnt1 = 0; cnt1 < PmString.Split('/').Count(); cnt1++)
                        {
                            string tmpPcString = PmString.Split('/')[cnt1];
                            Actor PlayerGotPm = Battle.AllPlayersByOrder.Find(f => f.pseudo == tmpPcString.Split(':')[0]);
                            int PmAdded = Convert.ToInt32(tmpPcString.Split(':')[1]);
                            int currentPm = Convert.ToInt32(tmpPcString.Split(':')[2]);

                            Point playerOfSpellPoint1 = PlayerGotPm.realPosition;
                            Rectangle playerOfSpellRectangle1 = PlayerGotPm.ibPlayer.rectangle;
                            int playerOfSpellZindex1 = PlayerGotPm.ibPlayer.zindex;

                            // ajout des PM
                            PlayerGotPm.originalPm += PmAdded;
                            PlayerGotPm.currentPm += PmAdded;
                            if (PlayerGotPm.originalPm != currentPm)
                                MessageBox.Show("pm client different du pm serveur,[client = " + PlayerGotPm.originalPm + "][Server = " + currentPm + "]");
                            new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(500);  // pour que l'ecriture ne sois pas aligné horizontalement avec les pc utilisé, du coup on vois pas
                                CommonCode.showAnimUpgrade(PmAdded, playerOfSpellPoint1, playerOfSpellRectangle1, playerOfSpellZindex1, "pm");
                            })).Start();
                        }

                        #region envoutement visible non systeme pour ajout de PM
                        for (int cnt2 = 0; cnt2 < PmString.Split('/').Count(); cnt2++)
                        {
                            string gotPmPseudo = PmString.Split('/')[cnt2].Split(':')[0];
                            int bonnusPm = Convert.ToInt32(PmString.Split('/')[cnt2].Split(':')[1]);
                            int totalPm = Convert.ToInt32(PmString.Split('/')[cnt2].Split(':')[2]);
                            Actor piGotPm = Battle.AllPlayersByOrder.Find(f => f.pseudo == gotPmPseudo);
                            
                            if (piGotPm.BuffsList.Exists(f => f.SortID == sortID && !f.systeme))
                            {
                                Actor.Buff piEnv = piGotPm.BuffsList.Find(f => f.SortID == sortID && !f.systeme);
                                piEnv.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv.playerRoxed.Add("null");      // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                            }
                            else
                            {
                                // ajout du sort dans les envoutements
                                Actor.Buff piEnv1 = new Actor.Buff();
                                piEnv1.SortID = sortID;
                                piEnv1.title = sort(sortID).title;
                                piEnv1.Debuffable = true;
                                piEnv1.visibleToPlayers = true;
                                piEnv1.playerRoxed.Add("null");     // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                                piEnv1.relanceInterval = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;     // relanceInterval egale le bonus, pour que l'envoutement s'expire a la fin du bonus et non a la fin du vrais relanceInterval du sort vus que ce dernier est plus grand que le 1er
                                piEnv1.BuffState = Buff.State.Fin;
                                piEnv1.relanceParTour = 1;
                                piEnv1.systeme = false;
                                piEnv1.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv1.Bonus.originalPm = sort(sortID).isbl[sortLvl - 1].piBonus.originalPm;
                                piEnv1.Bonus.currentPm = sort(sortID).isbl[sortLvl - 1].piBonus.currentPm;
                                piEnv1.Cd = cd;
                                piEnv1.player = player;
                                piGotPm.BuffsList.Add(piEnv1);
                            }
                        }
                        #endregion
                    }
                    Thread.Sleep(100);
                }
                #endregion
                // pc utilisés
                // on cherche si UsedPoint contiens la valeur PcUsed:PC

                Point playerOfSpellPoint = spellCaster.realPosition;
                Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                if (usedPointL.Exists(f => f[0] == "PcUsed"))
                {
                    string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                    new Thread(new ThreadStart(() =>
                    {
                        CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                    })).Start();
                }

                #region Mise en envoutement system NON VISIBLE
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd, "null");
                #endregion
                // envoutement visible a été fait avant puisqu'il ya une possibilité que plusieurs joueurs sois affecté par cet envoutement de zone

                // BONUS +2PM

                CommonCode.ChatMsgBattleFormat("spell", player, rawData, TargetPoint, sortID);

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[1].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                // effacement des cadront des joueurs dans la timeline
                Battle.RefreshTimeLine();

                // libération et purgage de la liste des cmd
                CommonCode.blockNetFlow = false;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow15 = false");
                #endregion
            }
            else if (sortID == 9)
            {
                #region transfert de puissance
                // "typeRox:desinvocation|cd:" + cdAllowed + "|chakra:neutral|deadList:" + playerDead + "|" + healedString
                // healedString = "null" ou playersInArea[cnt].Pseudo + ":" + pc + ":" + playersInArea[cnt].pc + ":" + "|";

                bool cd = Convert.ToBoolean(rawData.Split('|')[1].Split(':')[1]);

                System.Media.SoundPlayer __hit;
                // lancement de sort kagebunshin_no_jutsu.wav
                __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                __hit.Play();

                // desinvocation du clone
                // affichage de l'animation de recovoir des dom
                //////////////////////
                #region affichage des dom en haut de personnage
                for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                {
                    string[] data = rawData.Split('#');
                    string tmp = data[cnt].ToString();

                    string[] DomString = tmp.Split('|');
                    List<string> deadList = DomString[3].Split(':').ToList();
                    deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                    if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                        deadList.Clear();

                    // check si un ou plusieurs joueurs sont mort
                    if (deadList.Count > 0)
                    {
                        for (int i = 0; i < deadList.Count; i++)
                        {
                            Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                            if (ibPlayer != null)
                                CommonCode.animDeadPlayer(ibPlayer);
                        }
                    }

                    // supprimer notre joueur si la liste deadList est plus grande que 0
                    if (deadList.Count > 0)
                    {
                        for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                        {
                            Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                            Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                        }

                        // on actualise la timeline
                        Battle.RefreshTimeLine();
                    }
                    //cmd•spellTileGranted•narutox•6•19•8•0•5•typeRox:desinvocation|cd:False|chakra:neutral|deadList:narutox$ddir8|narutox$hi9ct:125:236:244|narutox:0:500:500•PcUsed:4
                    // transfert de puissance
                    string PuissanceString = tmp.Split('|')[4];
                    if (PuissanceString != "null")
                    {
                        for (int cnt1 = 0; cnt1 < PuissanceString.Split('/').Count(); cnt1++)
                        {
                            string tmpPpuissanceString = PuissanceString.Split('/')[cnt1];
                            Actor PlayerGotPuissance = Battle.AllPlayersByOrder.Find(f => f.pseudo == tmpPpuissanceString.Split(':')[0]);
                            int PuissanceAdded = Convert.ToInt32(tmpPpuissanceString.Split(':')[1]);
                            int currentPuissance = Convert.ToInt32(tmpPpuissanceString.Split(':')[2]);

                            Point playerOfSpellPoint1 = PlayerGotPuissance.realPosition;
                            Rectangle playerOfSpellRectangle1 = PlayerGotPuissance.ibPlayer.rectangle;
                            int playerOfSpellZindex1 = PlayerGotPuissance.ibPlayer.zindex;

                            // ajout de la puissance
                            PlayerGotPuissance.power += PuissanceAdded;
                            if (PlayerGotPuissance.power != currentPuissance)
                                MessageBox.Show("puissance client different du puissance serveur,[client = " + PlayerGotPuissance.power + "][Server = " + currentPuissance + "]");
                            new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(500); // pour que l'ecriture ne sois pas aligné horizontalement avec les pc utilisé, du coup on vois pas
                                CommonCode.showAnimUpgrade(PuissanceAdded, playerOfSpellPoint1, playerOfSpellRectangle1, playerOfSpellZindex1, "puissance");
                            })).Start();
                        }

                        #region envoutement visible non systeme pour ajout de PM
                        for (int cnt2 = 0; cnt2 < PuissanceString.Split('/').Count(); cnt2++)
                        {
                            string gotPuissancePseudo = PuissanceString.Split('/')[cnt2].Split(':')[0];
                            int bonnusPuissance = Convert.ToInt32(PuissanceString.Split('/')[cnt2].Split(':')[1]);
                            int totalPuissance = Convert.ToInt32(PuissanceString.Split('/')[cnt2].Split(':')[2]);
                            Actor piGotPuissance = Battle.AllPlayersByOrder.Find(f => f.pseudo == gotPuissancePseudo);

                            if (piGotPuissance.BuffsList.Exists(f => f.SortID == sortID && !f.systeme))
                            {
                                Actor.Buff piEnv = piGotPuissance.BuffsList.Find(f => f.SortID == sortID && !f.systeme);
                                piEnv.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv.playerRoxed.Add("null");      // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                            }
                            else
                            {
                                // ajout du sort dans les envoutements
                                Actor.Buff piEnv1 = new Actor.Buff();
                                piEnv1.SortID = sortID;
                                piEnv1.title = sort(sortID).title;
                                piEnv1.Debuffable = true;
                                piEnv1.visibleToPlayers = true;
                                piEnv1.playerRoxed.Add("null");     // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                                piEnv1.relanceInterval = sort(sortID).isbl[sortLvl - 1].relanceInterval;     // relanceInterval egale le bonus, pour que l'envoutement s'expire a la fin du bonus et non a la fin du vrais relanceInterval du sort vus que ce dernier est plus grand que le 1er
                                piEnv1.BuffState = Buff.State.Fin;
                                piEnv1.relanceParTour = 1;
                                piEnv1.systeme = false;
                                piEnv1.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv1.Bonus.power = sort(sortID).isbl[sortLvl - 1].piBonus.power;
                                piEnv1.Cd = cd;
                                piEnv1.player = player;
                                piGotPuissance.BuffsList.Add(piEnv1);
                            }
                        }
                        #endregion
                    }

                    Thread.Sleep(100);
                }
                #endregion
                // pc utilisés
                // on cherche si UsedPoint contiens la valeur PcUsed:PC

                Point playerOfSpellPoint = spellCaster.realPosition;
                Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                if (usedPointL.Exists(f => f[0] == "PcUsed"))
                {
                    string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                    new Thread(new ThreadStart(() =>
                    {
                        CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                    })).Start();
                }

                #region Mise en envoutement system NON VISIBLE
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd, "null");
                #endregion
                // envoutement visible a été fait avant puisqu'il ya une possibilité que plusieurs joueurs sois affecté par cet envoutement de zone

                CommonCode.ChatMsgBattleFormat("spell", player, rawData, TargetPoint, sortID);

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[1].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                // effacement des cadront des joueurs dans la timeline
                Battle.RefreshTimeLine();

                // libération et purgage de la liste des cmd
                CommonCode.blockNetFlow = false;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow16 = false");
                #endregion
            }
            else if (sortID == 10)
            {
                #region Etat Sennin
                // "typeRox:etat:sinnin|cd:" + cdAllowed + "|chakra:neutral|deadList:" + playerDead + "|" + DotonString;
                bool cd = Convert.ToBoolean(rawData.Split('|')[1].Split(':')[1]);

                /////////////////////////// traitement specifique au sort
                // affichage de l'animation de recovoir des dom, inutile dans ce sort puisqu'il sagit d'un sort de boost
                #region affichage des éffets du sort, image translucide des yeux en moide SENNIN
                Anim __SenninEye_to_Small = new Anim(800, 1);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 1F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.9F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.8F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.7F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.6F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.5F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.4F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.3F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.2F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0.1F, 50);
                __SenninEye_to_Small.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 0, 0, 0F, 50);
                Manager.manager.GfxObjList.Add(__SenninEye_to_Small);
                __SenninEye_to_Small.PointOfParent = true;
                __SenninEye_to_Small.Ini(Manager.TypeGfx.Obj, "__SenninEye_to_Small", true);
                __SenninEye_to_Small.img.point.X = ((ScreenManager.TilesWidth * 30) / 2) - (__SenninEye_to_Small.img.rectangle.Width / 2);
                __SenninEye_to_Small.img.point.Y = 200;
                __SenninEye_to_Small.AutoResetAnim = false;
                __SenninEye_to_Small.HideAtLastFrame = true;
                __SenninEye_to_Small.DestroyAfterLastFrame = true;
                __SenninEye_to_Small.Start();

                Anim __SenninEye_to_Big = new Anim(800, 1);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 0, 276, 200, 439, 112, 1F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 1, 300, 200, 395, 101, 0.9F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 2, 320, 200, 351, 90, 0.8F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 3, 342, 200, 307, 78, 0.7F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 4, 377, 200, 236, 67, 0.6F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 5, 386, 200, 219, 56, 0.5F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 6, 408, 200, 175, 45, 0.4F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 7, 430, 200, 131, 33, 0.3F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 8, 461, 200, 69, 18, 0.2F, 50);
                __SenninEye_to_Big.AddCell(@"gfx\general\obj\1\senninEye.dat", 9, 474, 200, 43, 11, 0.1F, 50);
                Manager.manager.GfxObjList.Add(__SenninEye_to_Big);
                __SenninEye_to_Big.Ini(Manager.TypeGfx.Obj, "__SenninEye_to_Big", true);
                __SenninEye_to_Big.img.point.X = ((ScreenManager.TilesWidth * 30) / 2) - (__SenninEye_to_Big.img.rectangle.Width / 2);
                __SenninEye_to_Big.img.point.Y = 200;
                __SenninEye_to_Big.AutoResetAnim = false;
                __SenninEye_to_Big.HideAtLastFrame = true;
                __SenninEye_to_Big.DestroyAfterLastFrame = true;
                __SenninEye_to_Big.Start();
                #endregion
                // activation des sorts dépandants
                // on passe sur tous les sorts pour les verifier
                if (spellCaster.pseudo == CommonCode.MyPlayerInfo.instance.pseudo)
                {
                    for (int cnt = 0; cnt < spellCaster.spells.Count; cnt++)
                    {
                        Actor.SpellsInformations piis = spellCaster.spells[cnt];
                        if (spells.sort_need_etat_sennin.Exists(f => f == piis.sortID))
                        {
                            // sort qui necessite le mode Sennin, on verifie si le joueur à l'nvoutement du mode sennin
                            if (!spellCaster.BuffsList.Exists(f => f.StateList.Exists(e => e == Buff.Name.Senin)))
                            {
                                // le client n'est pas en mode Sennin, on désactive tous les sorts dépandant
                                // check si le joueur à le sort futon rasen shuriken
                                Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == piis.sortID));
                                spellIcon.ChangeBmp(@"gfx\general\obj\1\spells.dat", SpriteSheet.GetSpriteSheet(piis.sortID + "_spell", 0));
                            }
                        }
                    }
                }
                System.Media.SoundPlayer __hit;
                // lancement de sort kagebunshin_no_jutsu.wav
                __hit = new System.Media.SoundPlayer(@"sfx\spell\heartbeat.dat");
                __hit.Play();

                #region dom et envoutement visibles
                for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                {
                    string[] data = rawData.Split('#');
                    string tmp = data[cnt].ToString();

                    string[] DomString = tmp.Split('|');
                    List<string> deadList = DomString[3].Split(':').ToList();
                    deadList.RemoveAt(0);                               // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                    if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                        deadList.Clear();

                    // check si un ou plusieurs joueurs sont mort
                    if (deadList.Count > 0)
                    {
                        for (int i = 0; i < deadList.Count; i++)
                        {
                            Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                            if (ibPlayer != null)
                                CommonCode.animDeadPlayer(ibPlayer);
                        }
                    }

                    // supprimer des joueurs si la liste deadList est plus grande que 0
                    if (deadList.Count > 0)
                    {
                        for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                        {
                            Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                            Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                            CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                        }

                        // on actualise la timeline
                        Battle.RefreshTimeLine();
                    }
                    //cmd•spellTileGranted•narutox•6•19•8•0•5•"typeRox:etat:sinnin|cd:" + cdAllowed + "|chakra:neutral|deadList:" + playerDead + "|" + DotonString;
                    // Etat Sennin
                    string SenninString = tmp.Split('|')[4];
                    if (SenninString != "null")
                    {
                        for (int cnt1 = 0; cnt1 < SenninString.Split('/').Count(); cnt1++)
                        {
                            string tmpSenninString = SenninString.Split('/')[cnt1];
                            Actor PlayerGotSennin = Battle.AllPlayersByOrder.Find(f => f.pseudo == tmpSenninString.Split(':')[0]);
                            int SenninAdded = Convert.ToInt32(tmpSenninString.Split(':')[1]);
                            int currentSennin = Convert.ToInt32(tmpSenninString.Split(':')[2]);

                            Point playerOfSpellPoint1 = PlayerGotSennin.realPosition;
                            Rectangle playerOfSpellRectangle1 = PlayerGotSennin.ibPlayer.rectangle;
                            int playerOfSpellZindex1 = PlayerGotSennin.ibPlayer.zindex;

                            // ajout de la puissance
                            PlayerGotSennin.doton += SenninAdded;
                            if (PlayerGotSennin.doton != currentSennin)
                                MessageBox.Show("Doton client different du Doton serveur,[client = " + PlayerGotSennin.doton + "][Server = " + currentSennin + "]");
                            new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(500); // pour que l'ecriture ne sois pas aligné horizontalement avec les pc utilisé, du coup on vois pas
                                CommonCode.showAnimUpgrade(SenninAdded, playerOfSpellPoint1, playerOfSpellRectangle1, playerOfSpellZindex1, Enums.Chakra.Element.doton.ToString());
                            })).Start();
                        }

                        #region envoutement visible non systeme pour ajout de puissance + mode sennin
                        for (int cnt2 = 0; cnt2 < SenninString.Split('/').Count(); cnt2++)
                        {
                            string gotSenninPseudo = SenninString.Split('/')[cnt2].Split(':')[0];
                            int bonnusSennin = Convert.ToInt32(SenninString.Split('/')[cnt2].Split(':')[1]);
                            int totalSennin = Convert.ToInt32(SenninString.Split('/')[cnt2].Split(':')[2]);
                            Actor piGotSennin = Battle.AllPlayersByOrder.Find(f => f.pseudo == gotSenninPseudo);

                            if (piGotSennin.BuffsList.Exists(f => f.SortID == sortID && !f.systeme))
                            {
                                Actor.Buff piEnv = piGotSennin.BuffsList.Find(f => f.SortID == sortID && !f.systeme);
                                piEnv.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv.playerRoxed.Add("null");      // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                            }
                            else
                            {
                                // ajout du sort dans les envoutements
                                Actor.Buff piEnv1 = new Actor.Buff();
                                piEnv1.SortID = sortID;
                                piEnv1.title = sort(sortID).title;
                                piEnv1.Debuffable = true;
                                piEnv1.visibleToPlayers = true;
                                piEnv1.playerRoxed.Add("null");     // peut importe le nom de l'adversaire roxé, se qui compte c'est le nombre de lancé
                                piEnv1.relanceInterval = sort(sortID).isbl[sortLvl - 1].relanceInterval;     // relanceInterval egale le bonus, pour que l'envoutement s'expire a la fin du bonus et non a la fin du vrais relanceInterval du sort vus que ce dernier est plus grand que le 1er
                                piEnv1.BuffState = Buff.State.Fin;
                                piEnv1.relanceParTour = 1;
                                piEnv1.systeme = false;
                                piEnv1.BonusRoundLeft = sort(sortID).isbl[sortLvl - 1].BonusRoundLeft;
                                piEnv1.Bonus.doton = sort(sortID).isbl[sortLvl - 1].piBonus.doton;
                                piEnv1.StateList.Add(Buff.Name.Senin);
                                piEnv1.Cd = cd;
                                piEnv1.player = player;
                                piGotSennin.BuffsList.Add(piEnv1);
                            }
                        }
                        #endregion
                    }
                    Thread.Sleep(100);
                }
                #endregion
                // pc utilisés
                // on cherche si UsedPoint contiens la valeur PcUsed:PC

                Point playerOfSpellPoint = spellCaster.realPosition;
                Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                if (usedPointL.Exists(f => f[0] == "PcUsed"))
                {
                    string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                    new Thread(new ThreadStart(() =>
                    {
                        CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                    })).Start();
                }

                #region Mise en envoutement system NON VISIBLE
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd, "null");
                #endregion
                // envoutement visible a été fait avant puisqu'il ya une possibilité que plusieurs joueurs sois affecté par cet envoutement de zone

                CommonCode.ChatMsgBattleFormat("spell", player, rawData, TargetPoint, sortID);

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[1].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                // effacement des cadront des joueurs dans la timeline
                Battle.RefreshTimeLine();

                // libération et purgage de la liste des cmd
                CommonCode.blockNetFlow = false;
                CommonCode.ChatMsgFormat("S", "null", "blockNetFlow17 = false");
                #endregion
            }
            else if (sortID == 11)
            {
                #region katas des crapauds
                #region determination de la direction du sort
                string directionOfSpell = redirectPlayer(spellCaster, TargetPoint, sortID);
                #endregion

                #region Mise en envoutement
                // check si c'est un cd ou pas, a assigner les envoutement systeme et non systeme comem bonnus / malus
                string data3 = rawData.Split('#')[0];
                string[] DomString3 = data3.Split('|');
                // CD se trouve a l'occurance DomString3[2] lorsqu'il s'agit de typeRox:rox, et quand c'est typeRox:Invoc ou Desinvoc Cd se trouve à DomString3[1]
                bool cd3 = Convert.ToBoolean(DomString3[2].Split(':')[1]);
                //////////////////////// mise en envoutement du sort
                string _roxed = rawData.Split('|')[6];
                SetEnvoutementSystem(player, spellCaster, sortID, sortLvl, cd3, _roxed);
                #endregion

                #region diminution des pv du joueur roxé
                downgradPlayerLife(player, rawData, TargetPoint, sortID);
                #endregion

                #region // affichage de l'image coup dangeureux au dessus de la tete du roxeur
                string data2 = rawData.Split('#')[0];
                string[] DomString2 = data2.Split('|');
                bool cd2 = Convert.ToBoolean(DomString2[2].Split(':')[1]);
                ShowCDabovePlayer(spellCaster, cd2);
                #endregion

                #region // thread affichage du sort
                new Thread((() =>
                {
                    Thread.CurrentThread.Name = "__redraw_Spell_Thread";
                    Anim __rasengan = new Anim(30, 1);
                    long startTime = Environment.TickCount; // contien le temps pour le timer créer avec une la boucle whiel
                    System.Media.SoundPlayer __hit;

                    while (!Manager.manager.mainForm.IsDisposed)
                    {
                        #region changement de l'image du joueur roxé pour faire semblement de recevoir des dom

                        // affichage des dom en haut de personnage
                        for (int cnt = 0; cnt < rawData.Split('#').Count(); cnt++)
                        {
                            string[] data = rawData.Split('#');
                            string tmp = data[cnt].ToString();

                            Anim __gamakichi = new Anim(15, 1);
                            int posX = (TargetPoint.X * 30) - 10;
                            int posY = (TargetPoint.Y * 30) - 235;
                            __gamakichi.AddCell(@"gfx\general\obj\3\gamakichi.dat", 0, posX, posY, 15);
                            for (int cnt2 = 0; cnt2 < 22; cnt2++)
                            {
                                posY += 10;
                                __gamakichi.AddCell(@"gfx\general\obj\3\gamakichi.dat", 0, posX, posY, 10);
                            }
                            posY += 10;
                            __gamakichi.AddCell(@"gfx\general\obj\3\gamakichi.dat", 0, posX, posY, 500);
                            __gamakichi.Ini(Manager.TypeGfx.Obj, "__gamakichi", true);
                            __gamakichi.img.zindex = CommonCode.VerticalSyncZindex(TargetPoint.Y, Manager.TypeGfx.Obj);
                            __gamakichi.AutoResetAnim = false;
                            __gamakichi.HideAtLastFrame = true;
                            __gamakichi.DestroyAfterLastFrame = true;
                            __gamakichi.Start();
                            Manager.manager.GfxObjList.Add(__gamakichi);

                            string roxed = tmp.Split('|')[6];
                            Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);

                            if (playerTargeted2 != null)
                            {
                                Point playerTargetedPoint = playerTargeted2.realPosition;
                                Rectangle playerTargetedRectangle = playerTargeted2.ibPlayer.rectangle;
                                int playerTargetedZindex = playerTargeted2.ibPlayer.zindex;

                                new Thread(new ThreadStart(() =>
                                {
                                    Thread.CurrentThread.Name = "__Display_Dom_Acros_Player_" + cnt + "_Thread";
                                    CommonCode.showAnimDamageAbove(playerTargetedPoint, playerTargetedRectangle, playerTargetedZindex, tmp);
                                })).Start();

                                string[] DomString = tmp.Split('|');
                                List<string> deadList = DomString[5].Split(':').ToList();
                                deadList.RemoveAt(0);           // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                                if (deadList.Count == 1 && deadList[0] == "")       // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                                    deadList.Clear();

                                // check si un ou plusieurs joueurs sont mort
                                if (deadList.Count > 0)
                                {
                                    for (int i = 0; i < deadList.Count; i++)
                                    {
                                        Bmp ibPlayer = CommonCode.AllActorsInMap.Find(f => (f.tag as Actor).pseudo == deadList[i]);
                                        if (ibPlayer != null)
                                            CommonCode.animDeadPlayer(ibPlayer);
                                    }
                                }

                                // supprimer notre joueur si la liste deadList est plus grande que 0
                                if (deadList.Count > 0)
                                {
                                    for (int cnt2 = 0; cnt2 < deadList.Count; cnt2++)
                                    {
                                        Battle.DeadPlayers.Add((Actor)(Battle.AllPlayersByOrder.Find(f => f.pseudo == deadList[cnt2])).Clone());
                                        Battle.SideA.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        Battle.SideB.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        Battle.AllPlayersByOrder.RemoveAll(f => f.pseudo == deadList[cnt2]);
                                        CommonCode.ChatMsgFormat("dead", deadList[cnt2], "");
                                    }

                                    // on actualise la timeline
                                    Battle.RefreshTimeLine();
                                }
                            }
                            Thread.Sleep(100);
                        }
                        CommonCode.blockNetFlow = false;
                        CommonCode.ChatMsgFormat("S", "null", "blockNetFlow18 = false");
                        // pc utilisés
                        // on cherche si UsedPoint contiens la valeur PcUsed:PC
                        Point playerOfSpellPoint = spellCaster.realPosition;
                        Rectangle playerOfSpellRectangle = spellCaster.ibPlayer.rectangle;
                        int playerOfSpellZindex = spellCaster.ibPlayer.zindex;

                        if (usedPointL.Exists(f => f[0] == "PcUsed"))
                        {
                            string[] usedPointT = usedPointL.Find(f => f[0] == "PcUsed");

                            new Thread(new ThreadStart(() =>
                            {
                                CommonCode.showAnimUsedPC(Convert.ToInt32(usedPointT[1]), playerOfSpellPoint, playerOfSpellRectangle, playerOfSpellZindex);
                            })).Start();
                        }

                        __hit = new System.Media.SoundPlayer(@"sfx\spell\hit1.dat");
                        __hit.Play();

                        break;
                        #endregion
                    }
                })).Start();
                #endregion

                replacePlayerInCorrecteCoordinate(spellCaster);

                #endregion
            }

            // mise a jours des states
            string[] dataUsedPoint = UsedPoint.Split('|');
            for (int cnt = 0; cnt < dataUsedPoint.Count(); cnt++)
            {
                string[] curDataUsedPoint = dataUsedPoint[cnt].Split(':');
                if (curDataUsedPoint[0] == "PcUsed")
                {
                    // réduction de pa
                    spellCaster.currentPc -= Convert.ToInt32(curDataUsedPoint[1]);
                }
            }

            // update des pc au cas ou on subit un bonus de pa ou malus vus que c'est 2 states sont affiché dans d'autre indicateurs qui ne relève pas leurs données des states du joueur, du coup il faut les mettre à jours manuellement
            HudHandle.UpdatePc();
            HudHandle.UpdatePm();

            if (player == CommonCode.MyPlayerInfo.instance.pseudo)
            {
                Actor piib = Battle.AllPlayersByOrder.Find(f => f.pseudo == player);

                // check contre MaxLanceParTour, si > 1 on dois griser le sort en question
                if (piib.BuffsList.Exists(f => f.relanceParTour == f.playerRoxed.Count && f.SortID == sortID && f.systeme))
                {
                    // envoutement non visible du sort lancé
                    Actor.Buff piEnv = spellCaster.BuffsList.Find(f => f.SortID == sortID && f.systeme);

                    // pointeur vers l'image du sort lancé sur le tableau des sort
                    Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == sortID));

                    Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;

                    // changement de l'image de sort en image grisé
                    spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(sortID + "_spell", 0));

                    // check contre le relanceInterval, compte à rebour
                    if (piib.BuffsList.Exists(f => f.relanceInterval > 0 && f.SortID == sortID && f.systeme))
                    {
                        Txt relanceInterval_sortID = new Txt(piEnv.relanceInterval.ToString(), Point.Empty, "relanceInterval_" + piEnv.SortID, Manager.TypeGfx.Top, true, new Font("Verdana", 9, FontStyle.Bold), Brushes.White);

                        // centrage
                        relanceInterval_sortID.point = new Point(spellIcon.point.X + (spellIcon.rectangle.Width / 2) - (TextRenderer.MeasureText(relanceInterval_sortID.Text, relanceInterval_sortID.font).Width / 2), spellIcon.point.Y + (spellIcon.rectangle.Height / 2) - (TextRenderer.MeasureText(relanceInterval_sortID.Text, relanceInterval_sortID.font).Height / 2));
                        HudHandle.all_sorts.Child.Add(relanceInterval_sortID);
                    }
                }

                for (int cnt = 0; cnt < (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).spells.Count; cnt++)
                {
                    List<Actor.SpellsInformations> piis = (CommonCode.MyPlayerInfo.instance.ibPlayer.tag as Actor).spells;

                    if (spellCaster.currentPc < spells.sort(piis[cnt].sortID).isbl[sortLvl - 1].pi.originalPc)
                    {
                        // il reste pas assez de pc pour lancer le sort
                        // pointeur vers l'image du sort lancé sur le tableau des sort
                        Bmp spellIcon = (Bmp)(HudHandle.all_sorts.Child.Find(f => f.GetType() == typeof(Bmp) && (f as Bmp).tag.GetType() == typeof(Actor.SpellsInformations) && ((f as Bmp).tag as Actor.SpellsInformations).sortID == piis[cnt].sortID));

                        Actor.SpellsInformations spellIS = spellIcon.tag as Actor.SpellsInformations;

                        // changement de l'image de sort en image grisé
                        spellIcon.ChangeBmp(@"gfx\general\obj\1\gray_spells.dat", SpriteSheet.GetSpriteSheet(spellIS.sortID + "_spell", 0));
                    }
                }
            }
            
        }
        public static string redirectPlayer(Actor playerOfSpell, Point TargetPoint, int sortID)
        {
            #region redirection du personnage
            string directionOfSpell;

            if (playerOfSpell.realPosition.X == TargetPoint.X && playerOfSpell.realPosition.Y > TargetPoint.Y)
            {
                // changement de la direction du joueur vers le haut
                playerOfSpell.directionLook = 3;
                playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.className + "_AttackSprite" + spells.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.className + "_attackSprite" + spells.sort(sortID).positionPlayer, playerOfSpell.directionLook));
                directionOfSpell = "up";
            }
            else if (playerOfSpell.realPosition.X == TargetPoint.X && playerOfSpell.realPosition.Y < TargetPoint.Y)
            {
                // changement de la direction du joueur vers le bas
                playerOfSpell.directionLook = 0;
                playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.className + "_AttackSprite" + spells.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.className + "_attackSprite" + spells.sort(sortID).positionPlayer, playerOfSpell.directionLook));
                directionOfSpell = "down";
            }
            else if (playerOfSpell.realPosition.X > TargetPoint.X && playerOfSpell.realPosition.Y == TargetPoint.Y)
            {
                // changement de la direction du joueur vers la gauche
                playerOfSpell.directionLook = 1;
                playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.className + "_AttackSprite" + spells.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.className + "_attackSprite" + spells.sort(sortID).positionPlayer, playerOfSpell.directionLook));
                directionOfSpell = "left";
                if (playerOfSpell.className == Enums.ActorClass.ClassName.naruto)
                    playerOfSpell.ibPlayer.point.X -= 8;
            }
            else
            {
                // changement de la direction du joueur vers la droite
                playerOfSpell.directionLook = 2;
                playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\attackSprite\" + playerOfSpell.className + "_AttackSprite" + spells.sort(sortID).positionPlayer + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.className + "_attackSprite" + spells.sort(sortID).positionPlayer, playerOfSpell.directionLook));
                directionOfSpell = "right";
            }
            return directionOfSpell;
            #endregion
        }
        public static void SetEnvoutementSystem(string player,Actor playerOfSpell,int sortID,int sortLvl,bool cd3, string _roxed)
        {
            if (playerOfSpell.BuffsList.Exists(f => f.SortID == sortID && f.systeme))
            {
                Actor.Buff piEnv = playerOfSpell.BuffsList.Find(f => f.SortID == sortID && f.systeme);
                piEnv.playerRoxed.Add(_roxed);
            }
            else
            {
                // ajout du sort dans les envoutements
                Actor.Buff piEnv1 = new Actor.Buff();
                piEnv1.SortID = sortID;
                piEnv1.title = spells.sort(sortID).title;
                piEnv1.Debuffable = false;
                piEnv1.visibleToPlayers = false;
                piEnv1.playerRoxed.Add(_roxed);
                piEnv1.relanceInterval = spells.sort(sortID).isbl[sortLvl - 1].relanceInterval;
                piEnv1.BuffState = Buff.State.Fin;
                piEnv1.relanceParTour = spells.sort(sortID).isbl[sortLvl - 1].relanceParTour;
                piEnv1.systeme = true;
                piEnv1.Cd = cd3;
                piEnv1.player = player;
                playerOfSpell.BuffsList.Add(piEnv1);
            }
        }
        public static void downgradPlayerLife(string player, string domString, Point TargetPoint, int sortID)
        {
            #region // diminution des pv du joueur roxé
            for (int cnt = 0; cnt < domString.Split('#').Count(); cnt++)
            {
                string data = domString.Split('#')[cnt];
                string[] DomString = data.Split('|');
                string typeRox = DomString[0].Split(':')[1];
                int jet = Convert.ToInt32(DomString[1].Split(':')[1]);
                bool cd = Convert.ToBoolean(DomString[2].Split(':')[1]);
                string chakra = DomString[3].Split(':')[1];
                int dom = Convert.ToInt32(DomString[4].Split(':')[1]);
                List<string> deadList = DomString[5].Split(':').ToList();
                deadList.RemoveAt(0); // on supprime la 1ere occuronce qui correpond au mot clef "deadlist", et le reste est le nom des adversaires mort a cause du sort séparé par :
                if (deadList.Count == 1 && deadList[0] == "") // il faut supprimer la 2eme partie vide du mnemonique "deadList:" si on le séppart par : puisqu'il va nous donner 2 partie, qui est déja supprimé et la 2éme partie a supprimer si'il ya eu aucun mort
                    deadList.Clear();
                string roxed = DomString[6];
                Actor playerTargeted2 = Battle.AllPlayersByOrder.Find(f => f.pseudo == roxed);

                // affichage des données du damages sur la zone chat
                Manager.manager.mainForm.BeginInvoke((Action)(() =>
                {
                    CommonCode.ChatMsgBattleFormat("spell", player, data, TargetPoint, sortID);
                }));

                if (playerTargeted2 != null)
                {
                    if (typeRox == "rox")
                    {
                        playerTargeted2.currentHealth -= dom;
                        // retrait de 5% des dom au total pdv du joueur roxé comme érosion
                        int ero = (dom * 5) / 100;
                        playerTargeted2.maxHealth -= ero;
                        if (playerTargeted2.currentHealth <= 0)
                            playerTargeted2.currentHealth = 0;

                        // s'il sagit de notre joueur, actualiser la barre de ses points
                        HudHandle.UpdateHealth();
                    }
                }
            }
            #endregion
        }
        public static void ShowCDabovePlayer(Actor playerOfSpell, bool cd)
        {
            if (cd)
            {
                new Thread((() =>
                {
                    Bmp cd_across_player = new Bmp(@"gfx\general\obj\1\cd_across_player.dat", Point.Empty, new Size(10, 10), "cd_across_player", Manager.TypeGfx.Obj, true, 1);
                    cd_across_player.point = new Point(playerOfSpell.ibPlayer.point.X + (playerOfSpell.ibPlayer.rectangle.Width / 2) - (cd_across_player.rectangle.Width / 2), playerOfSpell.ibPlayer.point.Y - 15 - cd_across_player.rectangle.Height);
                    cd_across_player.zindex = playerOfSpell.ibPlayer.zindex;
                    Manager.manager.GfxObjList.Add(cd_across_player);

                    // agrandissement
                    for (int cnt1 = 10; cnt1 < 60; cnt1 += 5)
                    {
                        cd_across_player.ChangeBmp(@"gfx\general\obj\1\cd_across_player.dat", new Size(cnt1, cnt1));
                        cd_across_player.point = new Point(playerOfSpell.ibPlayer.point.X + (playerOfSpell.ibPlayer.rectangle.Width / 2) - (cd_across_player.rectangle.Width / 2), playerOfSpell.ibPlayer.point.Y - 15 - cd_across_player.rectangle.Height);
                        Thread.Sleep(5);
                    }

                    Thread.Sleep(100);

                    // retrecissement
                    for (int cnt1 = 60; cnt1 > 10; cnt1 -= 5)
                    {
                        cd_across_player.ChangeBmp(@"gfx\general\obj\1\cd_across_player.dat", new Size(cnt1, cnt1));
                        cd_across_player.point = new Point(playerOfSpell.ibPlayer.point.X + (playerOfSpell.ibPlayer.rectangle.Width / 2) - (cd_across_player.rectangle.Width / 2), playerOfSpell.ibPlayer.point.Y - 15 - cd_across_player.rectangle.Height);
                        Thread.Sleep(5);
                    }

                    // agrandissement
                    for (int cnt1 = 10; cnt1 < 60; cnt1 += 5)
                    {
                        cd_across_player.ChangeBmp(@"gfx\general\obj\1\cd_across_player.dat", new Size(cnt1, cnt1));
                        cd_across_player.point = new Point(playerOfSpell.ibPlayer.point.X + (playerOfSpell.ibPlayer.rectangle.Width / 2) - (cd_across_player.rectangle.Width / 2), playerOfSpell.ibPlayer.point.Y - 15 - cd_across_player.rectangle.Height);
                        Thread.Sleep(5);
                    }

                    Thread.Sleep(500);
                    Manager.manager.GfxObjList.Remove(cd_across_player);
                }
                    )).Start();
            }
        }
        public static void replacePlayerInCorrecteCoordinate(Actor playerOfSpell)
        {
            #region // thread compteur pour remetre le joueur sur sa sprite d'origine apres le lancement du sort
            new Thread((() =>
            {
                Thread.CurrentThread.Name = "__redraw_Player_After_Spell_Thread";
                long startTime = Environment.TickCount;
                while (!Manager.manager.mainForm.IsDisposed)
                {

                    if (Environment.TickCount >= startTime + 2000)
                    {
                        // invocation du thread principale pour modifier l'image du joueur
                        Manager.manager.mainForm.BeginInvoke((Action)(() =>
                        {
                            Point pp = new Point(playerOfSpell.realPosition.X, playerOfSpell.realPosition.Y);
                            playerOfSpell.ibPlayer.point.X = (pp.X * 30) + 15 - (playerOfSpell.ibPlayer.rectangle.Width / 2);
                            playerOfSpell.ibPlayer.ChangeBmp(@"gfx\general\classes\" + playerOfSpell.className + ".dat", SpriteSheet.GetSpriteSheet(playerOfSpell.className.ToString(), playerOfSpell.directionLook * 4));
                        }));
                        break;
                    }

                    Thread.Sleep(1000);
                }
            })).Start();
            #endregion
        }
        public static List<sort_tuile_info> isAllowedSpellInSquareArea(int pe, int cacDistance, bool blockView)
        {
            #region champ d'un sort avec une distance au dela du cac et avec pris en charge de ligne de vus
            Point playerPosition = Battle.AllPlayersByOrder.Find(f => f.pseudo ==  CommonCode.MyPlayerInfo.instance.pseudo).realPosition;
            List<Point> allTuiles = new List<Point>();      // liste qui contiens tous les tuiles affecté par le sort y compris un obstacle ou pas
            List<sort_tuile_info> allTuilesInfo = new List<sort_tuile_info>();
            // allimentation de la liste allTuiles avec tous les cases atteignable par le sort

            // 1ere partie consiste a afficher toute la grille de tuiles
            for (int line = 0; line <= pe; line++)
            {
                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le bas
                if (line != 0 && playerPosition.Y + line < ScreenManager.TilesHeight)
                {
                    Point p = new Point(playerPosition.X, playerPosition.Y + line);
                    allTuiles.Add(p);

                    if (!CommonCode.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        if (Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == p.X && f.realPosition.Y == p.Y).Count > 0)
                        {
                            sti.isWalkable = true;
                            sti.isBlockingView = true;
                        }
                        else
                        {
                            sti.isWalkable = false;
                            sti.isBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        sti.isWalkable = true;
                        sti.isBlockingView = false;
                        allTuilesInfo.Add(sti);
                    }
                }

                // insersion d'une case/tuile en commencant par le centre si cnt = 0 vers le haut
                if (line != 0 && playerPosition.Y - line >= 0)
                {
                    Point p = new Point(playerPosition.X, playerPosition.Y - line);
                    allTuiles.Add(p);

                    if (!CommonCode.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        if (Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == p.X && f.realPosition.Y == p.Y).Count > 0)
                        {
                            sti.isWalkable = true;
                            sti.isBlockingView = true;
                        }
                        else
                        {
                            sti.isWalkable = false;
                            sti.isBlockingView = true;
                        }
                        allTuilesInfo.Add(sti);
                    }
                    else
                    {
                        sort_tuile_info sti = new sort_tuile_info();
                        sti.TuilePoint = p;
                        sti.isWalkable = true;
                        sti.isBlockingView = false;
                        allTuilesInfo.Add(sti);
                    }
                }

                if (pe == line)
                    break;

                for (int side = 1; side <= pe; side++)
                {
                    // ajouter des tuiles coté en bas a droite
                    if (playerPosition.X + side < ScreenManager.TilesWidth && playerPosition.Y + line < ScreenManager.TilesHeight)
                    {
                        Point p = new Point(playerPosition.X + side, playerPosition.Y + line);
                        allTuiles.Add(p);

                        if (!CommonCode.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == p.X && f.realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en bas a gauche
                    if (playerPosition.X - side < ScreenManager.TilesWidth && playerPosition.Y + line < ScreenManager.TilesHeight)
                    {
                        Point p = new Point(playerPosition.X - side, playerPosition.Y + line);
                        allTuiles.Add(p);

                        if (!CommonCode.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == p.X && f.realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a droite
                    if (playerPosition.X + side < ScreenManager.TilesWidth && playerPosition.Y - line < ScreenManager.TilesHeight && line > 0)
                    {
                        Point p = new Point(playerPosition.X + side, playerPosition.Y - line);
                        allTuiles.Add(p);

                        if (!CommonCode.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == p.X && f.realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }

                    // ajouter des tuiles coté en haut a gauche
                    if (playerPosition.X - side < ScreenManager.TilesWidth && playerPosition.Y - line < ScreenManager.TilesHeight && line > 0)
                    {
                        Point p = new Point(playerPosition.X - side, playerPosition.Y - line);
                        allTuiles.Add(p);

                        if (!CommonCode.CurMapFreeCellToSpell(new Point(p.X * 30, p.Y * 30)))
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            if (Battle.AllPlayersByOrder.FindAll(f => f.realPosition.X == p.X && f.realPosition.Y == p.Y).Count > 0)
                            {
                                sti.isWalkable = true;
                                sti.isBlockingView = true;
                            }
                            else
                            {
                                sti.isWalkable = false;
                                sti.isBlockingView = true;
                            }
                            allTuilesInfo.Add(sti);
                        }
                        else
                        {
                            sort_tuile_info sti = new sort_tuile_info();
                            sti.TuilePoint = p;
                            sti.isWalkable = true;
                            sti.isBlockingView = false;
                            allTuilesInfo.Add(sti);
                        }
                    }
                    // determiner si le nombre de tuiles attein à cause de la notion qui vaux 1 tuile diagonale vaux 2 tuile, donc on déduit 1pe de chaque ligne
                    if (side + line == pe)
                        break;
                }
            }

            // mise en mode isWalkable = false des tuiles obstacle mais qui laisse la ligne de vue comme meme comme de l'eau ...
            List<sort_tuile_info> lsti = allTuilesInfo.FindAll(f => f.isWalkable == true && f.isBlockingView == false);
            for (int i = 0; i < lsti.Count; i++)
                if (!CommonCode.CurMapFreeCellToWalk(new Point(lsti[i].TuilePoint.X * 30, lsti[i].TuilePoint.Y * 30)) && lsti[i].isWalkable == true && lsti[i].isBlockingView == false)
                    allTuilesInfo.Find(f => f.TuilePoint == lsti[i].TuilePoint && f.isWalkable == true && lsti[i].isBlockingView == false).isWalkable = false;

            if (blockView)
            {
                //////////////////// algo pour ligne de vue pour les obstacles
                // determination de la liste de tous les obstacles
                List<sort_tuile_info> block_view_tile = allTuilesInfo.FindAll(f => f.isBlockingView == true);

                // calcules préliminaires
                for (int i = 0; i < block_view_tile.Count; i++)
                {
                    // determination de la distance entre la position du joueur et l'obstacle
                    int xDistance = Math.Abs(playerPosition.X - block_view_tile[i].TuilePoint.X);
                    int yDistance = Math.Abs(playerPosition.Y - block_view_tile[i].TuilePoint.Y);

                    // determiner le niveau d'envergure de l'angle par tuile
                    float AngleA = 0;
                    float AngleB = 0;

                    // determination de ladirection de l'angle
                    bool rightDirection = false;
                    bool leftDirection = false;
                    bool downDirection = false;
                    bool upDirection = false;

                    if (playerPosition.X > block_view_tile[i].TuilePoint.X)
                        leftDirection = true;
                    else if (playerPosition.X < block_view_tile[i].TuilePoint.X)
                        rightDirection = true;

                    if (playerPosition.Y > block_view_tile[i].TuilePoint.Y)
                        upDirection = true;
                    else if (playerPosition.Y < block_view_tile[i].TuilePoint.Y)
                        downDirection = true;

                    // point de départ du polygone
                    Point pointAOfObstacle = new Point();
                    Point pointBOfObstacle = new Point();

                    // coordonnées de l'obstacle en cours de vérifications selons la position du joueur
                    if (upDirection && playerPosition.X != block_view_tile[i].TuilePoint.X && playerPosition.Y != block_view_tile[i].TuilePoint.Y)
                    {
                        // calcule des point d'intersection avec l'obstacle pour tracer la ligne
                        pointAOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointAOfObstacle.Y = block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X + 1 : block_view_tile[i].TuilePoint.X;
                        pointBOfObstacle.Y = block_view_tile[i].TuilePoint.Y + 1;
                    }
                    else if (downDirection && playerPosition.X != block_view_tile[i].TuilePoint.X && playerPosition.Y != block_view_tile[i].TuilePoint.Y)
                    {
                        // calcule des point d'intersection avec l'obstacle pour tracer la ligne
                        pointAOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X + 1 : block_view_tile[i].TuilePoint.X;
                        pointAOfObstacle.Y = block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointBOfObstacle.Y = block_view_tile[i].TuilePoint.Y + 1;
                    }
                    else if ((upDirection || downDirection) && !rightDirection && !leftDirection)
                    {
                        // le joueur est aligné horizontalement
                        pointAOfObstacle.X = block_view_tile[i].TuilePoint.X;
                        pointAOfObstacle.Y = (upDirection) ? block_view_tile[i].TuilePoint.Y + 1 : block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = block_view_tile[i].TuilePoint.X + 1;
                        pointBOfObstacle.Y = (upDirection) ? block_view_tile[i].TuilePoint.Y + 1 : block_view_tile[i].TuilePoint.Y;
                    }
                    else if ((rightDirection || leftDirection) && !upDirection && !downDirection)
                    {
                        // le joueur est aligné horizontalement
                        pointAOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointAOfObstacle.Y = block_view_tile[i].TuilePoint.Y;

                        pointBOfObstacle.X = (rightDirection) ? block_view_tile[i].TuilePoint.X : block_view_tile[i].TuilePoint.X + 1;
                        pointBOfObstacle.Y = block_view_tile[i].TuilePoint.Y + 1;
                    }

                    // calcule de l'envergure de l'angle par tuile passé
                    if (upDirection && (rightDirection || leftDirection))
                    {
                        AngleA = ((yDistance * 30) + 15) / (xDistance - 0.5F);
                        AngleB = (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F);
                    }
                    else if (downDirection && (rightDirection || leftDirection))
                    {
                        AngleA = (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F);
                        AngleB = ((yDistance * 30) + 15) / (xDistance - 0.5F);
                    }
                    else if (!downDirection && !upDirection && (rightDirection || leftDirection))
                    {
                        AngleA = ((yDistance * 30) + 15) / ((xDistance + 1) - 0.5F);   // direction vers la gauche
                        AngleB = ((yDistance * 30) + 15) / ((xDistance + 1) - 0.5F);   // direction vers la droite
                    }
                    else if (!rightDirection && !leftDirection && (upDirection || downDirection))
                    {
                        AngleA = ((yDistance * 30) + 15) / (yDistance - 0.5F);   // direction vers la gauche
                        AngleB = ((yDistance * 30) + 15) / (yDistance - 0.5F);   // direction vers la droite
                    }

                    AngleA = Math.Abs((upDirection) ? ((yDistance * 30) + 15) / (xDistance - 0.5F) : (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F));
                    AngleB = Math.Abs((upDirection) ? (((yDistance - 1) * 30) + 15) / ((xDistance + 1) - 0.5F) : ((yDistance * 30) + 15) / (xDistance - 0.5F));

                    List<Point> nextPointAL = new List<Point>();
                    List<Point> nextPointBL = new List<Point>();

                    for (int j = 1; j <= pe; j++)
                    {
                        if (upDirection && rightDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (upDirection && leftDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (downDirection && rightDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (downDirection && leftDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (upDirection && !rightDirection && !leftDirection && !downDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (downDirection && !rightDirection && !leftDirection && !upDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) - (j * 30), (pointAOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (rightDirection && !downDirection && !upDirection && !leftDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (leftDirection && !downDirection && !upDirection && !rightDirection)
                            nextPointAL.Add(new Point((pointAOfObstacle.X * 30) + (j * 30), (pointAOfObstacle.Y * 30) - (int)(AngleA * j)));
                    }

                    for (int j = 1; j <= pe; j++)
                    {
                        if (upDirection && rightDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30) + 30, (pointBOfObstacle.Y * 30) - (int)(AngleB * j)));
                        else if (upDirection && leftDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) - (j * 30) - 30, (pointBOfObstacle.Y * 30) - (int)(AngleB * j)));
                        else if (downDirection && rightDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleB * j)));
                        else if (downDirection && leftDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) - (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleB * j)));
                        else if (upDirection && !rightDirection && !leftDirection && !downDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) - (int)(AngleA * j)));
                        else if (downDirection && !rightDirection && !leftDirection && !upDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (rightDirection && !upDirection && !downDirection && !leftDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
                        else if (leftDirection && !upDirection && !downDirection && !rightDirection)
                            nextPointBL.Add(new Point((pointBOfObstacle.X * 30) + (j * 30), (pointBOfObstacle.Y * 30) + (int)(AngleA * j)));
                    }

                    // tracage du cadre qui délimite les tuiles entre le joueur et le champ de vision
                    for (int a = 0; a < allTuilesInfo.Count; a++)
                    {
                        if (allTuilesInfo[a].TuilePoint == block_view_tile[i].TuilePoint)
                            continue;
                        // check si la tuile en cours se trouve entre les angles A et B
                        if (upDirection && rightDirection && !leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X >= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y <= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle superieur à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleA * (x + 1)) / 30))
                                    if ((y + 1) > Math.Floor(((AngleB * x) / 30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (upDirection && !rightDirection && leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X <= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y <= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle superieur à gauche du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en haut de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleA * (x + 1)) / 30))
                                    if ((y + 1) > Math.Floor(((AngleB * x) / 30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (downDirection && !rightDirection && leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X <= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y >= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle inférieure à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)(y) < ((AngleB * (x + 1)) / 30))
                                    if ((float)(y + 1) > Math.Floor(((AngleA * (x)) / (float)30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (downDirection && rightDirection && !leftDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X >= block_view_tile[i].TuilePoint.X && allTuilesInfo[a].TuilePoint.Y >= block_view_tile[i].TuilePoint.Y)
                            {
                                // la tuile est dans le rectangle inférieure à droite du joueur
                                // determination du distance entre l'obstacle et la position du tuile en cours
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // check si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)(y) < ((AngleB * (x + 1)) / 30))
                                    if ((float)(y + 1) > Math.Floor(((AngleA * (x)) / (float)30)))
                                    {
                                        allTuilesInfo[a].isBlockingView = true;
                                        allTuilesInfo[a].isWalkable = false;
                                    }
                            }
                        }
                        else if (rightDirection && !leftDirection && !upDirection && !downDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X >= block_view_tile[i].TuilePoint.X)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // check si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleB * (x + 1)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                        else if (leftDirection && !rightDirection && !upDirection && !downDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.X <= block_view_tile[i].TuilePoint.X)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y < ((AngleB * (x + 1)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                        else if (upDirection && !downDirection && !leftDirection && !rightDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.Y <= block_view_tile[i].TuilePoint.Y)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y >= ((AngleB * (x)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                        else if (downDirection && !upDirection && !leftDirection && !rightDirection)
                        {
                            if (allTuilesInfo[a].TuilePoint.Y >= block_view_tile[i].TuilePoint.Y)
                            {
                                int x = Math.Abs(allTuilesInfo[a].TuilePoint.X - block_view_tile[i].TuilePoint.X);
                                int y = Math.Abs(block_view_tile[i].TuilePoint.Y - allTuilesInfo[a].TuilePoint.Y);
                                // cheque si la tuile en cours est en bas de l'angle B, puis 2éme check pour l'angle A
                                if ((float)y >= ((AngleA * (x)) / 30))
                                {
                                    allTuilesInfo[a].isBlockingView = true;
                                    allTuilesInfo[a].isWalkable = false;
                                }
                            }
                        }
                    }
                }
            }
            // sustraction des cases qui ne sont pas accessible et qui sont a coté du joueur
            if (cacDistance == 2)
                allTuilesInfo.RemoveAll(f => (f.TuilePoint.X == playerPosition.X + 1 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X - 1 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X + 2 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X - 2 && f.TuilePoint.Y == playerPosition.Y) || (f.TuilePoint.X == playerPosition.X + 1 && f.TuilePoint.Y == playerPosition.Y + 1) || (f.TuilePoint.X == playerPosition.X - 1 && f.TuilePoint.Y == playerPosition.Y + 1) || (f.TuilePoint.X == playerPosition.X + 1 && f.TuilePoint.Y == playerPosition.Y - 1) || (f.TuilePoint.X == playerPosition.X - 1 && f.TuilePoint.Y == playerPosition.Y - 1) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y + 1) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y - 1) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y + 2) || (f.TuilePoint.X == playerPosition.X && f.TuilePoint.Y == playerPosition.Y - 2));

            return allTuilesInfo;
            #endregion
        }
        public static List<int> returnClassSpells(Enums.ActorClass.ClassName className)
        {
            // retourne les sorts d'une classe
            // il faut retourner ces infos depuis la bdd depuis une table qui contiens les sorts des classes
            if (className == Enums.ActorClass.ClassName.naruto)
                return new List<int>() { 0, 2, 3, 5, 6, 7, 8, 9, 10, 11 };
            else
                return new List<int>();
        }
    }
    public class sort_Stats
    {
        public string title;
        public int spellID;
        public string technique;
        public string rang;
        public Enums.Chakra.Element element;
        public int positionPlayer;          // id d'un type de position lorsqu'un joueur lance un sort pour faire samblant que le joueur a fait un truc, du genre tandre sa main ...
        public List<info_sort_by_level> isbl = new List<info_sort_by_level>();
        
        public class info_sort_by_level
        {
            // contiens les infos des sorts selons leurs niveau qui sont ensuite stoqué dans la liste isbl, donc sort a la position 0 = lvl1 ...
            public bool etenduModifiable;
            public int domMin, domMax, cd, etendu;
            public int relanceParTour;                        // combien de fois le sort peux être lancé en général par tour
            public int relanceParJoueur;                    // combien de fois le sort peux etre lancé sur le meme joueur
            public int relanceInterval;                     // combient de tours pour que le sort sois accessible
            public bool ligneDeVue;
            public Actor pi;                           // contien des infos des invocations par ex
            public int BonusRoundLeft;                      // s'il s'agit d'un bonus ou malus, ce variable compte combien de tour ceci est appliqué
            public Actor piBonus = new Actor();        // quand un bonus s'ajout au stats de joueur quand c'est positif ou se retire quand c negatif
        }
    }
    public class sort_tuile_info
    {
        // classe utilisé lors de l'affichage de la porté d'un sort, pour vérifier si une tuile est un obstacle ou non et si oui si sa dois bloquer la ligne de vus pour faciliter le controle
        public Point TuilePoint;
        public bool isWalkable;
        public bool isBlockingView;
        public string data;                 // pour passer des information aux tuiles affichés
    }
}