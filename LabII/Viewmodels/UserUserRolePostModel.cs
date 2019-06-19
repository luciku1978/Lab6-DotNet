using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Viewmodels
{
    public class UserUserRolePostModel
    {
        public int UserId { get; set; }
        public string UserRoleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }


    }
}
