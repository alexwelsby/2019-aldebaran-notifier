using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NAudio.Wave;


namespace ADBNotifier
{
    public partial class Macro : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        Image img;
        string url;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        int getWidth;
        int getHeight;
        double ratio;
        double remainder;

        public Macro(string image, string song)
        {
            InitializeComponent();
            base.ShowInTaskbar = false; //prevents macro from showing in taskbar as an icon
            SetStyle(ControlStyles.SupportsTransparentBackColor, true); 
            this.BackColor = this.pictureBox1.BackColor;
            this.TransparencyKey = this.pictureBox1.BackColor; //sets so the background color is transparent

            img = Image.FromFile(@image);

            //the following just checks for img orientation/exif info and rotates based on that
            if (img.PropertyIdList.Contains(0x112)) //0x112 = Orientation
            {
                var prop = img.GetPropertyItem(0x112);
                if (prop.Type == 3 && prop.Len == 2)
                {
                    UInt16 orientationExif = BitConverter.ToUInt16(img.GetPropertyItem(0x112).Value, 0);
                    if (orientationExif == 8)
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    else if (orientationExif == 3)
                    {
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (orientationExif == 6)
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    //just a bunch of if statements that checks for rotation and flips
                }
            }

            //heres the basics. getting width and height of the image assigned
            getWidth = img.Width;
            getHeight = img.Height;
            ratio = 0;

            

            if (getWidth > getHeight && getWidth > 400) // if width is greater than height
            {
                remainder = getWidth % 400;
                ratio = getWidth / 400; // ratio is width divided by 400
                getWidth = 400; //set width to 400
                getHeight = (int)(getHeight / ratio);
            }
            else if (getWidth < getHeight && getHeight > 400)
            {
                remainder = getHeight / 400;
                ratio = getHeight / 400;
                getHeight = 400;
                getWidth = (int)(getWidth / ratio);
            }
            else if (getWidth < 400 || getHeight < 400)
            {

            }
            this.Width = getWidth;
            this.Height = getHeight;
            pictureBox1.Width = getWidth;
            pictureBox1.Height = getHeight;
            pictureBox1.Image = img;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(@song);
                outputDevice.Init(audioFile);
                //passes the string Song as a directory location to audioFile, which outputDevice then plays
            }
            outputDevice.Play();

        }

        public void GetURL(string gotUrl)
        {
            Debug.WriteLine("writing url");
            url = gotUrl;
            Debug.WriteLine(url);
        }

        protected override void OnLoad(EventArgs e)
        {
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            PlaceLowerRight();
            base.OnLoad(e);
            //just looking for the bounds of the window
            //and how and where to put the image on the lower right corner
        }

        private void PlaceLowerRight()
        {
            //Determine "rightmost" screen
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            this.StartPosition = FormStartPosition.Manual;
            this.Left = resolution.Right - (int)(pictureBox1.Width - ( remainder / 125 ));
            this.Top = resolution.Bottom - (int)(pictureBox1.Height - ( remainder / 3 ));
            //this is then called to place the macro box there
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (url != null)
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);
                //if the string url is not null, start new internet browser process
                //pointing to the url
            }
            try
            {
                outputDevice?.Stop();
                outputDevice.Dispose();
                outputDevice = null;
                audioFile.Dispose();
                audioFile = null;
                base.Close();
                base.Dispose();
                //kills the audio player and the 'base', which is the image displayed
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Sound could not dispose: " + exception);
            }
        }
    }
}
