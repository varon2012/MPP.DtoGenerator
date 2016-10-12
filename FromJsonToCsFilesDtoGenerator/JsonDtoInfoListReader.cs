using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator;

namespace FromJsonToCsFilesDtoGenerator
{
    public class JsonDtoInfoListReader : IDtoInfoListReader
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event Action<DtoInfo> OnDtoInfoRead;
        public event Action OnReadCompleted;

        public void ReadList()
        {

        }
    }
}
