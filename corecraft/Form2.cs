using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace corecraft
{
    public partial class Form2 : Form
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.corecraft";
        public String line;
        int ram;
        string ramtoc;
        public Form2()
        {
            InitializeComponent();
            try
            {
                StreamReader sr = new StreamReader(appdata + "\\launchersettings.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Contains("r="))
                    {
                        ramtoc = line.TrimStart('r', '=');
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch
            {
                MessageBox.Show("Váratlan hiba!");
            }
                switch (ramtoc)
                {
                    case "1024":
                        radioButton1.Checked = true;
                        break;
                    case "2048":
                        radioButton2.Checked = true;
                        break;
                    case "4096":
                        radioButton5.Checked = true;
                        break;
                    case "8192":
                        radioButton3.Checked = true;
                        break;
                    case "12288":
                        radioButton4.Checked = true;
                        break;
                    case "16384":
                        radioButton6.Checked = true;
                        break;
                    default:
                        break;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if (radioButton1.Checked)
	{
        ram = 1024;
	}else if (radioButton2.Checked){
        ram = 2048;
    }
           else if (radioButton5.Checked)
           {
               ram = 4096;
           }
           else if (radioButton3.Checked)
           {
               ram = 8192;
           }
           else if (radioButton4.Checked)
           {
               ram = 12288;
           }
           else if (radioButton6.Checked)
           {
               ram = 16384;
           }
            if(ram != 0){
                 TextWriter tw = new StreamWriter(appdata + "\\launchersettings.txt");
                 tw.WriteLine("r=" + ram);
                 tw.Close();
                 this.Close();
            }
        }
    }
}
