using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ExperimentResult
    {
        public int Id { get; set; }
        public int ExperimentId { get; set; }
        public string DeviceToken { get; set; }
        public string Value { get; set; }
    }
}
