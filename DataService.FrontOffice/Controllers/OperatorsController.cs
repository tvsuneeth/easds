using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

using twg.chk.DataService.api;
using twg.chk.DataService.Business;
using twg.chk.DataService.FrontOffice.Models;
using twg.chk.DataService.FrontOffice.Helpers;
using twg.chk.DataService.chkData.Repository;


namespace twg.chk.DataService.FrontOffice.Controllers
{
    public class OperatorsController : ApiController
    {
        private IOperatorService _operatorService;
        public OperatorsController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }
       

        [HttpGet]
        [Route("operators", Name = "GetAllOperators")]
        [Authorize(Roles = "frontofficegroup")]
        public List<Operator> Index()
        {
            var operators = _operatorService.GetAll();
            if(operators.Count==0)
            {
                return null;
            }
            return operators;
        }
    }
}
