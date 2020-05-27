using System;
using System.Collections.Generic;

namespace SERVER
{
    public static class sorts
	{
        public static List<int> sort_d_invocation = new List<int>() { 3 };       // lister tous les sorts d'invocation
        public static List<int> sort_de_bonnus = new List<int>() { 7, 8, 9, 10 };       // lister tous les sorts qui ajoutes un bonnus ou malus ou autre ET qui utilises BonusRoundLeft pour decrementer
        /*public static sort_Stats sort(int _sortID)
		{
			// sorts de class naruto
			if (_sortID == 0)
			{
                #region rasengan
                sort_Stats ss = new sort_Stats();
				ss.title = "rasengan";
				ss.spellID = 0;
				ss.technique = "ninjutsu";
				ss.rang = "a";
				ss.element = "futon";

				sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
				isbllvl1.etendu = 2;
				isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = true;
                isbllvl1.min = 5;
				isbllvl1.max = 8;
				isbllvl1.cd = 40;
				isbllvl1.relanceInterval = 1;					// combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
				isbllvl1.relanceParJoueur = 1;					// combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
				isbllvl1.relanceParTour = 1;					// combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl1);

				sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level ();
				isbllvl2.etendu = 3;
				isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = true;
                isbllvl2.min = 7;
				isbllvl2.max = 10;
				isbllvl2.cd = 35;
				isbllvl2.relanceInterval = 1;
				isbllvl2.relanceParJoueur = 1;
				isbllvl2.relanceParTour = 2;
				isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl2);

				sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level ();
				isbllvl3.etendu = 4;
				isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = true;
                isbllvl3.min = 9;
				isbllvl3.max = 12;
				isbllvl3.cd = 30;
				isbllvl3.relanceInterval = 1;
				isbllvl3.relanceParJoueur = 2;
				isbllvl3.relanceParTour = 2;
				isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl3);

				sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level ();
				isbllvl4.etendu = 5;
				isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = true;
                isbllvl4.min = 11;
				isbllvl4.max = 14;
				isbllvl4.cd = 25;
				isbllvl4.relanceInterval = 1;
				isbllvl4.relanceParJoueur = 2;
				isbllvl4.relanceParTour = 3;
				isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl4);

				sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level ();
				isbllvl5.etendu = 6;
				isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = true;
                isbllvl5.min = 13;
				isbllvl5.max = 16;
				isbllvl5.cd = 20;
				isbllvl5.relanceInterval = 1;
				isbllvl5.relanceParJoueur = 2;
				isbllvl5.relanceParTour = 3;
				isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl5);

				ss.cdDomBonnus = 10;

				return ss;
                #endregion
            }
			else if (_sortID == 1)		// sort shuriken
			{
                #region Shuriken
                sort_Stats ss = new sort_Stats();
				ss.title = "shuriken";
				ss.spellID = 1;
				ss.technique = "ninjutsu";
				ss.rang = "a";
				ss.element = "doton";
				ss.cdDomBonnus = 10;

				sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
				isbllvl1.etendu = 2;
				isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = true;
                isbllvl1.min = 5;
				isbllvl1.max = 8;
				isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl1.pi = new PlayerInfo() { original_Pc = 3 };
				ss.isbl.Add (isbllvl1);

				sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level ();
				isbllvl2.etendu = 3;
				isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = true;
                isbllvl2.min = 7;
				isbllvl2.max = 10;
				isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl2.pi = new PlayerInfo() { original_Pc = 3 };
				ss.isbl.Add (isbllvl2);

				sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level ();
				isbllvl3.etendu = 4;
				isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = true;
                isbllvl3.min = 9;
				isbllvl3.max = 12;
				isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl3.pi = new PlayerInfo() { original_Pc = 3 };
				ss.isbl.Add (isbllvl3);

				sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level ();
				isbllvl4.etendu = 5;
				isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = true;
                isbllvl4.min = 11;
				isbllvl4.max = 14;
				isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl4.pi = new PlayerInfo() { original_Pc = 3 };
				ss.isbl.Add (isbllvl4);

				sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level ();
				isbllvl5.etendu = 6;
				isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = true;
                isbllvl5.min = 13;
				isbllvl5.max = 16;
				isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 3;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl5.pi = new PlayerInfo() { original_Pc = 3 };
				ss.isbl.Add (isbllvl5);

				ss.cdDomBonnus = 10;

				return ss;
                #endregion
            }
			else if (_sortID == 2)
			{
                #region rasen shuriken
                sort_Stats ss = new sort_Stats();
				ss.title = "rasen shuriken";
				ss.spellID = 2;
				ss.technique = "ninjutsu";
				ss.rang = "a";
				ss.element = "futon";

				sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
				isbllvl1.etendu = 2;
				isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = true;
                isbllvl1.min = 5;
				isbllvl1.max = 8;
				isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl1);

				sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level ();
				isbllvl2.etendu = 3;
				isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = true;
                isbllvl2.min = 7;
				isbllvl2.max = 10;
				isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl2);

				sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level ();
				isbllvl3.etendu = 4;
				isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = true;
                isbllvl3.min = 9;
				isbllvl3.max = 12;
				isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl3);

				sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level ();
				isbllvl4.etendu = 5;
				isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = true;
                isbllvl4.min = 11;
				isbllvl4.max = 14;
				isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl4);

				sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level ();
				isbllvl5.etendu = 6;
				isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = true;
                isbllvl5.min = 13;
				isbllvl5.max = 16;
				isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl5);
				
                ss.cdDomBonnus = 10;

				return ss;
                #endregion
            }
			else if (_sortID == 3)
			{
                #region Kage bunshin no jutsu
                sort_Stats ss = new sort_Stats();
				ss.title = "kage bunshin";
				ss.spellID = 3;
				ss.technique = "ninjutsu";
				ss.rang = "b";
				ss.element = "neutral";

				sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
				isbllvl1.etendu = 1;
				isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = false;
                isbllvl1.min = 0;
				isbllvl1.max = 0;
				isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl1);

				sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level ();
				isbllvl2.etendu = 2;
				isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = false;
                isbllvl2.min = 0;
				isbllvl2.max = 0;
				isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl2);

				sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level ();
				isbllvl3.etendu = 3;
				isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = false;
                isbllvl3.min = 0;
				isbllvl3.max = 0;
				isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl3);

				sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level ();
				isbllvl4.etendu = 4;
				isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = false;
                isbllvl4.min = 0;
				isbllvl4.max = 0;
				isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl4);

				sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level ();
				isbllvl5.etendu = 5;
				isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = false;
                isbllvl5.min = 0;
				isbllvl5.max = 0;
				isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 0;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
				isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl5);

				ss.cdDomBonnus = 10;

				return ss;
                #endregion
            }
			else if (_sortID == 4)
			{
                #region // sort dom terre de l'invocation clone naruto
                sort_Stats ss = new sort_Stats();
				ss.title = "pounch";
				ss.spellID = 4;
				ss.technique = "taijutsu";
				ss.rang = "d";
				ss.element = "doton";

				// l'invoc ne peux utiliser que le lvl 1 de ses sorts, pour le moment
				sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
				isbllvl1.etendu = 1;
				isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = true;
                isbllvl1.min = 4;
				isbllvl1.max = 6;
				isbllvl1.cd = 40;
				isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl1);

				sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level ();
				isbllvl2.etendu = 1;
				isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = true;
                isbllvl2.min = 7;
				isbllvl2.max = 9;
				isbllvl2.cd = 35;
				isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl2);

				sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level ();
				isbllvl3.etendu = 1;
				isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = true;
                isbllvl3.min = 10;
				isbllvl3.max = 12;
				isbllvl3.cd = 30;
				isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl3);

				sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level ();
				isbllvl4.etendu = 1;
				isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = true;
                isbllvl4.min = 13;
				isbllvl4.max = 15;
				isbllvl4.cd = 25;
				isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl4);

				sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level ();
				isbllvl5.etendu = 1;
				isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = true;
                isbllvl5.min = 16;
				isbllvl5.max = 18;
				isbllvl5.cd = 20;
				isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
				ss.isbl.Add (isbllvl5);
				ss.cdDomBonnus = 10;
				return ss;
                #endregion
            }
            else if (_sortID == 5)
            {
                #region // sort dom feu qui invoc le crappeau Gamabunta qui tap en zone de 3 cases
                sort_Stats ss = new sort_Stats();
                ss.title = "gamabunta";
                ss.spellID = 5;
                ss.technique = "invocation";
                ss.rang = "d";
                ss.element = "katon";

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level ();
                isbllvl1.etendu = 1;
                isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = true;
                isbllvl1.min = 4;
                isbllvl1.max = 6;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add (isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level ();
                isbllvl2.etendu = 1;
                isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = true;
                isbllvl2.min = 7;
                isbllvl2.max = 9;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 2;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add (isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level ();
                isbllvl3.etendu = 1;
                isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = true;
                isbllvl3.min = 10;
                isbllvl3.max = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add (isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level ();
                isbllvl4.etendu = 1;
                isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = true;
                isbllvl4.min = 13;
                isbllvl4.max = 15;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 2;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add (isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level ();
                isbllvl5.etendu = 1;
                isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = true;
                isbllvl5.min = 16;
                isbllvl5.max = 18;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 3;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 3;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add (isbllvl5);
                ss.cdDomBonnus = 10;
                return ss;
                #endregion
            }
            else if (_sortID == 6)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "transfert de vie";
                ss.spellID = 6;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "neutral";
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = false;
                isbllvl1.min = 0;
                isbllvl1.max = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = false;
                isbllvl2.min = 0;
                isbllvl2.max = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = false;
                isbllvl3.min = 0;
                isbllvl3.max = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = false;
                isbllvl4.min = 0;
                isbllvl4.max = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 3;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = false;
                isbllvl5.min = 0;
                isbllvl5.max = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 2;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 7)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "transfert de pc";
                ss.spellID = 7;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "neutral";
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.min = 0;
                isbllvl1.max = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.original_Pc = 2;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.min = 0;
                isbllvl2.max = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.original_Pc = 2;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.min = 0;
                isbllvl3.max = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 4;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.original_Pc = 2;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.min = 0;
                isbllvl4.max = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 3;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.original_Pc = 2;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.min = 0;
                isbllvl5.max = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 2;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.original_Pc = 2;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 8)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "transfert de pm";
                ss.spellID = 8;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "neutral";
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = false;
                isbllvl1.min = 0;
                isbllvl1.max = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.original_Pm = 2;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = false;
                isbllvl2.min = 0;
                isbllvl2.max = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.original_Pm = 2;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = false;
                isbllvl3.min = 0;
                isbllvl3.max = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.original_Pm = 2;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = false;
                isbllvl4.min = 0;
                isbllvl4.max = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.original_Pm = 2;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = false;
                isbllvl5.min = 0;
                isbllvl5.max = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.original_Pm = 2;
                ss.isbl.Add(isbllvl5);
                /////////////////////////////////////////////////
                return ss;
                #endregion
            }
            else if (_sortID == 9)
            {
                #region
                sort_Stats ss = new sort_Stats();
                ss.title = "transfert de pussance";
                ss.spellID = 9;
                ss.technique = "ninjutsu";
                ss.rang = "a";
                ss.element = "neutral";
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 2;
                isbllvl1.etenduModifiable = true;
                isbllvl1.ligneDeVue = false;
                isbllvl1.min = 0;
                isbllvl1.max = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl1.BonusRoundLeft = 2;
                isbllvl1.piBonus.puissance = 50;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 3;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = false;
                isbllvl2.min = 0;
                isbllvl2.max = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl2.BonusRoundLeft = 2;
                isbllvl2.piBonus.puissance = 100;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 4;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = false;
                isbllvl3.min = 0;
                isbllvl3.max = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl3.BonusRoundLeft = 2;
                isbllvl3.piBonus.puissance = 150;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 5;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = false;
                isbllvl4.min = 0;
                isbllvl4.max = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl4.BonusRoundLeft = 2;
                isbllvl4.piBonus.puissance = 200;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 6;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = false;
                isbllvl5.min = 0;
                isbllvl5.max = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl5.BonusRoundLeft = 2;
                isbllvl5.piBonus.puissance = 250;
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
                ss.element = "doton";
                // initialisation des stats du sorts selon le lvl

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 0;
                isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = false;
                isbllvl1.min = 0;
                isbllvl1.max = 0;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl1.BonusRoundLeft = 3;
                isbllvl1.piBonus.doton = 50;
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 0;
                isbllvl2.etenduModifiable = true;
                isbllvl2.ligneDeVue = false;
                isbllvl2.min = 0;
                isbllvl2.max = 0;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl2.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl2.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl2.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl2.BonusRoundLeft = 3;
                isbllvl2.piBonus.doton = 100;
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 0;
                isbllvl3.etenduModifiable = true;
                isbllvl3.ligneDeVue = false;
                isbllvl3.min = 0;
                isbllvl3.max = 0;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl3.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl3.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl3.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl3.BonusRoundLeft = 3;
                isbllvl3.piBonus.doton = 150;
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 0;
                isbllvl4.etenduModifiable = true;
                isbllvl4.ligneDeVue = false;
                isbllvl4.min = 0;
                isbllvl4.max = 0;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl4.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl4.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl4.pi = new PlayerInfo() { original_Pc = 4 };
                isbllvl4.BonusRoundLeft = 3;
                isbllvl4.piBonus.doton = 200;
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 0;
                isbllvl5.etenduModifiable = true;
                isbllvl5.ligneDeVue = false;
                isbllvl5.min = 0;
                isbllvl5.max = 0;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 5;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl5.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl5.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl5.pi = new PlayerInfo() { original_Pc = 4 };
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
                ss.element = "Doton";

                sort_Stats.info_sort_by_level isbllvl1 = new sort_Stats.info_sort_by_level();
                isbllvl1.etendu = 0;
                isbllvl1.etenduModifiable = false;
                isbllvl1.ligneDeVue = false;
                isbllvl1.min = 5;
                isbllvl1.max = 8;
                isbllvl1.cd = 40;
                isbllvl1.relanceInterval = 1;                   // combien de tours faut-il pour relancer le sort, si = 1 donc chaque tour le sort est jouable
                isbllvl1.relanceParJoueur = 1;                  // combien de fois le sort peux être lancé sur le même adversaire, si = 0 donc infini
                isbllvl1.relanceParTour = 1;                    // combien de fois le sort peux être lancé durant un tour, si = 0 donc infini
                isbllvl1.pi = new PlayerInfo() { original_Pc = 3 };
                ss.isbl.Add(isbllvl1);

                sort_Stats.info_sort_by_level isbllvl2 = new sort_Stats.info_sort_by_level();
                isbllvl2.etendu = 0;
                isbllvl2.etenduModifiable = false;
                isbllvl2.ligneDeVue = false;
                isbllvl2.min = 7;
                isbllvl2.max = 10;
                isbllvl2.cd = 35;
                isbllvl2.relanceInterval = 1;
                isbllvl2.relanceParJoueur = 1;
                isbllvl2.relanceParTour = 2;
                isbllvl2.pi = new PlayerInfo() { original_Pc = 3 };
                ss.isbl.Add(isbllvl2);

                sort_Stats.info_sort_by_level isbllvl3 = new sort_Stats.info_sort_by_level();
                isbllvl3.etendu = 0;
                isbllvl3.etenduModifiable = false;
                isbllvl3.ligneDeVue = false;
                isbllvl3.min = 9;
                isbllvl3.max = 12;
                isbllvl3.cd = 30;
                isbllvl3.relanceInterval = 1;
                isbllvl3.relanceParJoueur = 2;
                isbllvl3.relanceParTour = 2;
                isbllvl3.pi = new PlayerInfo() { original_Pc = 3 };
                ss.isbl.Add(isbllvl3);

                sort_Stats.info_sort_by_level isbllvl4 = new sort_Stats.info_sort_by_level();
                isbllvl4.etendu = 0;
                isbllvl4.etenduModifiable = false;
                isbllvl4.ligneDeVue = false;
                isbllvl4.min = 11;
                isbllvl4.max = 14;
                isbllvl4.cd = 25;
                isbllvl4.relanceInterval = 1;
                isbllvl4.relanceParJoueur = 2;
                isbllvl4.relanceParTour = 3;
                isbllvl4.pi = new PlayerInfo() { original_Pc = 3 };
                ss.isbl.Add(isbllvl4);

                sort_Stats.info_sort_by_level isbllvl5 = new sort_Stats.info_sort_by_level();
                isbllvl5.etendu = 0;
                isbllvl5.etenduModifiable = false;
                isbllvl5.ligneDeVue = false;
                isbllvl5.min = 13;
                isbllvl5.max = 16;
                isbllvl5.cd = 20;
                isbllvl5.relanceInterval = 1;
                isbllvl5.relanceParJoueur = 2;
                isbllvl5.relanceParTour = 3;
                isbllvl5.pi = new PlayerInfo() { original_Pc = 3 };
                ss.isbl.Add(isbllvl5);

                ss.cdDomBonnus = 10;

                return ss;
                #endregion
            }
            else
				throw new Exception("nan");
		}*/
	}

