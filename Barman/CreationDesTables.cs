﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barman.EmployeDossier;
using Barman.BouteilleDossier;

namespace Barman
{
    public static class CreationDesTables
    {
        public static PdfPTable CreerTableInventaire(PdfPTable table, ObservableCollection<Bouteille> lstBouteilles)
        {


            PdfPCell cell = new PdfPCell();
            Phrase text = new Phrase();
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;

            text = new Phrase("Marque");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Type");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Volume ini.");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Vol restant");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Numéro");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Emplac.");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_LEFT;

            text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            foreach (Bouteille b in lstBouteilles)
            {


                if (b.SaMarque.Nom.Length >= 10)
                {
                    string nom = b.SaMarque.Nom.Substring(0, 9);
                    nom += ".";
                    text = new Phrase(nom);
                    cell = new PdfPCell(text);
                }
                else
                {
                    text = new Phrase(b.SaMarque.Nom);
                    cell = new PdfPCell(text);

                }
                table.AddCell(cell);


                text = new Phrase(b.SaMarque.SonTypeAlcool.Nom);
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(b.VolumeInitial.ToString());
                cell = new PdfPCell(text);
                table.AddCell(cell);                

                text = new Phrase(b.VolumeRestant.ToString());
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(b.Numero.ToString());
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(b.SonEmplacement.Nom);
                cell = new PdfPCell(text);
                table.AddCell(cell);

            }
            return table;

        }

        public static PdfPTable CreerTableEmploye(PdfPTable table, ObservableCollection<Employe> lstEmploye)
        {


            PdfPCell cell = new PdfPCell();
            Phrase text = new Phrase();
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;

            text = new Phrase("Nom");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Prénome");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Téléphone");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Date d'embauche");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("NAS");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Role");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);

            text = new Phrase("Code");
            cell = new PdfPCell(text);
            cell.Phrase.Font.SetStyle(Font.BOLD);
            table.AddCell(cell);


            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_LEFT;

            text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            foreach (Employe e in lstEmploye)
            {


                if (e.Nom.Length >= 10)
                {
                    string nom = e.Nom.Substring(0, 9);
                    nom += ".";
                    text = new Phrase(nom);
                    cell = new PdfPCell(text);
                }
                else
                {
                    text = new Phrase(e.Nom);
                    cell = new PdfPCell(text);

                }
                table.AddCell(cell);

                if (e.Prenom.Length >= 10)
                {
                    string prenom = e.Prenom.Substring(0, 9);
                    prenom += ".";
                    text = new Phrase(prenom);
                    cell = new PdfPCell(text);
                }
                else
                {
                    text = new Phrase(e.Prenom);
                    cell = new PdfPCell(text);

                }
                table.AddCell(cell);
                

                text = new Phrase(e.Telephone);
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(e.DateEmbauche.Date.ToString("dd/MM/yyyy"));
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(e.NAS);
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(e.SonRole.Code);
                cell = new PdfPCell(text);
                table.AddCell(cell);

                text = new Phrase(e.CodeEmploye);
                cell = new PdfPCell(text);
                table.AddCell(cell);

            }
            return table;

        }

    }
}
