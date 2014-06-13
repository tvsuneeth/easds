using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twg.chk.DataService.Business;
using twg.chk.DataService.chkData.Infrastructure;

namespace twg.chk.DataService.chkData.Repository
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
            List<String> categoryFilterList = new List<String>();
            List<Operator> operatorObjs = new List<Operator>();

            using (var connection = CreateConnection())
            {
                using (var cmd = CreateCommand(connection, "chk.GetCompaniesPaged"))
                {
                    AddCommandParameter(cmd, "@StartsWith", string.Empty);
                    AddCommandParameter(cmd, "@SearchPhrase", string.Empty);
                    AddCommandParameter(cmd,"@CategoryFilter", String.Join(",", categoryFilterList));
                                       

                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Operator operatorObj = new Operator()
                        {
                            Id = GetValue<int>(reader["liCompanyID"]),
                            Name = GetValue<string>(reader["sName"]),
                            Description = GetValue<string>(reader["sDescription"]),
                            Telephone = GetValue<string>(reader["sTelephone"]),
                            Fax = GetValue<string>(reader["sFax"]),
                            Email = GetValue<string>(reader["sEmail"]),
                            URLTitle = GetValue<string>(reader["sURLTitle"]),
                            URL = GetValue<string>(reader["sURL"]),
                            Notes = GetValue<string>(reader["sNotes"]),
                            Activities = GetValue<string>(reader["sActivities"]),
                            TimeLine = GetValue<string>(reader["sTimeLine"]),
                            FinancialSnapshot = GetValue<string>(reader["sFinancialSnapshot"]),
                            OperatingData = GetValue<string>(reader["sOperatingData"]),
                            Strategy = GetValue<string>(reader["sStrategy"]),
                            KeyPeople = GetValue<string>(reader["sKeyPeople"]),
                            Commentary = GetValue<string>(reader["sCommentary"]),
                            ChiefExecutive = GetValue<string>(reader["sChiefExecutive"])
                        };

                        int imageId = GetValue<int>(reader["liAssetID"]);
                        if (imageId != 0)
                        {                           
                            operatorObj.LogoImage = new Image()
                            {
                                Id = imageId,
                                Name = GetValue<string>(reader["sAssetName"]),
                                Width = GetValue<int>(reader["iWidth"]),
                                Height = GetValue<int>(reader["iHeight"]),
                                Extension = GetValue<string>(reader["sFileExt"]),
                                CreatedDate = GetValue<DateTime>(reader["imageCreatedDate"]),
                                LastModifiedDate = GetValue<DateTime>(reader["imageLastModifiedDate"])
                            };
                        }

                        operatorObj.Address = new Address()
                        {
                            Line1 = GetValue<string>(reader["sAddress1"]),
                            Line2 = GetValue<string>(reader["sAddress2"]),
                            Town = GetValue<string>(reader["sTown"]),
                            County = GetValue<string>(reader["sCounty"]),
                            PostCode = GetValue<string>(reader["sPostcode"]),
                            Country = GetValue<string>(reader["sCountry"]),
                            CountryCode = GetValue<string>(reader["sCountryCode"])
                        };

                        operatorObjs.Add(operatorObj);
                    }
                }
            }
            return operatorObjs;

        }

        public List<TaxonomyCategory> GetOperatorTaxonomy(int companyId)
        {

            var categories = new List<TaxonomyCategory>();
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(connection, "chk.GetCompanyTaxonomies"))
                {
                    command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                    var sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var category = new TaxonomyCategory
                        {
                            CategoryId = Convert.ToInt32(sqlReader["liCategoryID"]),
                            CategoryName = Convert.ToString(sqlReader["sCategoryName"]),
                        };
                        categories.Add(category);
                    }

                    sqlReader.NextResult();

                    while (sqlReader.Read())
                    {
                        int categoryId = Convert.ToInt32(sqlReader["liCategoryID"]);
                        var category = categories.Find(i => i.CategoryId == categoryId);
                        var categoryItem = new TaxonomyCategoryItem()
                        {
                            CategoryItemId = Convert.ToInt32(sqlReader["liCategoryItemID"]),
                            CategoryItemName = Convert.ToString(sqlReader["sItemName"]),
                            ParentItemId = DBNull.Value.Equals(sqlReader["liParentID"]) ? null : (int?)Convert.ToInt32(sqlReader["liParentID"])
                        };
                        category.AddItem(categoryItem);
                    }
                }
            }
            return categories;
        }
    }
}
