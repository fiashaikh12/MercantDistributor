using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICommonRepository
    {
        List<States> GetStates();
        List<Cities> GetCitiesByState(States states);
        List<Genders> GetGenders();
    }
}
