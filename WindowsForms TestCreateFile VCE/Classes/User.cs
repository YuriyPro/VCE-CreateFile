using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_TestCreateFile_VCE.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        //////////Status
        public int? StatusId { get; set; }
        public virtual Status Status { get; set; }
        //public ICollection<TestResult> TestResults { get; set; }

        //public User()
        //{
        //    TestResults = new List<TestResult>();
        //}

        //public override string ToString()
        //{
        //    return Name;
        //}
    }
}
