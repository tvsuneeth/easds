using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWG.EASDataService.Business;

namespace TWG.EASDataService.Data.Repository
{
    public interface IAuthorRepository
    {
        List<Author> GetAllAuthors();
        Author GetAuthorDetailsByID(int authorID);
    }

    public class AuthorRepository : DbRepositoryBase, IAuthorRepository
    {
        public List<Author> GetAllAuthors()
        {            
            var authors = new List<Author>();
            authors = GetListWithAutoMapping<Author>("[EASDS].[GetAllAuthors]", null);
            return authors;            
        }

        public Author GetAuthorDetailsByID(int authorID)
        {
            throw new NotImplementedException();
        }
    }
}
