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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private Config.Config _config;
        public Settings()
        {
            InitializeComponent();
            _config = Config.Config.getConfig();
            cbbCloseAction.SelectedIndex = _config.CloseAction;
            txtActivityId.Text = _config.ActivityId.ToString();
            txtIdleSecondsBeforeStopTracking.Text = _config.IdleSecondsBeforeStopTracking.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _config.ActivityId = UInt32.Parse(txtActivityId.Text);
            _config.IdleSecondsBeforeStopTracking = UInt32.Parse(txtIdleSecondsBeforeStopTracking.Text);
            _config.CloseAction = cbbCloseAction.SelectedIndex;
            _config.save();
            this.Close();
        }    
    }
}
