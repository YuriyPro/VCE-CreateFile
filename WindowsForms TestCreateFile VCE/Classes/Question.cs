using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_TestCreateFile_VCE.Classes
{
    [Serializable]
    public class Question
    {
        public string Title { get; set; }
        public string Text { get; set; }

       public List<Answer>answers=new List<Answer>();

    }
}
