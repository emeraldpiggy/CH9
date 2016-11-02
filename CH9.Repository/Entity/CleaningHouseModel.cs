using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CH9.Framework;

namespace CH9.Repository.Entity
{
    public class CleaningHouseModel : EntityBase
    {

        private bool _dusting;
        public bool Dusting
        {
            get { return _dusting; }
            set
            {
                SetPropertyInternal(ref _dusting, value);
            }
        }


        private bool _vacumming;

        public bool Vacuuming
        {
            get
            {
                return _vacumming;
            }
            set
            {
                SetPropertyInternal(ref _vacumming, value);
            }
        }

        private bool _mopping;

        public bool Mopping
        {
            get
            {
                return _mopping;
            }
            set

            {
                SetPropertyInternal(ref _mopping, value);
            }
        }


        private bool SetPropertyInternal<T>(ref T propertyBackingField, T value,
          [CallerMemberName] string propertyName = null)
        {
            if (propertyBackingField.AreEqual(value))
            {
                return false;
            }
            propertyBackingField = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        public CleaningHouseModel(bool dusting, bool vacumming, bool mopping)
        {
            _dusting = dusting;
            _vacumming = vacumming;
            _mopping = mopping;
        }
    }
}

