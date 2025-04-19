using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.Models
{
    [Table("games")]
    public class Game
    {
        [Key]
        [Column("id_game")]
        public int id_game { get; set; }

        [Required]
        [ForeignKey("Player")]
        [Column("id_player")]
        public int id_player { get; set; }

        [Required]
        [Column("difficulty")]
        [StringLength(50)]
        public string difficulty { get; set; }

        [Required]
        [Column("result")]
        [StringLength(50)]
        public string result { get; set; }

        [Required]
        [Column("time")]
        public TimeSpan time { get; set; }

        [Required]
        [Column("movements")]
        public int movements { get; set; }

        [Required]
        [Column("score")]
        public int score { get; set; }

        // Propiedad de navegación
        public Player Player { get; set; }
    }
}
