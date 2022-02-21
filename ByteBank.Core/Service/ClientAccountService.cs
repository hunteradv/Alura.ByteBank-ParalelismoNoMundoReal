using ByteBank.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Core.Service
{
    public class ClientAccountService
    {
        public string ConsolidateAccounts(ClientAccount account)
        {
            var sum = 0m;

            foreach (var movement in account.Movements)
                sum += movement.Value * MultiplicationFactor(movement.Date);

            UpdateInvestments(account);
            return $"Cliente {account.ClientName} tem saldo atualizado de R${sum.ToString("#00.00")}";
        }

        private static decimal MultiplicationFactor(DateTime movementDate)
        {
            const decimal CTE_FACTOR = 1.0000000005m;

            var calendarDaysFromTheDueDate = (movementDate - new DateTime(1900, 1, 1)).Days;
            var result = 1m;

            for (int i = 0; i < calendarDaysFromTheDueDate * 2; i++)
                result = result * CTE_FACTOR;

            return result;
        }
        private static void UpdateInvestments(ClientAccount client)
        {
            const decimal CTE_BONIFICATION_MOV = 1m / (10m * 5m);
            client.Investment *= CTE_BONIFICATION_MOV;
        }
    }
}
