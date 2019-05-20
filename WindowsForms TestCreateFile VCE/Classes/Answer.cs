using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_TestCreateFile_VCE.Classes
{
    [Serializable]
   public class Answer
    {
        public bool Correct { get; set; }
        public string Text { get; set; }

        public float Weight { get; set; }

    }
}
