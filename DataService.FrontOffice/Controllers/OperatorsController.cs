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
        public List<Operator> Index()
        {
            return _operatorService.GetOperatorsPaged(1, 1, string.Empty, null, string.Empty);
        }
    }
}
