using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Dto
{
    public class AuthentificationDto
    {
        /// <summary>
        /// Username of user
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password of user
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
