using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllMyMusic.ViewModel
{
    public class ComputerTopViewModel : ViewModelBase
    {
        // This class has to be a collection
        // but in fact we only need one child

        private List<ComputerViewModel> computerNames;
        public List<ComputerViewModel> ComputerNames
        {
            get { return computerNames; }
        }

        public ComputerTopViewModel()
        {
            computerNames = new List<ComputerViewModel>();
            computerNames.Add(new ComputerViewModel("Computer"));
            computerNames[0].IsExpanded = true;

            RaisePropertyChanged("ComputerNames");
        }
    }
}
