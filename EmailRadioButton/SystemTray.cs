using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using S22.Imap;
using System.Xml.Serialization;
using System.Net.Mail;
using System.Threading;
using System.Text.RegularExpressions;

namespace ADBNotifier
{
    public partial class SystemTray : Form
    {
        string email;
        string password;
        string server;
        public string URL { get; set; }
        bool newEmail;
        string CharImage;
        string CharMusic;

        //just declaring this here for ease of access
        //sometimes a man just gotta change some stuff.

        //string searchemail = 
        // "noreply@s2.jcink.com";
        //"alexwelsby13@gmail.com";
        //I've commented these out because the email it's going to search
        //is going to be whatever the debugger set their own email to (yours).

        //just instantiates a bunch of new character objects we're going to use 
        //and then adds them to a List
        Settings.Character defaultChar = new Settings.Character();
        Settings.Character character1 = new Settings.Character();
        Settings.Character character2 = new Settings.Character();
        Settings.Character character3 = new Settings.Character();
        Settings.Character character4 = new Settings.Character();
        Settings.Character character5 = new Settings.Character();
        List<Settings.Character> characterList = new List<Settings.Character>();

        public SystemTray()
        {
            InitializeComponent();

            if (File.Exists("LoginSettings.xml"))
            {
                fileExists(); //sends us to read LoginSettings and save the information to strings
            }
            else if (!File.Exists("LoginSettings.xml"))
            { ADBNotifier login = new ADBNotifier();
                login.Show(); //if LoginSettings.xml doesn't exist, go back to the Login screen
                fileNonExist(); // and have the systemtray process wait until LoginSettings.xml is made
            }

        }

        private void fileExists()
        {
            ReadUser(); //reads LoginSettings.xml into a new user object with email/password/server strings
            BGWorker.DoWork += new DoWorkEventHandler(BGWorker_DoWork); //runs a BGWorker that starts the main show
            BGWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGWorker_RunWorkerCompleted);
            BGWorker.RunWorkerAsync();
        }

