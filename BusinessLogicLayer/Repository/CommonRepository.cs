using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Data;
using BusinessLogicLayer.Repository;
using static Enum.Enums;

namespace Repository
{
    public class CommonRepository : ICommonRepository
    {
        public ServiceRes<List<Cities>> GetCitiesByState(States states)
        {
            ServiceRes<List<Cities>> cities = new ServiceRes<List<Cities>>();
            try {
                SqlParameter[] sqlParameter = new SqlParameter[1];
                sqlParameter[0] = new SqlParameter { ParameterName = "@StateId", Value = states.StateId };
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetCitiesByState", sqlParameter);
                foreach(DataRow row in dtCities.Rows)
                {
                    Cities city = new Cities {
                        CityId = Convert.ToInt32(row["CityId"]),
                        City=Convert.ToString(row["City"])
                    };
                    cities.Data.Add(city);
                }
            }
            catch(Exception ex) { throw ex; }
            return cities;
        }
        public ServiceRes<List<Genders>> GetGenders()
        {
            ServiceRes<List<Genders>> genders = new ServiceRes<List<Genders>>();
            try
            {
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetGender");
                foreach (DataRow row in dtCities.Rows)
                {
                    Genders gender = new Genders
                    {
                        GenderId = Convert.ToInt32(row["GenderId"]),
                        Gender = Convert.ToString(row["Gender"])
                    };
                    genders.Data.Add(gender);
                }
            }
            catch (Exception ex) { throw ex; }
            return genders;
        }
        public ServiceRes<List<States>> GetStates()
        {

            ServiceRes<List<States>> serviceRes = new ServiceRes<List<States>>();
            try
            {
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetStates");
                foreach (DataRow row in dtCities.Rows)
                {
                    States state = new States
                    {
                        StateId = Convert.ToInt32(row["StateId"]),
                        State = Convert.ToString(row["State"])
                    };
                    serviceRes.Data.Add(state);
                }
            }
            catch (Exception ex) { LogManager.WriteLog(ex, SeverityLevel.Critical); }
            return serviceRes;
        }
    }
}
