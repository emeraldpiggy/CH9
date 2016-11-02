﻿using System.Windows;
using System.Windows.Input;
using CH9.Repository.Entity;
using CH9.Shell.Base;
using Telerik.Windows.Controls;

namespace CH9.Shell.ViewModels
{
    public class CleaningHouseViewModel : ViewModelDisposableBase<CleaningHouseModel>
    {

        private ICommand _dCommand;
        private ICommand _vCommand;
        private ICommand _mCommand;

        public CleaningHouseViewModel()
        {
            Model = new CleaningHouseModel(false, false, false);

            _dCommand = new DelegateCommand(DustingAction, CanDustingAction);
            _vCommand = new DelegateCommand(VacummingAction, CanVacummingAction);
            _mCommand = new DelegateCommand(MoppingAction, CanMoppingAction);

        }

        private bool CanMoppingAction(object obj)
        {
            return !Mopping;
        }

        private bool CanVacummingAction(object obj)
        {
            return !Vacumming;
        }

        private bool CanDustingAction(object obj)
        {
            return !Dusting;
        }

        private void MoppingAction(object obj)
        {
            Mopping = true;
        }

        private void VacummingAction(object obj)
        {
            Vacumming = true;
        }

        private void DustingAction(object obj)
        {
            Dusting = true;
        }


        public ICommand DCommand => _dCommand;
        

        public ICommand VCommand => _vCommand;

        public ICommand MCommand => _mCommand;

        #region Property 

        public string DustingDisplay => "Dusting";

        public string VacummingDisplay => "Vacumming";

        public string MoppingDisplay => "Mopping";


        public bool Dusting
        {
            get { return Model.Dusting; }
            set
            {
                Model.Dusting = value;
                OnPropertyChanged(nameof(Dusting));
            }
        }

        public bool Vacumming
        {
            get { return Model.Vacumming; }
            set
            {
                Model.Vacumming = value;
                OnPropertyChanged(nameof(Vacumming));
            }
        }


        public bool Mopping
        {
            get { return Model.Mopping; }
            set
            {
                Model.Mopping = value;
                OnPropertyChanged(nameof(Mopping));

            }
        }



        #endregion
    }
}