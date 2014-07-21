using System;
using System.Collections.Generic;
using System.Linq;
using TWG.EASDataService.Data.Repository;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data;

namespace TWG.EASDataService.Services
{
    public interface IOperatorService
    {        
        List<Operator> GetAll();
    }

    public class OperatorService : IOperatorService
    {
        private IOperatorRepository _opetorRepository;
        public OperatorService( IOperatorRepository operatorRepository)
        {
            _opetorRepository = operatorRepository;
        }       

       
        public List<Operator> GetAll()
        {
            var list =  _opetorRepository.GetAll();
            foreach (var op in list)
            {
                var taxonomy = _opetorRepository.GetOperatorTaxonomy(op.Id);
                if(taxonomy!=null && taxonomy.Count>0)
                { 
                    op.Taxonomy = new CompanyTaxonomy() { CategoryAssignments = taxonomy };
                }
            }
            return list;
        }
    }
}
