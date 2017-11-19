using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.DTO.Animals
{
    public class EditAnimalModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PasswordNo { get; set; }

        public string ImageUrl { get; set; }
    }
}
