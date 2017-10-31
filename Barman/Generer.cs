using System;
using System.Collections.Generic;
using Barman.BouteilleDossier;
using Barman.BouteilleDossier.Hibernate;

namespace Barman
{
    public static class Generer
    {
        private static Random rnd = new Random();

        public static string genererCode(List<string> lstCode)
        {
            bool resultat = false;
            int codeGenere;
            


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

        // Cette fonction retourne un code pour une bouteille, elle s'assure qu'aucune bouteille détien le code avant de retourné
        public static string GenererCodeBouteille()
        {
            string code;
            bool unique = true;
            List<Bouteille> listB = new List<Bouteille>();
            listB = HibernateBouteilleService.RetrieveAll();

            do
            {
                unique = true;
                code = rnd.Next(10000, 99999).ToString();
                foreach (var i in listB)
                {
                    if (i.Numero == code)
                        unique = false;
                }

            } while (!unique);
            


            return code;
        }


    }
}
