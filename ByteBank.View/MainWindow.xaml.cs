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

            var accountsQuantityPerThread = accounts.Count() / 8;

            var accountsPart1 = accounts.Take(accountsQuantityPerThread);
            var accountsPart2 = accounts.Skip(accountsQuantityPerThread).Take(accountsQuantityPerThread);
            var accountsPart3 = accounts.Skip(accountsQuantityPerThread * 2).Take(accountsQuantityPerThread);
            var accountsPart4 = accounts.Skip(accountsQuantityPerThread * 3).Take(accountsQuantityPerThread);
            var accountsPart5 = accounts.Skip(accountsQuantityPerThread * 4).Take(accountsQuantityPerThread);
            var accountsPart6 = accounts.Skip(accountsQuantityPerThread * 5).Take(accountsQuantityPerThread);
            var accountsPart7 = accounts.Skip(accountsQuantityPerThread * 6).Take(accountsQuantityPerThread);
            var accountsPart8 = accounts.Skip(accountsQuantityPerThread * 7);


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

            Thread threadPart3 = new Thread(() =>
            {
                foreach (var account in accountsPart3)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            Thread threadPart4 = new Thread(() =>
            {
                foreach (var account in accountsPart4)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            Thread threadPart5 = new Thread(() =>
            {
                foreach (var account in accountsPart5)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            Thread threadPart6 = new Thread(() =>
            {
                foreach (var account in accountsPart6)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            Thread threadPart7 = new Thread(() =>
            {
                foreach (var account in accountsPart7)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            Thread threadPart8 = new Thread(() =>
            {
                foreach (var account in accountsPart8)
                {
                    var resultProcess = r_Service.ConsolidarMovimentacao(account);
                    result.Add(resultProcess);
                }
            });

            threadPart1.Start();
            threadPart2.Start();
            threadPart3.Start();
            threadPart4.Start();
            threadPart5.Start();
            threadPart6.Start();
            threadPart7.Start();
            threadPart8.Start();

            while (threadPart1.IsAlive || threadPart2.IsAlive || threadPart3.IsAlive || threadPart4.IsAlive || threadPart5.IsAlive || threadPart6.IsAlive || threadPart7.IsAlive 
                || threadPart8.IsAlive)
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
