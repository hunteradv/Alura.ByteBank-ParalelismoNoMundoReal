using ByteBank.Core.Model;
using ByteBank.Core.Repository;
using ByteBank.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ByteBank.View
{
    public partial class MainWindow : Window
    {
        private readonly ClientAccountRepository r_Repository;
        private readonly ClientAccountService r_Service;

        public MainWindow()
        {
            InitializeComponent();

            r_Repository = new ClientAccountRepository();
            r_Service = new ClientAccountService();
        }

        private void BtnProcessar_Click(object sender, RoutedEventArgs e)
        {
            var accounts = r_Repository.GetClientAccount();

            var accountsPart1 = accounts.Take(accounts.Count() / 2);
            var accountsPart2 = accounts.Skip(accounts.Count() / 2);

            var result = new List<string>();

            UpdateView(new List<string>(), TimeSpan.Zero);

            var start = DateTime.Now;

            Thread threadPart1 = new Thread(() =>
            {
                foreach (var account in accountsPart1)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            Thread threadPart2 = new Thread(() =>
            {
                foreach (var account in accountsPart2)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            threadPart1.Start();
            threadPart2.Start();

            while(threadPart1.IsAlive || threadPart2.IsAlive)
            {
                //verifica a todo momento se a propriedade IsAlive retorna true, quando restornar false termina a execução das duas threads
            }

            var end = DateTime.Now;

            UpdateView(result, end - start);
        }

        private void UpdateView(List<String> result, TimeSpan elapsedTime)
        {
            var tempoDecorrido = $"{ elapsedTime.Seconds }.{ elapsedTime.Milliseconds} segundos!";
            var message = $"Processamento de {result.Count} clientes em {tempoDecorrido}";

            LstResultados.ItemsSource = result;
            TxtTempo.Text = message;
        }
    }
}
