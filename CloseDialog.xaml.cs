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
    /// Interaction logic for CloseDialog.xaml
    /// </summary>
    public partial class CloseDialog : Window
    {
        private int _answer;
        public enum Answers {Yes, No, Cancel};

        public Answers Answer
        {
            get
            {
                return (Answers) _answer;
            }
        }

        public CloseDialog()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            _answer = (int)Answers.Yes;
            if (ckbRemember.IsChecked == true)
            {
                Config.Config config = Config.Config.getConfig();
                config.CloseAction = 1;
                config.save();
            }
            this.Hide();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            _answer = (int)Answers.No;
            if (ckbRemember.IsChecked == true)
            {
                Config.Config config = Config.Config.getConfig();
                config.CloseAction = 2;
                config.save();
            }
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _answer = (int)Answers.Cancel;
            this.Hide();
        }
    }
}
