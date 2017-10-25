using System;
using System.Collections.Generic;

namespace Barman
{
    public static class Generer
    {
        public static string genererCode(List<string> lstCode)
        {
            bool resultat = false;
            int codeGenere;
            Random rnd = new Random();


            do
            {
                codeGenere = rnd.Next(100000, 999999);
                resultat = ValideCode(codeGenere, lstCode);
            } while (resultat);
            return codeGenere.ToString();
        }

        private static bool ValideCode(int codeGenere, List<string> lstCode)
        {
            bool codeValide = false;
            foreach (var code in lstCode)
            {
                if (code.Length > 0)
                {
                    if (codeGenere == int.Parse(code))
                        codeValide = true;
                    else
                        codeValide = false;
                }
            }


            return codeValide;
        }
    }
}
