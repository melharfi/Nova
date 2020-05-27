using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MMORPG
{
    public static class Security
    {
        public static bool check_valid_pseudo(string pseudo)
        {
            // used LoadinMap
            string allowedChar = "azertyuiopqsdfghjklmwxcvbn0123456789-";
            bool valide = true;

            for (int cnt = 0; cnt < pseudo.Length; cnt++)
            {
                if (allowedChar.IndexOf(pseudo.Substring(cnt, 1)) == -1)
                {
                    valide = false;
                    break;
                }
            }
            return valide;
        }
        public static bool check_valid_user(string user)
        {
            // used LoadinMap
            string allowedChar = "azertyuiopqsdfghjklmwxcvbn0123456789-";
            bool valide = true;

            for (int cnt = 0; cnt < user.Length; cnt++)
            {
                if (allowedChar.IndexOf(user.Substring(cnt, 1)) == -1)
                {
                    valide = false;
                    break;
                }
            }

            // check contre l'utilisation multiple du caractère -
            List<char> userSplited = new List<char>(user.ToCharArray());
            if (userSplited.FindAll(f => f == '-').Count > 1)
                valide = false;

            // check si le nom d'utilisateur commance par un caractère non alphabétique
            if ("1234567890-".IndexOf(userSplited[0]) != -1)
                valide = false;
            return valide;
        }
        public static bool check_valid_pwd(string pseudo)
        {
            // used LoadingMap
            string allowedChar = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN0123456789_-$*+@";
            bool valide = true;

            for (int cnt = 0; cnt < pseudo.Length; cnt++)
            {
                if (allowedChar.IndexOf(pseudo.Substring(cnt, 1)) == -1)
                {
                    valide = false;
                    break;
                }
            }
            return valide;
        }
        public static bool check_valid_msg(string msg)
        {
            string allowedChar = "•";
            bool valide = true;

            for (int cnt = 0; cnt < msg.Length; cnt++)
            {
                if (allowedChar.IndexOf(msg.Substring(cnt, 1)) != -1)
                {
                    valide = false;
                    break;
                }
            }
            return valide;
        }
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));
            return sb.ToString();
        }
    }
}
