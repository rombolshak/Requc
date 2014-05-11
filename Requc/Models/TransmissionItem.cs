using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;

namespace Requc.Models
{
    public class TransmissionItem : NotificationObject
    {
        private QuantumState _quantumState;
        private double _alicePhase;
        private double _bobPhase;
        private double _evaPhase;
        private double _phase0;
        private double _phase1;
        private bool _aliceValue;
        private bool _bobValue;
        private bool _evaValue;
        private bool _catchedByEva;
        private MeasurementResult _evaResult;

        public TransmissionItem(double phase0, double phase1)
        {
            Phase0 = phase0;
            Phase1 = phase1;
        }

        public QuantumState QuantumState
        {
            get { return _quantumState; }
            set
            {
                _quantumState = value;
                RaisePropertyChanged(() => QuantumState);
            }
        }

        public bool AliceValue
        {
            get { return _aliceValue; }
            set
            {
                _aliceValue = value;
                RaisePropertyChanged(() => AliceValue);
                AlicePhase = value ? Phase0 : Phase1;
            }
        }

        public bool BobValue
        {
            get { return _bobValue; }
            set
            {
                _bobValue = value;
                RaisePropertyChanged(() => BobValue);
                BobPhase = value ? Phase0 : Phase1;
            }
        }

        public bool EvaValue
        {
            get { return _evaValue; }
            set
            {
                _evaValue = value;
                RaisePropertyChanged(() => EvaValue);
                EvaPhase = value ? Phase0 : Phase1;
            }
        }

        public double AlicePhase
        {
            get { return _alicePhase; }
            set
            {
                _alicePhase = value;
                RaisePropertyChanged(() => AlicePhase);
            }
        }

        public double BobPhase
        {
            get { return _bobPhase; }
            set
            {
                _bobPhase = value;
                RaisePropertyChanged(() => BobPhase);
            }
        }

        public double EvaPhase
        {
            get { return _evaPhase; }
            set
            {
                _evaPhase = value;
                RaisePropertyChanged(() => EvaPhase);
            }
        }

        public double Phase0
        {
            get { return _phase0; }
            private set
            {
                _phase0 = value;
                RaisePropertyChanged(() => Phase0);
            }
        }

        public double Phase1
        {
            get { return _phase1; }
            private set
            {
                _phase1 = value;
                RaisePropertyChanged(() => Phase1);
            }
        }

        public bool CatchedByEva
        {
            get { return _catchedByEva; }
            set
            {
                _catchedByEva = value;
                RaisePropertyChanged(() => CatchedByEva);
            }
        }

        public MeasurementResult EvaResult
        {
            get { return _evaResult; }
            set
            {
                _evaResult = value;
                RaisePropertyChanged(() => EvaResult);
            }
        }

        public bool IsBlocked
        {
            get { return _evaResult == MeasurementResult.Inconclusive; }
        }
    }
}
