using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FTTW
{
    /// <summary>
    /// Interaction logic for Authentication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        private MainWindow _main;
        private Config.ServerHistory _history;

        public List<string> History
        {
            get
            {
                return _history.History;
            }
        }

        public Authentication()
        {
            InitializeComponent();

            Config.Config config = Config.Config.getConfig();
            _history = Config.ServerHistory.get();
            cbbRedmineAddress.ItemsSource = _history.History;         
            this.DataContext = this;            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            authenticate();
        }
        public void Show(MainWindow main)
        {
            _main = main;
            this.Show();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                authenticate();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void authenticate()
        {
            if (cbbRedmineAddress.Text == string.Empty || txtPassword.Password == string.Empty || txtLogin.Text == string.Empty)
            {
                MessageBox.Show("Você não pode deixar campos em branco.");
                return;
            }
            
            _history.addWord(cbbRedmineAddress.Text);
            _main.closeLoginDialog(cbbRedmineAddress.Text, txtLogin.Text, txtPassword.Password, (ckbPersistLogin.IsChecked == true));
        }
    }
}
