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
using WindowsForms_TestCreateFile_VCE.Forms;

namespace WindowsForms_TestCreateFile_VCE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text))
            {
              
                    if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text))
                    {
                        using (MyContext context = new MyContext())
                        {
                            string str = Additional.CreateMD5Hash(textBox2.Text);
                            User user = context.Users.Where(x => x.Name == textBox1.Text && x.Password == str && x.Status.Name == "Teachers").SingleOrDefault();

                            if (user != null)
                            {
                                MessageBox.Show("Hi!!!");

                                Form1.ActiveForm.Hide();
                                Work w = new Work();
                                w.ShowDialog();
                                Close();
                            }
                            else MessageBox.Show("Login or Password is incorect");

                        }
                       
                    }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        //Registration
        private void label3_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Hide();
            Registration r = new Registration();
            r.ShowDialog();
            Close();
        }
    }
}
