using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER
{
    class CheckQuete
    {
        public static int isSubmitedQuest(string quete, Actor pi)
        {
            // verifie si une quette a été faite
            // return = -3 quand le joueur n'est pas dans la map de pnj
            // return = -2 quand la quete demandé par le client n'est pas répértorié sur le serveur se qui est impossible
            // return = -1 quand la quete n'est pas associé au joueur, il la pas encore commencé
            // return = 0 la quete a été commencé par le joueur mais pas encore achevé
            // return = 1 la quete a été validé par le joueur

            if(quete == "FirstFight")
            {
                // on vérifie si le joueur a préalablement fait la quête FirstFight Pnj Iruka
                // on check si le joueur est dans une autre map
                if (pi.map != "Start")
                    return -3;

                mysql.quete q = (DataBase.DataTables.quete as List<mysql.quete>).Find(f => f.pseudo == pi.Pseudo && f.nom_quete == quete);

                if(q != null)
                    return q.submited;
                return -1;
            }
            return -2;
        }
    }
}
