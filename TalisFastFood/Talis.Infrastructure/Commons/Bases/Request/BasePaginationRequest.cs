using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talis.Infrastructure.Commons.Bases.Request
{
    public class BasePaginationRequest
    {
        public int NumPage { get; set; } = 1;
        public int NumRecordsPage { get; set; } = 10;
        public int NumMaxRecords { get; set; } = 50;
        public int NumMaxPages => (int)Math.Ceiling((double)NumMaxRecords / NumRecordsPage);
        public string BaseURL { get; set; }

        public int Records
        {
            get
            {
                return NumRecordsPage;
            }
            set
            {
                NumRecordsPage = (value > NumMaxRecords) ? NumMaxRecords : value;
            }
        }
        public int RecordsToSkip => NumRecordsPage * (NumPage - 1);
    }
}
