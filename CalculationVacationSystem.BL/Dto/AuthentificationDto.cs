using System.ComponentModel.DataAnnotations;

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
