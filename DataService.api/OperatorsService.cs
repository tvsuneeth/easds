using System;
using System.Collections.Generic;
using System.Linq;
using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData;

namespace twg.chk.DataService.api
{
    public interface IOperatorService
    {
        List<Operator> GetOperatorsPaged(int pageNumber, int pageSize, String searchPhrase, List<String> categoryFilterList, String startsWith);
    }

    public class OperatorService : IOperatorService
    {
        private IOperatorRepository _opetorRepository;
        public OperatorService( IOperatorRepository operatorRepository)
        {
            _opetorRepository = operatorRepository;
        }       

        public List<Operator> GetOperatorsPaged(int pageNumber, int pageSize, String searchPhrase, List<String> categoryFilterList, String startsWith)
        {
            return _opetorRepository.GetOperatorsPaged(1, 1, string.Empty, null, string.Empty);
        }
    }
}
