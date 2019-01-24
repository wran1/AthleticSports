
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.Dictionary
{
    public class Train : DbSetBase
    {
        public Train()
        {

            SystemId = "000";
            Enable = true;
        }

        [MaxLength(100)]
        [Required]

        public string Name { get; set; }

      
        [MaxLength(40)]
        public string FullName { get; set; }

        [MaxLength(30)]
        [Required]
        public string SystemId { get; set; }

        public bool Enable { get; set; }


        [ScaffoldColumn(false)]
        [NotMapped]
        public bool Selected { get; set; }
    }
}
