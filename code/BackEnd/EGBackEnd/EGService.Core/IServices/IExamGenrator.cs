using EGService.Core.Models.ViewModels.ExamGenrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGService.Core.IServices
{
    public interface IExamGenrator
    {
        public ResultModel Genarate(contextModel Context);

    }
}
