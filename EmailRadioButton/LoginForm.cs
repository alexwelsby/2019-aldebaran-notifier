using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using S22.Imap;
using System.IO;
using System.Xml.Serialization;

namespace ADBNotifier
{

    public partial class LoginForm : Form
    {
        string email;
        string password;
        string howto;
        string server;

        [XmlRoot("SuperUser")]
        public class SuperUser
        {
            public User user
            {
                get;
                set;
            }
        }

        [XmlType("User")]
        public class User
        {
            [XmlElement("Email")]
            public string Email { get; set; }
            [XmlElement("Password")]
            public string Password { get; set; }
            [XmlElement("Server")]
            public string Server { get; set; }

        }

        public LoginForm()
        {
            InitializeComponent();
            DynamicResize(); //resizes the LoginForm to be slightly less god damn ugly
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoginAttempt();
        }

        private void LoginAttempt()
        {
            email = EmailInput.Text;
            password = passwordInput.Text;
            //sets strings email and password equal to the user's textbox input
            //and then checks the email for its domain 
            //and saves the corresponding imap server accordingly
            if (email.Contains("@gmail.com"))
            {
                server = "imap.gmail.com";
            }
            else if (email.Contains("@outlook.com") || email.Contains("@hotmail.com"))
            {
                server = "imap-mail.outlook.com";
            }
            else if (email.Contains("@yahoo.com"))
            {
                server = "imap.mail.yahoo.com";
            }
            else { };

            //it then goes to IMAPLogin, which attempts to use all three strings to log in
            IMAPLogin();
        }

        private void IMAPLogin()
        {
            loadingLabel.Visible = true;
            try
            {
                using (ImapClient Client = new ImapClient(server, 993,
                     email, password, AuthMethod.Login, true))
                {
                    loadingLabel.Text = "Loading...";
                    ExitLogin();
                    //if you succeed at logging in, you hit ExitLogin, 
                    //which then creates the xml and loads the next window
                }//IMAPClient disposes automatically
            }
            catch (InvalidCredentialsException)
            //if you don't succeed at logging in!
            {
                if (email.Contains("@gmail.com"))
                {
                    howto = "https://www.youtube.com/watch?v=UdMQ91W55to&feature=youtu.be";
                }
                else if (email.Contains("@outlook.com") || email.Contains("@hotmail.com"))
                {
                    howto = "https://www.youtube.com/watch?v=nyQnjWMfwsY&feature=youtu.be";
                }
                else if (email.Contains("@yahoo.com"))
                {
                    howto = "https://youtu.be/WaM_S35tElg";
                }
                loadingLabel.ForeColor = Color.Red;
                loadingLabel.Text = "Authentication failed. Please make sure you've enabled IMAP compatibility with your email. For information on how to do this, please check here!:";
                howToButton.Visible = true;
            }
            catch (ArgumentNullException)
            {
                loadingLabel.ForeColor = Color.Red;
                loadingLabel.Text = "You cannot enter an email without a valid domain (@gmail.com, @outlook.com, etc). Please try again.";
            }
        }//end of IMAPLogin

        private void ExitLogin()
        {
            BuildUserXML();
            //CreateShortcut(); //CreateShortcut makes it run on boot every time
            this.Hide();
            Settings settings = new Settings();
            settings.Show();
            //closes this window and shows the settings, instead
        }

        public void BuildUserXML()
        //creates a new LoginSettings.xml along with a new User object
        //it saves the strings email, password, and server to the user object
        //which it then saves to the larger object, SuperUser,
        //and SuperUser is then saved to the xml and the writer is closed
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SuperUser));
            using (TextWriter writer = new StreamWriter("LoginSettings.xml"))
            {
                SuperUser superuser = new SuperUser();
                User user = new User();
                user.Email = email;
                user.Password = password;
                user.Server = server;
                superuser.user = user;
                serializer.Serialize(writer, superuser);
                writer.Close();
            }
            //Encryption newEncrypt = new Encryption();
            //newEncrypt.OnLoad("LoginSettings.xml");
        }

        private void CreateShortcut()
        //adds ADBNotifier to the Startup folder, insuring it loads on computer boot
        {
            IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\ADBNotifier.exe.lnk") as IWshRuntimeLibrary.IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = Environment.CurrentDirectory + @"\ADBNotifier.exe";
            shortcut.WindowStyle = 1;
            shortcut.Description = "ADBNotifier";
            shortcut.WorkingDirectory = Environment.CurrentDirectory + @"\";
            //shortcut.IconLocation = "specify icon location";
            shortcut.Save();
        }

        private void DynamicResize()
        {
            label6.Width = this.Width;
            LabelLogin.Left = ((this.Width - LabelLogin.Width) / 2) - 13;
            EmailInput.Left = ((this.Width - EmailInput.Width) / 2) -13;
            passwordInput.Left = ((this.Width - passwordInput.Width) / 2) -13;
            LoginButton.Left = ((this.Width - LoginButton.Width) / 2) - 13;
            loadingLabel.Left = ((this.Width - loadingLabel.Width) / 2) - 13;
            howToButton.Left = ((this.Width - howToButton.Width) / 2) - 13;
            //just makes it look slightly better. still ugly. god i hate windows forms
        }

        private void HowToButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(howto);
            Process.Start(sInfo);
            //the string howto is set to a corresponding youtube tutorial
            //it then opens that youtube video when the button is clicked 
        }


        private void EmailInput_TextChanged(object sender, EventArgs e)
        {
            EmailInput.ForeColor = Color.Black;
        }

        private void PasswordInput_TextChanged(object sender, EventArgs e)
        {
            passwordInput.ForeColor = Color.Black;
        }

        private void EmailInput_Click(object sender, EventArgs e)
        {
            EmailInput.Clear();
        }

        private void PasswordInput_Click(object sender, EventArgs e)
        {
            passwordInput.Clear();
            passwordInput.ForeColor = Color.Black;
        }

        private void LoadingLabel_Click(object sender, EventArgs e)
        {

        }

        private void PasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginAttempt();
                //got tired of having to move my mouse.
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Process.GetCurrentProcess().Kill();
        }
    }
}
