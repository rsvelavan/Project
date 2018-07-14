using CrossSolar.Domain;
using System.Collections.Generic;

namespace CrossSolar.Models
{
    public class OneDayElectricityListModel
    {
        public IEnumerable<OneDayElectricityModel> OneDayElectricity { get; set; }
    }
}
