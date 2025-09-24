using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ADBNotifier
{

    public partial class Settings : Form
    {

        [XmlRoot("Config")]
        public class Config
        {

            [XmlElement("OfficialArray")]
            public ArrayOfCharacters ArrayOfCharacters
            {
                get;
                set;
            }

        }

        [XmlType("Character")]
        public class Character
        {
            [XmlElement("Name")]
            public string Name { get; set; }
            [XmlElement("Music")]
            public string Music { get; set; }
            [XmlElement("Image")]
            public string Image { get; set; }
        }

        [XmlType("ArrayOfCharacters")]
        public class ArrayOfCharacters
        {
            [XmlArray("array_name")]
            [XmlArrayItem("Item_in_array")]
            public List<Character> characterList { get; set; }

            public ArrayOfCharacters()
            {
                characterList = new List<Character>();
            }
        }

        //just instantiate all the characters you need. theres a set number
        LoginForm.User user = new LoginForm.User();
        Character defaultChar = new Character();
        Character Character1 = new Character();
        Character Character2 = new Character();
        Character Character3 = new Character();
        Character Character4 = new Character();
        Character Character5 = new Character();
        string curd = Environment.CurrentDirectory; //string of the current directory of the .exe


        public Settings()
        {
            InitializeComponent();

            ResizeControls();

            CheckDirectory(); //checks if macros/images and macros/music folders exist, then makes them

            if (File.Exists("macros.xml")) //if macros.xml exists, retrieve all info from it
            {                                                                 //and populate the form with it
                ReadXML();
            }
            else //if macro settings dont exist yet, set default macro for the user and display it in the UI
            {
                defaultChar.Music = (curd + @"\Macros\Music\DefaultMusic.mp3"); //assign current directory + child directory file to music prop
                defaultChar.Image = (curd + @"\Macros\Images\DefaultImage.jpg"); //assign current directory + child directory file to image prop
                defaultImage.Text = Path.GetFileName(defaultChar.Image);
                defaultMusic.Text = Path.GetFileName(defaultChar.Music);
                CreateXML("macros.xml");
            }

            ReadLoginXML();
            exposeEmail.Text = user.Email;
            exposePassword.Text = "*********";
            exposeServer.Text = user.Server;
        }

        private void CheckDirectory()
        {
            if (!Directory.Exists(curd + @"\Macros\Images\"))
            { DirectoryInfo di = Directory.CreateDirectory(curd + @"\Macros\Images\"); }
            else if (!Directory.Exists(curd + @"\Macros\Music\"))
            { DirectoryInfo di = Directory.CreateDirectory(curd + @"\Macros\Music\"); }
            else { }
            //creates new folders (Macros\Images and Macros\Music) if they do not currently exist
        }

        private void ResizeControls()
            //i hate graphic design.
            //this has to exist so it fits on screens regardless of resolution
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Height = 1066;

            if (screen.Height < this.Height)
            {
                this.Height = (int)(.9 * screen.Height);
                this.AutoScroll = true;
            }
            else
            {
                this.Height = 1066;
                this.Width = 605;
            }
            label6.Width = tabControl1.Width;
            label7.Width = tabControl1.Width;
            aldebaranBanner.Left = (this.ClientSize.Width - aldebaranBanner.Width) / 2;
            pictureBox1.Left = (panel1.Width - pictureBox1.Width) / 2;
            aboutTitleLabel.Left = (panel1.Width - aboutTitleLabel.Width) / 2;
            Locationlabel.Left = (panel1.Width - Locationlabel.Width) / 2;
            pronounsLabel.Left = (panel1.Width - pronounsLabel.Width) / 2;
            label13.Left = (panel1.Width - label13.Width) / 2;
            NameButton.Left = (panel1.Width - NameButton.Width) / 2;
            label14.Left = (panel1.Width - label14.Width) / 2;
            label11.Left = (panel1.Width - label11.Width) / 2;
            label15.Left = (panel1.Width - label15.Width) / 2;
            label16.Left = (panel1.Width - label16.Width) / 2;
            //oh i hate this.
        }



        //figure out what to do with the custom character macros? oh god this will be so much redundant code
        //new generic method that all test buttons take that takes the overhead of Character.music, character.image?
        private void SaveButton_Click(object sender, EventArgs e)
        {
            CreateXML("macros.xml"); //gathers up all the user-input character data and saves it to an xml tree
            SaveButton.Text = "Saved!";

            Application.DoEvents(); // This will process all UI events currently in message queue

            System.Threading.Thread.Sleep(1000);
            SaveButton.Text = "Save";
        }

        private void CreateXML(string filename)
        {
            //when the Save button is pressed, creates a new xml with a passed name (macros.xml)
            //saves all text fields to the corresponding Character.Name property,
            //adds each character one-by-one to the array (has to be a better way?!)
            //creates a new config object, which then contains the array,
            //and then the serializer uses the writer to serialize the config object
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (TextWriter writer = new StreamWriter(filename))
            {
                Character1.Name = Character1Name.Text;
                Character2.Name = Character2Name.Text;
                Character3.Name = CharacterName3.Text;
                Character4.Name = CharacterName4.Text;
                Character5.Name = CharacterName5.Text;
                ArrayOfCharacters adb = new ArrayOfCharacters();
                adb.characterList.Add(defaultChar);
                adb.characterList.Add(Character1);
                adb.characterList.Add(Character2);
                adb.characterList.Add(Character3);
                adb.characterList.Add(Character4);
                adb.characterList.Add(Character5);
                Config config = new Config();
                config.ArrayOfCharacters = adb;
                serializer.Serialize(writer, config);
                writer.Close();
            }
        }

        protected void ReadXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings.Config));
            using (StreamReader sr = new StreamReader("macros.xml"))
            //grabbing from the macros.xml and placing the retrieved information into character objects created on load
            {
                Settings.Config config = null;
                config = (Settings.Config)serializer.Deserialize(sr);

                defaultChar.Music = config.ArrayOfCharacters.characterList[0].Music;
                defaultChar.Image = config.ArrayOfCharacters.characterList[0].Image;
                defaultImage.Text = Path.GetFileName(defaultChar.Image);
                defaultMusic.Text = Path.GetFileName(defaultChar.Music);
                    //method takes previously instantiated character, int for array, the arrray
                    //the associated checkbox, associated groupbox, associated name textbox, associated image textbox, associated music textbox
                    //this is a monster. i'm coping
                    setNameMusic(Character1, 1, config, config.ArrayOfCharacters, groupBox1, Character1Name, Char1Image, Char1Music);
                    setNameMusic(Character2, 2, config, config.ArrayOfCharacters, groupBox2, Character2Name, Char2Image, Char2Music);
                    setNameMusic(Character3, 3, config, config.ArrayOfCharacters, groupBox3, CharacterName3, Char3Image, Char3Music);
                    setNameMusic(Character4, 4, config, config.ArrayOfCharacters, groupBox4, CharacterName4, Char4Image, Char4Music);
                    setNameMusic(Character5, 5, config, config.ArrayOfCharacters, groupBox5, CharacterName5, Char5Image, Char5Music);
                sr.Close();
            }
        }



        private void ReadLoginXML()
            //here's the problem. Reads saved LoginSettings.xml and retrieves plain-text login information
        {
            XmlSerializer serializerLogin = new XmlSerializer(typeof(LoginForm.SuperUser));
            using (StreamReader sr = new StreamReader("LoginSettings.xml"))
            {
                LoginForm.SuperUser Userconfig = null;
                Userconfig = (LoginForm.SuperUser)serializerLogin.Deserialize(sr);
                user.Email = Userconfig.user.Email;
                user.Password = Userconfig.user.Password;
                user.Server = Userconfig.user.Server;
                //it does this so the user can see their email in the Email Settings tab and choose to log out
                sr.Close();
            }
        }

        private void setNameMusic(Character character, int i, Config config, ArrayOfCharacters arrayofcharacters, GroupBox groupbox, TextBox textbox1, TextBox textbox2, TextBox textbox3)
        {
            //passed in character name, music, image gets filled with the corresponding information in the characterList
            character.Name = config.ArrayOfCharacters.characterList[i].Name;
            character.Music = config.ArrayOfCharacters.characterList[i].Music;
            character.Image = config.ArrayOfCharacters.characterList[i].Image;
            //if the character name is not default, then
            //change this to textbox checking i guess?!
            if (character.Name != "Character Name" && !character.Name.Contains(" (null)"))
            {
                //change their corresponding name textbox to their name
                textbox1.Text = character.Name;
                textbox1.ForeColor = Color.Black;
                //change their image textbox to display the filename of their image
                textbox2.Text = Path.GetFileName(character.Image);
                //change their music textbox to display the filename of their music
                textbox3.Text = Path.GetFileName(character.Music);
            }
           
        }


        private void testMacro(string music, string image)
        //lets the user see what their macro looks like
        //also checks if they're testing an empty macro
        //and warns them about it
        {
            if (image == null || music == null)            
            {
                MessageBox.Show("Please put both a valid music file and image file in the macro!", "Empty values",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //if they've entered both a valid image and music file, pass it to a new macro object
                //and show the resultingmacro dialogue
                Macro defaultmacro = new Macro(image, music);
                defaultmacro.ShowDialog();
            }
        }//end of testMacro

        private void ImageBrowse(TextBox txt, Character character)
        {
            using (FileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = curd + @"\Macros\Images\";
                fileDialog.CheckFileExists = true;
                fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif";
                fileDialog.DefaultExt = ".jpg; .jpeg; .jpe; .jfif; .png; .gif";
                if (DialogResult.OK == fileDialog.ShowDialog())
                {
                    character.Image = fileDialog.FileName;
                    txt.Text = Path.GetFileName(fileDialog.FileName);
                }
            }
        }

        private void MusicBrowse(TextBox txt, Character character)
        {
            using (FileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = curd + @"\Macros\Music\";
                fileDialog.CheckFileExists = true;
                fileDialog.Filter = "Audio files (*.wav, *.mp3, *.wma, *.ogg, *.flac) | *.wav; *.mp3; *.wma; *.ogg; *.flac";
                fileDialog.DefaultExt = ".wav; .mp3; .wma; .ogg; .flac";
                if (DialogResult.OK == fileDialog.ShowDialog())
                {
                    character.Music = fileDialog.FileName;
                    txt.Text = Path.GetFileName(fileDialog.FileName);
                }
            }
        }

        private void TestDefault_Click(object sender, EventArgs e)
        {
            testMacro(defaultChar.Music, defaultChar.Image);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            testMacro(Character1.Music, Character1.Image);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            testMacro(Character2.Music, Character2.Image);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            testMacro(Character3.Music, Character3.Image);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            testMacro(Character4.Music, Character4.Image);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            testMacro(Character5.Music, Character5.Image);
        }

        private void DefaultImage_MouseClick(object sender, MouseEventArgs e)
        {
            ImageBrowse(defaultImage, defaultChar);
        }

        private void Char1Image_MouseClick(object sender, MouseEventArgs e)
        {
            ImageBrowse(Char1Image, Character1);
        }

        private void Char2Image_MouseClick(object sender, MouseEventArgs e)
        {
            ImageBrowse(Char2Image, Character2);
        }

        private void Char3Image_MouseClick(object sender, MouseEventArgs e)
        {
            ImageBrowse(Char3Image, Character3);
        }

        private void Char4Image_MouseClick(object sender, MouseEventArgs e)
        {
            ImageBrowse(Char4Image, Character4);
        }

        private void Char5Image_MouseClick(object sender, MouseEventArgs e)
        {
            ImageBrowse(Char5Image, Character5);
        }

        private void DefaultMusic_MouseClick(object sender, MouseEventArgs e)
        {
            MusicBrowse(defaultMusic, defaultChar);
        }

        private void Char1Music_MouseClick(object sender, MouseEventArgs e)
        {
            MusicBrowse(Char1Music, Character1);
        }

        private void Char2Music_MouseClick(object sender, MouseEventArgs e)
        {
            MusicBrowse(Char2Music, Character2);
        }

        private void Char3Music_MouseClick(object sender, MouseEventArgs e)
        {
            MusicBrowse(Char3Music, Character3);
        }

        private void Char4Music_MouseClick(object sender, MouseEventArgs e)
        {
            MusicBrowse(Char4Music, Character4);
        }

        private void Char5Music_MouseClick(object sender, MouseEventArgs e)
        {
            MusicBrowse(Char5Music, Character5);
        }

        private void NameBoxes(TextBox txt)
        {
            txt.Clear();
            txt.ForeColor = Color.Black;
        }

        private void Character1Name_MouseClick(object sender, MouseEventArgs e)
        {
            NameBoxes(Character1Name);
        }
        private void Character2Name_MouseClick(object sender, MouseEventArgs e)
        {
            NameBoxes(Character2Name);
        }

        private void CharacterName3_MouseClick(object sender, MouseEventArgs e)
        {
            NameBoxes(CharacterName3);
        }

        private void CharacterName4_MouseClick(object sender, MouseEventArgs e)
        {
            NameBoxes(CharacterName4);
        }

        private void CharacterName5_MouseClick(object sender, MouseEventArgs e)
        {
            NameBoxes(CharacterName5);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == AboutTab)//your specific tabname
            {

                label5.Width = tabControl1.Width;
                Size labels = new Size(((int)(.5 * tabControl1.Width)), 300);
                label1.MaximumSize = labels;
                label2.MaximumSize = labels;
                label3.MaximumSize = labels;
                label4.MaximumSize = labels;

            }

        }

        private void PasswordCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (passwordCheck.Checked)
            { exposePassword.Text = user.Password; }
            else
            { exposePassword.Text = "*********"; }
        }

        private void UpdateFlavorLabel_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://forms.gle/fogmcDSDarSzH2Fi7");
            Process.Start(sInfo);
        }

        private void NameButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitter.com/alexwelsby13");
            Process.Start(sInfo);
        }
    }
}
