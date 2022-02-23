using ByteBank.Core.Model;
using ByteBank.Core.Repository;
using ByteBank.Core.Service;
using ByteBank.View.Utils;
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

        private async void BtnProcessar_Click(object sender, RoutedEventArgs e)
        {
            BtnProcessar.IsEnabled = false;

            var accounts = r_Repository.GetClientAccount();

            PgsProgress.Maximum = accounts.Count();

            ClearView();

            var start = DateTime.Now;

            var progress = new Progress<String>(str =>
            PgsProgress.Value++);
            //var byteBankProgress = new ByteBankProgress<String>(str =>
            //PgsProgress.Value++);

            var result = await ConsolidateAccounts(accounts, byteBankProgress);
            var end = DateTime.Now;
            UpdateView(result, end - start);
            BtnProcessar.IsEnabled = true;
        }

        private async Task<string[]> ConsolidateAccounts(IEnumerable<ClientAccount> accounts, IProgress<string> progressReporter)
        {
            var tasks = accounts.Select(account =>
                Task.Factory.StartNew(() =>
                {
                    var consolidateResult = r_Service.ConsolidateAccounts(account);

                    progressReporter.Report(consolidateResult);

                    return consolidateResult;
                })
            );

            return await Task.WhenAll(tasks);
        }

        private void ClearView()
        {
            LstResultados.ItemsSource = null;
            TxtTempo.Text = null;
        }

        private void UpdateView(IEnumerable<String> result, TimeSpan elapsedTime)
        {
            var tempoDecorrido = $"{ elapsedTime.Seconds }.{ elapsedTime.Milliseconds} segundos!";
            var message = $"Processamento de {result.Count()} clientes em {tempoDecorrido}";

            LstResultados.ItemsSource = result;
            TxtTempo.Text = message;
        }
    }
}
