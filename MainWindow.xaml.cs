using FTTW.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

namespace FTTW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private Authentication _auth;
        
        private bool _isStarted;

        private Front.FIssue _current;
        private uint _idleTime;
        private double _widthBeforeMinimizing;
        private System.Windows.Forms.ContextMenu _contextMenu;

        private List<Config.Server> _servers;
        private int _serverIndex;

        private System.Windows.Forms.MenuItem _stopTracking;

        private bool _hardShutdown;

        public MainWindow()
        {
            _hardShutdown = false;
            _widthBeforeMinimizing = 0;
            _idleTime = 0;
            InitializeComponent();
            _isStarted = false;
            _servers = new List<Config.Server>();
            _auth = new Authentication();
            _serverIndex = 0;

            List<Config.Auth> auths = Config.Auth.getList();
            if (auths != null)
            {
                foreach (Config.Auth auth in auths)
                {
                    try {
                        _servers.Add(new Config.Server(auth));
                    } catch (RedmineServerInaccessibleException ex)
                    {

                    }
                }
            }
            if (_servers.Count == 0)
                openLoginDialog(this);

            cbServers.ItemsSource = _servers;
            cbServers.SelectedIndex = 0;

            startPage();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_hardShutdown)
            {
                Config.Config config = Config.Config.getConfig();
                if (config.CloseAction == 0)
                {                    
                    CloseDialog cdialog = new CloseDialog();
                    cdialog.ShowDialog();
                    var result = cdialog.Answer;
                    if (result == CloseDialog.Answers.Yes)
                    {
                        _notifyIcon.Visible = true;
                        e.Cancel = true;
                        this.Hide();
                    }
                    else if (result == CloseDialog.Answers.Cancel)
                    {
                        e.Cancel = true;
                    }
                } else if (config.CloseAction == 1)
                {
                    _notifyIcon.Visible = true;
                    e.Cancel = true;
                    this.Hide();
                }
            }
            base.OnClosing(e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
            _notifyIcon.Click += _notifyIcon_Click;
            _notifyIcon.Text = "Apontador de horas trabalhadas";
            _notifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/icons/clock.ico")).Stream);

            _contextMenu = new System.Windows.Forms.ContextMenu();

            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Abrir", new EventHandler(menuItemOpen_Click));
            System.Windows.Forms.MenuItem settings = new System.Windows.Forms.MenuItem("Configurações", new EventHandler(menuItemSettings_Click));
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Sair", new EventHandler(menuItemExit_Click));
            _stopTracking = new System.Windows.Forms.MenuItem("Parar", new EventHandler(menuItemStopTracking_Click));            
            _stopTracking.Enabled = false;

            _contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { open, _stopTracking, settings, exit });            
            _notifyIcon.ContextMenu = _contextMenu;
        }

        private void _notifyIcon_Click(object sender, EventArgs e)
        {
            
        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            this.Show();
            _notifyIcon.Visible = false;
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            _hardShutdown = true;
            Application.Current.Shutdown();
        }

        private void menuItemStopTracking_Click(object sender, EventArgs e)
        {
            stopTracking();
        }

        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            _notifyIcon.Visible = false;
        }

        /// <summary>
        /// Open a dialog on which the user can input his redmine login and password
        /// </summary>
        /// <param name="mw">Main window object to which the focus should return after validating the credentials.</param>
        private void openLoginDialog(MainWindow mw)
        {
            _auth.Show(mw);
            this.Hide();
        }

        /// <summary>
        /// To be called when the login dialog is closed after inputing login and password
        /// </summary>
        /// <param name="login">Login put by user</param>
        /// <param name="password">Password put by user</param>
        public void closeLoginDialog(string address, string login, string password, bool persist)
        {
            //Do something with credentials
            try
            {
                Config.Config config = Config.Config.getConfig();
                Config.Server server = new Config.Server(address, login, password, persist);
                _servers.Add(server);
                cbServers.ItemsSource = null;
                cbServers.ItemsSource = _servers;            
                cbServers.SelectedIndex = _servers.Count-1;
            } catch (FileNotFoundException ex)
            {
                MessageBox.Show("Há arquivos necessários faltando.", "Erro"); 
                openLoginDialog(this);
                if (_servers.Count == 0)
                    return;
            } catch (WebException ex)
            {                
                MessageBox.Show("Falha: " + ex.Message);
                openLoginDialog(this);
                if (_servers.Count == 0)
                    return;
            } catch (Exceptions.InvalidLoginPasswordCombinationException ex)
            {
                MessageBox.Show(ex.Message);
                openLoginDialog(this);
                if (_servers.Count == 0)
                    return;
            } catch (Exceptions.RedmineServerInaccessibleException ex)
            {
                MessageBox.Show(ex.Message);
                if (_servers.Count == 0)
                    return;
            }
            //Shake me down. Not a lot of people left around.            
            this.Show();
            _auth.Hide();
            reloadPage();
        }

        /// <summary>
        /// Loads projects and issues. Starts timing thread.
        /// </summary>       
        private void startPage()
        {
            if (_servers.Count > 0)
            {
                _serverIndex = 0;

                cbProjects.ItemsSource = _servers[_serverIndex].Projects;
                cbProjects.SelectedIndex = 0;

                dgLastWeek.ItemsSource = _servers[_serverIndex].Ds;
            }
            System.Threading.Thread t = new System.Threading.Thread(updateTimer) { IsBackground = true};
            t.Start();            
        }

        private void reloadPage()
        {
            if (_servers.Count == 0)
                return;
            _serverIndex = Math.Max(0, _serverIndex);
            cbProjects.ItemsSource = _servers[_serverIndex].Projects;
            dgLastWeek.ItemsSource = _servers[_serverIndex].Ds;
            txtIssueFilter.Text = string.Empty;
            cbProjects.SelectedIndex = 0;
            dgTasks.ItemsSource = _servers[_serverIndex].Projects[cbProjects.SelectedIndex].getWithFilter(txtIssueFilter.Text);
            dgHistory.ItemsSource = null;
            dgHistory.ItemsSource = _servers[_serverIndex].History;
        }

        /// <summary>
        /// Starts tracking work on an issue.
        /// </summary>
        /// <param name="issue">Issue on which the user is working.</param>
        private void startTracking(Front.FIssue issue)
        {            
            issue.startWork();
            _servers[_serverIndex].Ds[0].startWatch();
            _current = issue;
            issue.IsSelected = false;
            btnStartStop.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F4ABBA"));
            btnStartStop.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E6808A"));
            lblStartStop.Text = "Parar";
            lblWorkingOnIssue.Text = issue.Id.ToString() + " - " + issue.Name.Substring(0, Math.Min(20, issue.Name.Length)) + (issue.Name.Length >= 20 ? "...":string.Empty);           
            _servers[_serverIndex].CurrentProjetI = cbProjects.SelectedIndex;
            _stopTracking.Enabled = true;

            dgHistory.Items.Refresh();
        }

        /// <summary>
        /// Stops tracking user's work.
        /// </summary>
        private void stopTracking()
        {           
            _current.endWork(_servers[_serverIndex].Getter);
            _servers[_serverIndex].Ds[0].stopWatch();
            _current.IsSelected = false;
            if (dgTasks.SelectedIndex != -1)
                ((Front.FIssue)dgTasks.SelectedItem).IsSelected = false;
            btnStartStop.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#AAE5AA"));
            btnStartStop.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#79DB75"));
            lblStartStop.Text = "Iniciar";
            
            Front.History.addHistory(_current, _servers[_serverIndex].History);
            dgHistory.ItemsSource = null;
            dgHistory.ItemsSource = _servers[_serverIndex].History;
            


            dgHistory.HeadersVisibility = DataGridHeadersVisibility.Column;
            lblWorkingOnIssue.Text = string.Empty;
            _stopTracking.Enabled = false;
        }

        /// <summary>
        /// Updates all timers, including main timer, last week timer, datagrid timer and idle time.
        /// </summary>
        private void updateTimer()
        {
            uint idleSecond;
            while(true)
            {
                System.Threading.Thread.Sleep(1000);                               
                if (_isStarted)
                {
                    lblTime.Dispatcher.BeginInvoke((Action)(() => lblTime.Text = _servers[_serverIndex].Ds[0].Time));
                    dgTasks.Dispatcher.BeginInvoke((Action)(() => dgTasks.Items.Refresh()));
                    dgLastWeek.Dispatcher.BeginInvoke((Action)(() => dgLastWeek.Items.Refresh()));
                    idleSecond = WorkPerfomance.Monitor.GetIdleTime();
                    idleSecond -= _idleTime;
                    if (idleSecond == 1000)
                        _current.increaseIdleTime();
                    _idleTime = WorkPerfomance.Monitor.GetIdleTime();

                    Config.Config config = Config.Config.getConfig();

                    if (config.IdleSecondsBeforeStopTracking > 0 && _current.getIdleTime() >= config.IdleSecondsBeforeStopTracking)
                    {
                        _isStarted = false;
                        dgTasks.Dispatcher.BeginInvoke((Action)(() => stopTracking())); 
                        MessageBox.Show("Você está inativo há muito tempo. Paramos o tempo para você.");
                    }
                }
            }
        }

        /// <summary>
        /// Changed selected project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtIssueFilter.Text = string.Empty;
            if (cbProjects.SelectedIndex >= 0)
                dgTasks.ItemsSource = _servers[_serverIndex].Projects[cbProjects.SelectedIndex].getWithFilter(txtIssueFilter.Text);
            else
                dgTasks.ItemsSource = null;
        }

        /// <summary>
        /// The button used to start/stop traking an issue was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if ((!_isStarted) && (cbProjects.SelectedIndex == -1 || dgTasks.SelectedIndex == -1))
            {
                MessageBox.Show("Erro: você deve selecionar a terefa antes de iniciar o trabalho.", "Erro");
                return;
            }
            if (_isStarted)
                stopTracking();
            else
                startTracking((Front.FIssue)dgTasks.SelectedItem);
            dgTasks.Items.Refresh();
            dgTasks.UnselectAll();
            _isStarted = !_isStarted;
        }

        /// <summary>
        /// The button used to start/stop tracking an issue, located on the datagrid, was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Front.IStartEnd startEnd = ((FrameworkElement)sender).DataContext as Front.IStartEnd;
            Front.FIssue issue = startEnd.getIssue();

            if (_isStarted)
            {
                stopTracking();
                _isStarted = false;
                if (_current.Id != issue.Id)
                {
                    startTracking(issue);
                    _isStarted = true;
                }
            } else
            {
                startTracking(issue);
                _isStarted = true;
            }
            dgTasks.Items.Refresh();
            dgTasks.UnselectAll();
        }


        private void dgTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Front.FIssue)dgTasks.SelectedItem == null)
                return;
            ((Front.FIssue)dgTasks.SelectedItem).IsSelected = true;
            if (e.RemovedItems != null && e.RemovedItems.Count > 0)
                ((Front.FIssue)e.RemovedItems[0]).IsSelected = false;
            dgTasks.Items.Refresh();
        }

        /// <summary>
        /// Logout.
        /// Todo: do action after deleting the authentication file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            /*Config.Auth auth = Config.Auth.get();
            auth.Token = string.Empty;
            _hardShutdown = true;
            Application.Current.Shutdown() ; */
            //Todo: finish that button's funcionality
            //openLoginDialog(new MainWindow());  

            
            _servers[_serverIndex].forgetThisServer();
            _servers.RemoveAt(_serverIndex);

            if (_servers.Count == 0)
            {
                openLoginDialog(this);
            } else
            {
                _serverIndex = Math.Max(0, _serverIndex - 1);
            }
            cbServers.ItemsSource = null;
            cbServers.ItemsSource = _servers;
            cbServers.SelectedIndex = _serverIndex;

            startPage();

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void txtIssueFilter_KeyUp(object sender, KeyEventArgs e)
        {
            dgTasks.ItemsSource =_servers[_serverIndex].Projects[cbProjects.SelectedIndex].getWithFilter(txtIssueFilter.Text);            
        }

        private void btnMinimizeFromFull_Click(object sender, RoutedEventArgs e)
        {
            if (_widthBeforeMinimizing == 0)
            {
                _widthBeforeMinimizing = Application.Current.MainWindow.Width;
                Application.Current.MainWindow.MinWidth = 255;
                Application.Current.MainWindow.Width = 255;
                Application.Current.MainWindow.MaxWidth = 255;
                imgMinimize.Source = new BitmapImage(new Uri(@"icons/arrow_right.png", UriKind.Relative));
            } else
            {                
                Application.Current.MainWindow.MaxWidth = 3840;
                Application.Current.MainWindow.MinWidth = 680;
                Application.Current.MainWindow.Width = _widthBeforeMinimizing;
                _widthBeforeMinimizing = 0;
                imgMinimize.Source = new BitmapImage(new Uri(@"icons/arrow_left.png", UriKind.Relative));
            }
        }

        private void dgHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnWeb_Click(object sender, RoutedEventArgs e)
        {
            Front.FIssue issue = ((FrameworkElement)sender).DataContext as Front.FIssue;
            string link = _servers[_serverIndex].Address;
            StringBuilder sbuilder = new StringBuilder(link);
            if (sbuilder[sbuilder.Length - 1] != '/')
                sbuilder.Append("/");
            link = sbuilder.ToString() + "issues/" + issue.Id.ToString();
            System.Diagnostics.Process.Start(link);

        }

        private void btnAddServer_Click(object sender, RoutedEventArgs e)
        {
            if (_isStarted) stopTracking();
            _auth.Show(this);
        }

        private void cbServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isStarted) stopTracking();
            _serverIndex = cbServers.SelectedIndex;
            reloadPage();
        }
    }
}