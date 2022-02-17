using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Core.Model
{
    public class ClientAccount
    {
        public string ClientName { get; set; }
        public List<Movement> Movements { get; set; }
        public decimal Investment { get; set; }
    }
}
