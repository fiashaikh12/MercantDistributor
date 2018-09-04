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
        ServiceRes<List<States>> GetStates();
        ServiceRes<List<Cities>> GetCitiesByState(States states);
        ServiceRes<List<Genders>> GetGenders();
    }
}