        private void fileNonExist()
        {
            BGWorker2.DoWork += new DoWorkEventHandler(BGWorker2_DoWork); 
            BGWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGWorker2_RunWorkerCompleted);
            BGWorker2.RunWorkerAsync();
            //just repeatedly checks if LoginSettings.xml exists; if it doesn't, sends the thread to sleep
            //for another 600 ticks
        }

        private void BGWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!File.Exists("LoginSettings.xml"))
            {
                Thread.Sleep(600); //just sleep for as long as the file doesn't exist
            }
            fileExists(); //when the file does exist, ReadUser() and run the main BGWorker that logs in
        }

        private void BGWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BGWorker2.Dispose(); //just throw the whole thing in the trash
        }

        private void ReadUser() 
            //retrieves from LoginSettings.xml 
            //and then saves all information to strings 
            //which are used to log in via IMAP
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LoginForm.SuperUser));
            using (StreamReader sr = new StreamReader("LoginSettings.xml"))
            {
                LoginForm.SuperUser superuser = null;
                superuser = (LoginForm.SuperUser)serializer.Deserialize(sr);
                LoginForm.User user = superuser.user;
                email = user.Email;
                password = user.Password;
                server = user.Server;


                sr.Close();
            }
        }

        //this grabs the character information from the macros.xml file
        protected void ReadMacros()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings.Config));

            if (File.Exists("macros.xml"))
            { 
                using (StreamReader sr = new StreamReader("macros.xml"))
                {
                    //reads all the saved character keywords, retrieves them,
                    //and saves them to new objects which are placed in an array
                    Settings.Config config = null;
                    config = (Settings.Config)serializer.Deserialize(sr);

                    //messy, but it works. checked every time it checks for an email
                    //in case the user has changed the list since the program's initial boot
                    defaultChar.Name = null;
                    defaultChar.Music = config.ArrayOfCharacters.characterList[0].Music;
                    defaultChar.Image = config.ArrayOfCharacters.characterList[0].Image;

                    character1.Name = config.ArrayOfCharacters.characterList[1].Name;
                    character1.Music = config.ArrayOfCharacters.characterList[1].Music;
                    character1.Image = config.ArrayOfCharacters.characterList[1].Image;

                    character2.Name = config.ArrayOfCharacters.characterList[2].Name;
                    character2.Music = config.ArrayOfCharacters.characterList[2].Music;
                    character2.Image = config.ArrayOfCharacters.characterList[2].Image;

                    character3.Name = config.ArrayOfCharacters.characterList[3].Name;
                    character3.Music = config.ArrayOfCharacters.characterList[3].Music;
                    character3.Image = config.ArrayOfCharacters.characterList[3].Image;

                    character4.Name = config.ArrayOfCharacters.characterList[4].Name;
                    character4.Music = config.ArrayOfCharacters.characterList[4].Music;
                    character4.Image = config.ArrayOfCharacters.characterList[4].Image;

                    character5.Name = config.ArrayOfCharacters.characterList[5].Name;
                    character5.Music = config.ArrayOfCharacters.characterList[5].Music;
                    character5.Image = config.ArrayOfCharacters.characterList[5].Image;

                        characterList.Add(character1);
                        characterList.Add(character2);
                        characterList.Add(character3);
                        characterList.Add(character4);
                        characterList.Add(character5);

                    CharImage = defaultChar.Image;
                    CharMusic = defaultChar.Music;
                    //sets the CharImage and CharMusic to the default
                    //so CharImage and CharMusic aren't empty, if
                    //no keyword and associated character image/music is found

                    sr.Close();
                }
            }

        }

        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadMacros(); //checks if the macros.xml has changed and reupdates it if it has
            bool UrlGot = false;
            Thread.Sleep(5000);
            using (ImapClient Client = new ImapClient(server, 993,
                 email, password, AuthMethod.Login, true))
                {                                   
                    IEnumerable<uint> uids = Client.Search(SearchCondition.From(email).And(SearchCondition.Unseen()).And(SearchCondition.SentSince(DateTime.Today)));
                    //search for email from specific address, that is unseen, and from today (to avoid flooding)
                    //this is set to the user's regular email currently, since this is a debug version
                    if (uids.Count() > 0)
                    //if the number of emails fitting this criteria is greater than 0, then;
                    {
                        //get the first one and save the body to a string
                        MailMessage msg = Client.GetMessage(uids.First());
                        string body = msg.Body;

                    //we're checking the string body for specific phrases now
                    if (body.Contains("personal message"))
                    {
                        URL = "https://aldebaran.jcink.net/index.php?act=Msg&CODE=01";
                        newEmail = true;
                        return;
                    }

                    else if (body.Contains("quoted") || body.Contains("tagged") || body.Contains("reply"))
                    {
                        foreach (Match item in Regex.Matches(body, @"(http|ftp|https)(.*)\b(\w+)\s+(.*?)"))
                        //this regex checks for valid urls in the body of the email
                        {
                            if (UrlGot == false)
                            {
                                URL = item.Value;
                                Debug.WriteLine(URL);
                                UrlGot = true;
                                //if it hasn't gotten an URL,  and it finds a valid url, 
                                //it saves it and sets UrlGot to true
                            }
                            else
                            {
                                break;
                                //it got its first url.
                            }
                        }

                        for (int i = 0; i < characterList.Count; i++)
                        {
                            //still inside 'quoted, tagged, contained' else-if statement
                            //now that we've retrieved the body of the email, 
                            //and have determined it's from a specific user and has a unique url,
                            //we're going to iterate through the character objects
                            //to check for keyword matches against the email

                            foreach (Match item in Regex.Matches(body, characterList[i].Name, RegexOptions.IgnoreCase))
                            {
                                //grabs the saved keyword/'name' of a character
                                //then checks it against the body of the email
                                //if it finds a match, it sets CharMusic and CharImage 
                                //to that character's associated data
                                CharMusic = characterList[i].Music;
                                CharImage = characterList[i].Image;
                            }
                        } //end of for loop
                    } // end of "if new email contains quoted/tagged/replied" statement

                        //newEmail tells the worker, when completed,
                        //whether it should push a macro or not
                        //here it is true; it found a new email,
                        //and has successfully scraped an url
                        newEmail = true;
                        return;
                    } //end of 'if new email is found' statement
                    else
                    {
                        //newEmail is false; just do nothing and check again
                        newEmail = false;
                        return;
                    }
                } //end of using IMAPClient; it's disposed automatically
        } // end of BGWorker_DoWork

        private void BGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Update UI
            if (newEmail == true)
                //if true, create new macro and pass the current CharImage and CharMusic
                //go to macro's methods and run GetURL (opens a new tab to an url)
                //and show the macro's window (just an image the user has set)
            {
                Macro macro = new Macro(CharImage, CharMusic);
                Debug.WriteLine("Here is the URL: ", URL);
                macro.GetURL(URL); 
                macro.ShowDialog();
            }

            else  { } //if no newEmail, do nothing.

            BGWorker.RunWorkerAsync();   // This will make the BgWorker run again
        }
    }

}
