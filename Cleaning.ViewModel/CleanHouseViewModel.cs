using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH9.MVVM;

namespace Cleaning.ViewModel
{
    public class CleanHouseViewModel: ViewModelDisposableBase<CleanHouseModel>
    {
        public CleanHouseViewModel()
        {
            
        }


        #region Property 

        private string _dusting;

        public string Dusting
        {
            get { return _dusting;}
            set { }
        }



        #endregion
    }
}
