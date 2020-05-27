using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Enums
{
    public class Species
    {
        public enum Name
        {
            // contien les different type de joueur, pour faire la differance entre une invoc est un joueur et un pnj ...
            // utilisé dans la classe PlayerInfo.genre
            // humain = un utilisateur réel
            // pnj = pnj qui agi comme un humain concernant les interactions, comme dialogue, en combat il vérou sa position, il est pris en charge dans les controles comme celui qui vérifie si tous les joueurs ont vérouillés leurs position pour lancer le combat, seul les invocs ne sont pas compté puisqu'ils ne doivents pas être exister
            // invoc = une invocation d'un joueur, qui n'est pas pris en charge par les contrôles de validations
            Human, Pnj, Summon
        }
    }
}
