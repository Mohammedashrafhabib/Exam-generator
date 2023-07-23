using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGService.Core.Models.ViewModels.ExamGenrator
{
    public class ResultModel
    {
        public IList<KeyValuePair< string, string>> QuestionsAnswers { get; set; }
    }
}
