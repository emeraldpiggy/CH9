using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH9.Repository.Entity
{
    public class CleaningHouseModel:EntityBase,IEntity
    {
        public string Dusting { get; set; }
        
        public string Vacuuming { get; set; }

        public string Mopping { get; set; }
    }
}

