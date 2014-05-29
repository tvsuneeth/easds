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
        List<Operator> GetOperatorsPaged(int pageNumber, int pageSize, String searchPhrase, List<String> categoryFilterList, String startsWith);
    }

    public class OperatorRepository : IOperatorRepository
    {
        public OperatorRepository()
        { }

        public List<Operator> GetOperatorsPaged(int pageNumber, int pageSize, String searchPhrase, List<String> categoryFilterList, String startsWith)
        {
            if (categoryFilterList == null)
                categoryFilterList = new List<String>();
            
             List<Operator> operatorObjs = new List<Operator>();            

             using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyChk"].ConnectionString))
             {
                using (var cmd = new SqlCommand())
                {
                    connection.Open();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = @"chk.GetCompaniesPaged";

                    cmd.Parameters.Add(new SqlParameter("@StartsWith", startsWith));
                    cmd.Parameters.Add(new SqlParameter("@SearchPhrase", searchPhrase));
                    cmd.Parameters.Add(new SqlParameter("@CategoryFilter", String.Join(",", categoryFilterList)));
                   // cmd.Parameters.Add(new SqlParameter("@CurrentPage", pageNumber));
                   // cmd.Parameters.Add(new SqlParameter("@PageSize", pageSize));


                    IDataReader reader = cmd.ExecuteReader();

                     while (reader.Read())
                     {
                         Operator operatorObj = new Operator();
                         operatorObj.Id = Convert.ToInt32(reader["liCompanyID"]);
                         operatorObj.Name = Convert.ToString(reader["sName"]);
                         operatorObj.Description = Convert.ToString(reader["sDescription"]);
                         operatorObj.Address1 = Convert.ToString(reader["sAddress1"]);
                         operatorObj.Address2 = Convert.ToString(reader["sAddress2"]);
                         operatorObj.Town = Convert.ToString(reader["sTown"]);
                         operatorObj.County = Convert.ToString(reader["sCounty"]);
                         operatorObj.Postcode = Convert.ToString(reader["sPostcode"]);
                         operatorObj.CountryCode = Convert.ToString(reader["sCountryCode"]);
                         operatorObj.Telephone = Convert.ToString(reader["sTelephone"]);
                         operatorObj.Fax = Convert.ToString(reader["sFax"]);
                         operatorObj.Email = Convert.ToString(reader["sEmail"]);
                         operatorObj.URLTitle = Convert.ToString(reader["sURLTitle"]);
                         operatorObj.URL = Convert.ToString(reader["sURL"]);
                         operatorObj.LogoID = Convert.ToInt32((reader["liLogoID"] != DBNull.Value) ? reader["liLogoID"] : 0);
                         operatorObj.Notes = Convert.ToString(reader["sNotes"]);
                         operatorObj.Activities = Convert.ToString(reader["sActivities"]);
                         operatorObj.TimeLine = Convert.ToString(reader["sTimeLine"]);
                         operatorObj.FinancialSnapshot = Convert.ToString(reader["sFinancialSnapshot"]);
                         operatorObj.OperatingData = Convert.ToString(reader["sOperatingData"]);
                         operatorObj.Strategy = Convert.ToString(reader["sStrategy"]);
                         operatorObj.KeyPeople = Convert.ToString(reader["sKeyPeople"]);
                         operatorObj.Commentary = Convert.ToString(reader["sCommentary"]);
                         operatorObj.ChiefExecutive = Convert.ToString(reader["sChiefExecutive"]);
                        operatorObjs.Add(operatorObj);
                    }
                }
            }
             return operatorObjs;
            
        }
    }
}
