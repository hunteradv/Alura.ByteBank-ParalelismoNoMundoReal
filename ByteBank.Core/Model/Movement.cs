using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Core.Model
{
    public enum MovementType
    {
        Withdraw,
        Deposit,
        Transfer,
        PhoneRecharge,
        DebitPayment
    }

    public class Movement
    {
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public MovementType Type { get; set; }
    }
}
