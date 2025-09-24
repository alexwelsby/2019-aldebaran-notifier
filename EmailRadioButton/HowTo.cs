using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ADBNotifier
{
    public partial class ADBNotifier : Form
    {
        string howTo;
        public ADBNotifier()
        {
            InitializeComponent();
            DynamicResize();
        }

        private void DynamicResize()
        {
            label3.Left = (panel1.Width - label3.Width) / 2;
            label2.Left = (panel2.Width - label2.Width) / 2;
            howToButton.Left = (panel3.Width - howToButton.Width) / 2;
            //just makes it slightly less ugly. graphic design is not my passion
        }


        private void LetMeLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
            //just takes you to the Login screen
        }

        private void HowToButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (gmailButton.Checked == true)
                { howTo = "https://www.youtube.com/watch?v=UdMQ91W55to&feature=youtu.be"; }
                else if (yahooButton.Checked == true)
                { howTo = "https://youtu.be/WaM_S35tElg"; }
                else if (outlookButton.Checked == true)
                { howTo = "https://www.youtube.com/watch?v=nyQnjWMfwsY&feature=youtu.be"; }
                else { }
                ProcessStartInfo sInfo = new ProcessStartInfo(howTo);
                Process.Start(sInfo);
                //takes you to various youtube videos depending on what radio button you clicked
            }
            catch { howToButton.Text = "No button selected."; }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            Process.GetCurrentProcess().Kill();
        }

    }
}
