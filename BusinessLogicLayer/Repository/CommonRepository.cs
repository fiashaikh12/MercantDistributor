using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Data;

namespace Repository
{
    public class CommonRepository : ICommonRepository
    {
        public List<Cities> GetCitiesByState(States states)
        {
            List<Cities> cities = new List<Cities>();
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
                    cities.Add(city);
                }
            }
            catch(Exception ex) { throw ex; }
            return cities;
        }
        public List<Genders> GetGenders()
        {
            List<Genders> genders = new List<Genders>();
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
                    genders.Add(gender);
                }
            }
            catch (Exception ex) { throw ex; }
            return genders;
        }
        public List<States> GetStates()
        {
            List<States> states = new List<States>();
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
                    states.Add(state);
                }
            }
            catch (Exception ex) { throw ex; }
            return states;
        }
    }
}
