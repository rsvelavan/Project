using System;

namespace CrossSolar.Models
{
    public class OneHourElectricityModel
    {
        public int Id { get; set; }

        public long KiloWatt { get; set; }

        public DateTime DateTime { get; set; }

        public string PanelId { get; set; }
    }
    public class OneHourElectricityInsert
    {
        public long KiloWatt { get; set; }

        public string PanelId { get; set; }
    }
}