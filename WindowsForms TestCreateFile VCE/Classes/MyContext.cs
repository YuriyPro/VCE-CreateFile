using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace WindowsForms_TestCreateFile_VCE.Classes
{
    public class MyContext : DbContext
    {
        //static MyContext()
        //{
        //    Database.SetInitializer<MyContext>(new Inicialization());
        //}
        public MyContext() : base("DbConections")
        {

        }
        public DbSet<Status> Status { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        //public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
