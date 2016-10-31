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

        private string _dusting;
        public string Dusting
        {
            get { return _dusting; }
            set
            {
                SetPropertyInternal(ref _dusting, value);
            }
        }


        private string _vacumming;

        public string Vacuuming
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

        private string _mopping;

        public string Mopping
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

        public CleaningHouseModel(string dusting, string vacumming, string mopping)
        {
            _dusting = dusting;
            _vacumming = vacumming;
            this._mopping = mopping;
        }

    }
}

