using EGService.Business.Services;
using EGService.Core.IServices;
using EGService.Core.Models.ViewModels.ExamGenrator;
using EGService.WebAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace EGService.WebAPI.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamGenratorController: ControllerBase
    {
        private IExamGenrator _examGenrator;

        public ExamGenratorController(IExamGenrator examGenrator)
        {
            _examGenrator = examGenrator;
        }
        [HttpPost]
        [Route("Genarate")]
        //[JwtAuthentication()]

        public ResultModel Genarate(contextModel context) {
        
        var resualt=_examGenrator.Genarate(context);
            return resualt;
        
        }
    }
}
