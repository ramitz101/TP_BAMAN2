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
                    SAuthentifier(Constante.ADMINISTRATEUR);
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
                SAuthentifier(Constante.ADMINISTRATEUR);
                if (Confirmer)
                {
                    if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR)
                        return true;
                    else
                    {
                        
                        return false;
                    }
                }
                else
                    return false;
            }
        }
        public bool ValiderRoleUtils()
        {

            if (ValideEmploye())
            {
                if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR || EcranAccueil.Employe.SonRole.Code == Constante.UTILISATEUR)
                    return true;
                else
                {
                    SAuthentifier(Constante.UTILISATEUR);
                    if (Confirmer)
                    {
                        if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR || EcranAccueil.Employe.SonRole.Code == Constante.UTILISATEUR)
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
                SAuthentifier(Constante.UTILISATEUR);
                if (Confirmer)
                {
                    if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR || EcranAccueil.Employe.SonRole.Code == Constante.UTILISATEUR)
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

        public void SAuthentifier(string pRoleDemander)
        {
            FenetreAuthentification FN = new FenetreAuthentification(this, pRoleDemander);
            FN.ShowDialog();
        }

    }
}
