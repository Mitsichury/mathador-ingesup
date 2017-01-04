using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathadorLibrary
{
    public class DatabaseEntry
    {
        public string NomJoueur { get; }
        public int Points { get; }
        public int TempsMoyen { get; }
        public int NombreMatahdor { get; }
        public int PointsMoyen { get; }

        public DatabaseEntry(string nomJoueur, int points, int tempsMoyen, int nombreMatahdor, int pointsMoyen)
        {
            NomJoueur = nomJoueur;
            Points = points;
            TempsMoyen = tempsMoyen;
            NombreMatahdor = nombreMatahdor;
            PointsMoyen = pointsMoyen;
        }
    }
}
