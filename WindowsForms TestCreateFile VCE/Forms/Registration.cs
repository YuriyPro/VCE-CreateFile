using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms_TestCreateFile_VCE.Classes;

namespace WindowsForms_TestCreateFile_VCE.Forms
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }
        //Add teachers
        private void button4_Click(object sender, EventArgs e)
        {
            using (MyContext context = new MyContext())
            {
                
                User person = new User()
                {
                    Name = textBox1.Text,
                    Password = Additional.CreateMD5Hash(textBox2.Text),
                    StatusId = context.Status.Where(x => x.Name == "Teachers").Select(x => x.Id).Single()

                };
                context.Users.Add(person);
                context.SaveChanges();
                label3.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Registration.ActiveForm.Hide();
            Form1 en = new Form1();
            en.ShowDialog();
            Close();
        }
    }
}
