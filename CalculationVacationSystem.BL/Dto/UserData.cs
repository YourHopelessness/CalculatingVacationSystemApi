using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Dto
{
    /// <summary>
    /// For auth claims
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// Id of user
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Role of user
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// User FullName
        /// </summary>
        public string FullName { get; set; }
    }
}
