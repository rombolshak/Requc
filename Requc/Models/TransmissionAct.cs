using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Requc.Models
{
    public class TransmissionAct
    {
        public TransmissionAct(TransmissionActScheme scheme)
        {
            _scheme = scheme;
        }

        public void Run()
        {
            var waiterTop = new AutoResetEvent(false);
            var waiterBottom = new AutoResetEvent(false);

            var deviceColumns = _scheme.Columns;
            for (int i = 0; i < _scheme.Columns.Count - 1; ++i)
            {
                var top = deviceColumns[i].Top;
                var bottom = deviceColumns[i].Bottom;
                var nextColumn = deviceColumns[i + 1];
                
                top.ProcessFinished += (sender, args) => nextColumn.Top.Process();
                bottom.ProcessFinished += (sender, args) => nextColumn.Bottom.Process();
            }

            var last = deviceColumns[deviceColumns.Count - 1];
            last.Top.ProcessFinished += (sender, args) => waiterTop.Set();
            last.Bottom.ProcessFinished += (sender, args) => waiterBottom.Set();

            Parallel.Invoke(
                () => deviceColumns[0].Top.Process(), 
                () => deviceColumns[0].Bottom.Process());
            waiterTop.WaitOne();
            waiterBottom.WaitOne();
        }

        private readonly TransmissionActScheme _scheme;
    }
}
