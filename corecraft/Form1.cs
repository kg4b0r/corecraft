using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace corecraft
{
    public partial class Form1 : Form
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.corecraft";
        public String line;
        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(appdata))
            {
                MessageBox.Show("Hibás telepítés!");
                this.Close();
            }
            panel1.BackColor = Color.FromArgb(200, Color.Black);

            checkversion();
        }

        private void checkversion()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri("http://corecraft.hu/kliens/update/verzioinfo.txt"), @appdata + "\\verzioinfo.txt");
            

        }

        private void kilépésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string ido = "Idő: " + Convert.ToString(DateTime.Now.Hour) + ":" + Convert.ToString(DateTime.Now.Minute) + ":" + Convert.ToString(DateTime.Now.Second);
            toolStripStatusLabel1.Text = ido;
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            try
            {
                StreamReader sr = new StreamReader(appdata + "\\verzioinfo.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Contains("v="))
                    {
                        toolStripStatusLabel3.Text = "Verzió: " + line.TrimStart('v', '=');
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        public class Login
        {
            /// <summary>
            /// Used to generate a session with login.minecraft.net for online use only
            /// </summary>
            /// <param name="username">The player's username</param>
            /// <param name="password">The player's password</param>
            /// <param name="clientVer">The client version (look here http://wiki.vg/Session)</param>
            /// <returns>Returns 5 values split by ":" Current Version : Download Ticket : Username : Session ID : UID</returns>
            public static string generateSession(string username, string password, int clientVer)
            {
                //TODO egyedi login page
                string netResponse = httpGET("https://login.minecraft.net?user=" + username + "&password=" + password + "&version=" + clientVer);
                return netResponse;
            }

            public static string httpGET(string URI)
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }

            /// <summary>
            /// Used to start minecraft with certain arguments
            /// </summary>
            /// <param name="mode">True for online and false for offline</param>
            /// <param name="ramMin">Equivalent to the -Xms argument</param>
            /// <param name="ramMax">Equivalent to the -Xmx argument</param>
            /// <param name="username">The player's username</param>
            /// <param name="sessionID">The session id generated from login.minecraft.net</param>
            /// <param name="debug">True for console boot and false for no console</param>
            public static void startMinecraft(int ramMin, int ramMax, string username, string sessionID)
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.corecraft";
                Process proc = new Process();
               
                
                    proc.StartInfo.FileName = "javaw";

                    proc.StartInfo.Arguments = "-Xms" + ramMin + "M -Xmx" + ramMax + "M -Djava.library.path=" + appData + "/bin/natives -cp " + appData + "/bin/minecraft.jar;" + appData + "/bin/jinput.jar;" + appData + "/bin/lwjgl.jar;" + appData + "/bin/lwjgl_util.jar net.minecraft.client.Minecraft " + username + " " + sessionID;
                proc.Start();
            }
        }

        private void beállításokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm2 = new Form2();
            frm2.Show();
        }

        private void coreCraftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://corecraft.hu");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text, pass = textBox2.Text ;
            string response = Login.generateSession(user, pass, 13);
            string[] split = response.Split(':');
            label3.Refresh();
            try
            {
                string sessionID = split[3];// Get Session ID
                string username = split[2];// Get username, in case user is on a migrated account.
                label3.ForeColor = System.Drawing.Color.White;
                label3.Text = "Ellenőrzés!";
                label3.Refresh();
                label3.Text = "Sikeres belépés!";
                Login.startMinecraft(512, 1024, username, sessionID);
                this.Close();
            }
            catch (System.IndexOutOfRangeException)
            {
                label3.ForeColor = System.Drawing.Color.Red;
                if (response == "Bad login")
                {
                    label3.Text = "Sikertelen belépés!";
                }
                else
                {
                    label3.Text = response;
                }
            }
        }
    }
}
