using System;
using System.Collections;

namespace Microsoft.DotNet.Interactive.SandDance
{
    public class DataExplorer
    {
        private readonly Func<IEnumerable> _dataRowsGenerator;

        public DataExplorer(Func<IEnumerable> dataRowsGenerator)
        {
            _dataRowsGenerator = dataRowsGenerator ?? throw new ArgumentNullException(nameof(dataRowsGenerator));
        }

        public IEnumerable GetData()
        {
            return _dataRowsGenerator();
        }
    }
}
