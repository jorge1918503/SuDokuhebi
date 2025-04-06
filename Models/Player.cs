using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.Models
{
    public class Player
    {
        [Key]
        public int id_player { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        [MaxLength(255)]
        public string password { get; set; } // Guardar hashes en producción

        [Required]
        [MaxLength(50)]
        public int highestScore { get; set; }
    }
}
