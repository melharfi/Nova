using System;
using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;

namespace SERVER
{
    public static class Security
    {
        public static string allowedCharPseudo;
        public static string allowedCharPwd;

        public static bool check_valid_pseudo(string pseudo)
        {
            bool valide = !pseudo.Where((t, cnt) => allowedCharPseudo.IndexOf(pseudo.Substring(cnt, 1), StringComparison.Ordinal) == -1).Any();

            // check contre les caractères non autorisés

            // check contre l'utilisation multiple du caractère - qui normalement 1 seul fois dois etre utilisé
            List<char> pseudoSplited = new List<char>(pseudo.ToCharArray());
            if (pseudoSplited.FindAll(f => f == '-').Count > 1)
                valide = false;

            // check si le pseudo commence par un caractère non alphabétique
            if ("1234567890-".IndexOf(pseudo[0]) != -1 || "1234567890-".IndexOf(pseudo[pseudo.Length - 1]) != -1)
                valide = false;

            return valide;
        }

        public static bool check_valid_user(string user)
        {
            const string allowedCharUser = "azertyuiopqsdfghjklmwxcvbn1234567890-";
            // check contre les caractères non autorisés
            bool valide = !user.Where((t, cnt) => allowedCharUser.IndexOf(user.Substring(cnt, 1), StringComparison.Ordinal) == -1).Any();

            // check contre l'utilisation multiple du caractère - qui normalement 1 seul fois dois etre utilisé
            List<char> userSplited = new List<char>(user.ToCharArray());
            if (userSplited.FindAll(f => f == '-').Count > 1)
                valide = false;

            // check si le user commence par un caractère non alphabétique
            if ("1234567890-".IndexOf(user[0]) != -1)
                valide = false;
            return valide;
        }

        public static bool check_valid_pwd(string pwd)
        {
            return !pwd.Where((t, cnt) => allowedCharPwd.IndexOf(pwd.Substring(cnt, 1), StringComparison.Ordinal) == -1).Any();
        }

        public static bool check_valid_cmd(string cmd)
        {
            const string restrictedChar = "\',;-&/\\=`[]$%}";
            string replace = cmd.Replace("\"", "");

            bool found = replace.Where((t, cnt) => restrictedChar.IndexOf(replace.Substring(cnt, 1), StringComparison.Ordinal) == -1).Any();
            return !found;
        }

        /*public static void User_banne(string msg, NetIncomingMessage im)
        {
            Console.WriteLine("Client " + ((Actor)im.SenderConnection.Tag).Username + " bannie pour 1 semaine");
            // ajout du client parmis les joueurs banni
            int timestamp = CommonCode.ReturnTimeStamp();
            int censure = 60 * 60 * 24 * 7;		// 1 semaine
            mysql.banneduser bannedUser = new mysql.banneduser { user = ((Actor)im.SenderConnection.Tag).Username, reson = msg, temps = timestamp + censure };
            ((List<mysql.banneduser>)DataBase.DataTables.banneduser).Add(bannedUser);

            im.SenderConnection.Disconnect(Enums.DisconnectReason.disconnectReason.USER_BANNED.ToString());
        }*/

        public static void User_banne(string msg, NetConnection nc)
        {
            Console.WriteLine("Client " + ((Actor)nc.Tag).Username + " bannie pour 1 semaine");
            // ajout du client parmis les joueurs banni
            int timestamp = CommonCode.ReturnTimeStamp();
            const int censure = 60 * 60 * 24 * 7; // 1 semaine
            mysql.banneduser bannedUser = new mysql.banneduser { user = ((Actor)nc.Tag).Username, reson = msg, temps = timestamp + censure };
            ((List<mysql.banneduser>)DataBase.DataTables.banneduser).Add(bannedUser);
            nc.Disconnect(Enums.DisconnectReason.disconnectReason.USER_BANNED.ToString());
        }

        public static bool check_valid_msg(string msg)
        {
            const string allowedChar = "•";
            return !msg.Where((t, cnt) => allowedChar.IndexOf(msg.Substring(cnt, 1), StringComparison.Ordinal) != -1).Any();
        }
    }
}

