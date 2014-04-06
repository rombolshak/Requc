using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;

namespace Requc.Models
{
    public class QuantumState : NotificationObject 
    {
        public QuantumState()
        {
            Id = Guid.NewGuid();
            Timeslot = new ObservableCollection<Complex>(new Complex[] {0, 0, 0});
            Timeslot.CollectionChanged += (sender, args) => RaisePropertyChanged(() => Timeslot);
        }

        public ObservableCollection<Complex> Timeslot { get; set; }

        public Guid Id { get; private set; }

        public static QuantumState Vacuum
        {
            get { return new QuantumState(); }
        }
    }
}