	public class SpellStats
	{
		public string title;
		public int spellID;
		public string technique;
		public string rang;
		public string element;
		public int CdDomBonus;                     // le nombre de dommage suplementaire que peux attendre un sort en CD
		public List<SpellInformationByLevel> Sibl = new List<SpellInformationByLevel>();
		public class SpellInformationByLevel
		{
			// contiens les infos des sorts selons leurs niveau qui sont ensuite stoqué dans la liste isbl, donc sort a la position 0 = lvl1 ...
			public bool etenduModifiable;
            public bool ligneDeVue;
            public int min, max, cd, etendu;
			public int relanceParTour;						// combien de fois le sort peux être lancé par tour
			public int relanceParJoueur;					// combien de fois le sort peux etre lancé sur le meme joueur
			public int relanceInterval;						// dans combiens de tour le sort pourra être relancé
			public Actor actor;                           // contien des infos des invoc par ex
            public int BonusRoundLeft;                      // s'il s'agit d'un bonus ou malus, ce variable compte combien de tour ceci est appliqué
            public Actor Bonus = new Actor();   // quand un bonus s'ajout au stats de joueur quand c'est positif ou se retire quand c negatif
		}
	}
	public class SortTuileInfo
	{
		// classe utilisé lors de l'affichage de la porté d'un sort, pour vérifier si une tuile est un obstacle ou non et si oui si sa dois bloquer la ligne de vus pour faciliter le controle
		public Point TuilePoint;
		public bool IsWalkable;
		public bool IsBlockingView;
	}
}

