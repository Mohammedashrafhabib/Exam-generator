using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGService.Core.Models.ViewModels.ExamGenrator
{
    public class contextModel
    {
        public string context { get; set; }
        public bool MCQ { get; set; }
        public bool COMPLETE { get; set; }

        public bool WH { get; set; }
        public bool T_F { get; set; }


    }
}
