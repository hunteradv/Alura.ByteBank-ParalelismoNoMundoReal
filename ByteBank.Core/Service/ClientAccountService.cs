using ByteBank.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByteBank.Core.Service
{
    public class ClientAccountService
    {
        public string ConsolidateAccounts(ClientAccount account)
        {
            return ConsolidateAccounts(account, CancellationToken.None);
        }

        public string ConsolidateAccounts(ClientAccount account, CancellationToken ct)
        {
            var sum = 0m;

            foreach (var movement in account.Movements)
            {
                ct.ThrowIfCancellationRequested();
                sum += movement.Value * MultiplicationFactor(movement.Date);
            }

            ct.ThrowIfCancellationRequested();
            UpdateInvestments(account);
            return $"Cliente {account.ClientName} tem saldo atualizado de R${sum.ToString("#00.00")}";
        }

        private static decimal MultiplicationFactor(DateTime movementDate)
        {
            const decimal CTE_FACTOR = 1.0000000005m;

            var calendarDaysFromTheDueDate = (movementDate - new DateTime(1900, 1, 1)).Days;
            var result = 1m;

            for (int i = 0; i < calendarDaysFromTheDueDate * 2; i++)
                result *= CTE_FACTOR;

            return result;
        }
        private static void UpdateInvestments(ClientAccount client)
        {
            const decimal CTE_BONIFICATION_MOV = 1m / (10m * 5m);
            client.Investment *= CTE_BONIFICATION_MOV;
        }
    }
}
