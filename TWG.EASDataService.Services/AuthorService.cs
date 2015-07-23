using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Repository;

namespace TWG.EASDataService.Services
{
    public interface IAuthorService 
    {
        List<Author> GetAllAuthors();
        Author GetAuthorDetailsByID(int authorID);
    }

    public class AuthorService : IAuthorService
    {
        IAuthorRepository authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public List<Author> GetAllAuthors()
        {
            return authorRepository.GetAllAuthors();
        }

        public Author GetAuthorDetailsByID(int authorID)
        {
            throw new NotImplementedException();
        }
    }
}
