using CH9.Repository.Entity;
using CH9.Shell.Base;

namespace CH9.Shell.ViewModels
{
    public class CleaningHouseViewModel : ViewModelDisposableBase<CleaningHouseModel>
    {
        public CleaningHouseViewModel()
        {

        }


        #region Property 

        public string DustingDisplay => "Dusting";

        public string VacummingDisplay => "Vacumming";

        public string MoppingDisplay => "Mopping";


        public string Dusting
        {
            get { return Model.Dusting; }
            set { Model.Dusting = value; }
        }

        public string Vacuuming
        {
            get { return Model.Vacuuming; }
            set { Model.Vacuuming = value; }
        }


        public string Mopping
        {
            get { return Model.Mopping; }
            set { Model.Mopping = value; }
        }



        #endregion
    }
}
