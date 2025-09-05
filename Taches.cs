using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TachesTestDeploie
{
    internal class Taches
    {
        public string Nom { get; set; }
        public string Description { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateFermeture { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
