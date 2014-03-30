using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Requc.Helpers;

namespace Requc.Models
{
    public class TransmissionAct : IDisposable
    {
        public event EventHandler Completed = Actions.DoNothing;

        public TransmissionAct(TransmissionActScheme scheme)
        {
            _scheme = scheme;
        }

        public void Run()
        {
            var deviceColumns = _scheme.Columns;
            _processTop = new List<EventHandler>(deviceColumns.Count);
            _processBottom = new List<EventHandler>(deviceColumns.Count);
            for (int i = 0; i < deviceColumns.Count - 1; ++i)
            {
                var top = deviceColumns[i].Top;
                var bottom = deviceColumns[i].Bottom;
                var nextColumn = deviceColumns[i + 1];

                _processTop.Add((sender, args) => nextColumn.Top.Process());
                _processBottom.Add((sender, args) => nextColumn.Bottom.Process());

                top.ProcessFinished += _processTop[i];
                bottom.ProcessFinished += _processBottom[i];
            }

            var last = deviceColumns[deviceColumns.Count - 1];
            _processTop.Add((sender, args) =>
                {
                    _topCompleted = true;
                    CheckCompleted();
                });
            _processBottom.Add((sender, args) =>
                {
                    _bottomCompleted = true;
                    CheckCompleted();
                });

            last.Top.ProcessFinished += _processTop[deviceColumns.Count - 1];
            last.Bottom.ProcessFinished += _processBottom[deviceColumns.Count - 1];

            deviceColumns[0].Top.Process();
            deviceColumns[0].Bottom.Process();
        }

        private void CheckCompleted()
        {
            if (_topCompleted && _bottomCompleted)
            {
                Completed(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            var deviceColumns = _scheme.Columns;
            for (int i = 0; i < _scheme.Columns.Count; ++i)
            {
                var top = deviceColumns[i].Top;
                var bottom = deviceColumns[i].Bottom;

                top.ProcessFinished -= _processTop[i];
                bottom.ProcessFinished -= _processBottom[i];
            }
        }

        private IList<EventHandler> _processTop;
        private IList<EventHandler> _processBottom;
        private bool _topCompleted;
        private bool _bottomCompleted;

        private readonly TransmissionActScheme _scheme;
    }
}
