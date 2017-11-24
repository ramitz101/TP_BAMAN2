using Barman.ViewAutreDossier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Authentification
    {
        public bool Confirmer = false;
        public bool ValiderRoleAdmin()
        {

            if (ValideEmploye())
            {
                if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR)
                    return true;
                else
                {
                    SAuthentifier();
                    if (Confirmer)
                    {
                        if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            else
            {
                SAuthentifier();
                if (Confirmer)
                {
                    if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        private bool ValideEmploye()
        {
            if (EcranAccueil.Employe == null)
                return false;
            else
                return true;
        }

        private void SAuthentifier()
        {
            FenetreAuthentification FN = new FenetreAuthentification(this);
            FN.ShowDialog();
        }

    }
}
