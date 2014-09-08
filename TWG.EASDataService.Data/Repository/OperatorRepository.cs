using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWG.EASDataService.Business;
using TWG.EASDataService.Data.Infrastructure;
using TWG.EASDataService.Data.Extensions;

namespace TWG.EASDataService.Data.Repository
{
    public interface IOperatorRepository
    {
        List<Operator> GetAll();
        List<TaxonomyCategory> GetOperatorTaxonomy(int companyId);
    }

    public class OperatorRepository : DbRepositoryBase, IOperatorRepository
    {
        public OperatorRepository()
        { }

        public List<Operator> GetAll()
        {            
            List<Operator> operatorObjs = new List<Operator>();

            var list = GetListWithCustomMapping<Operator>(@"chk.GetCompaniesPaged", new { @StartsWith = String.Empty, @SearchPhrase = String.Empty, @CategoryFilter = String.Empty }, MapDataRrowToOperator);
            return list;                  
        }


        public Operator MapDataRrowToOperator(IDataRecord record)
        {
            Operator operatorObj = new Operator()
            {
                Id = record.GetValue<int>("liCompanyID"),
                Name = record.GetValue<string>("sName"),
                Description = record.GetValue<string>("sDescription"),
                Telephone = record.GetValue<string>("sTelephone"),
                Fax = record.GetValue<string>("sFax"),
                Email = record.GetValue<string>("sEmail"),
                URLTitle = record.GetValue<string>("sURLTitle"),
                URL = record.GetValue<string>("sURL"),
                Notes = record.GetValue<string>("sNotes"),
                Activities = record.GetValue<string>("sActivities"),
                TimeLine = record.GetValue<string>("sTimeLine"),
                FinancialSnapshot = record.GetValue<string>("sFinancialSnapshot"),
                OperatingData = record.GetValue<string>("sOperatingData"),
                Strategy = record.GetValue<string>("sStrategy"),
                KeyPeople = record.GetValue<string>("sKeyPeople"),
                Commentary = record.GetValue<string>("sCommentary"),
                ChiefExecutive = record.GetValue<string>("sChiefExecutive")
            };

            int imageId = record.GetValue<int>("liAssetID");
            if (imageId != 0)
            {
                operatorObj.LogoImage = new Image()
                {
                    Id = imageId,
                    Name = record.GetValue<string>("sAssetName"),
                    Width = record.GetValue<int>("iWidth"),
                    Height = record.GetValue<int>("iHeight"),
                    Extension = record.GetValue<string>("sFileExt"),
                    CreatedDate = record.GetValue<DateTime>("imageCreatedDate"),
                    LastModifiedDate = record.GetValue<DateTime>("imageLastModifiedDate")
                };
            }

            operatorObj.Address = new Address()
            {
                Line1 = record.GetValue<string>("sAddress1"),
                Line2 = record.GetValue<string>("sAddress2"),
                Town = record.GetValue<string>("sTown"),
                County = record.GetValue<string>("sCounty"),
                PostCode = record.GetValue<string>("sPostcode"),
                Country = record.GetValue<string>("sCountry"),
                CountryCode = record.GetValue<string>("sCountryCode")
            };

            return operatorObj;
        }

        public List<TaxonomyCategory> GetOperatorTaxonomy(int companyId)
        {

            var categories = new List<TaxonomyCategory>();
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(connection, "chk.GetCompanyTaxonomies", new { @CompanyId = companyId  }))
                {                    
                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var category = new TaxonomyCategory
                        {
                            CategoryId = sqlReader.GetValue<int>("liCategoryID"),
                            CategoryName = sqlReader.GetValue<string>("sCategoryName")
                        };
                        categories.Add(category);
                    }

                    sqlReader.NextResult();

                    while (sqlReader.Read())
                    {
                        int categoryId = sqlReader.GetValue<int>("liCategoryID");
                        var category = categories.Find(i => i.CategoryId == categoryId);
                        var categoryItem = new TaxonomyCategoryItem()
                        {
                            CategoryItemId =sqlReader.GetValue<int>("liCategoryItemID"),
                            CategoryItemName = sqlReader.GetValue<string>("sItemName"),
                            ParentItemId = sqlReader.GetValue<int>("liParentID") 
                        };
                        category.AddItem(categoryItem);
                    }
                }
            }
            return categories;
        }
    }
}
