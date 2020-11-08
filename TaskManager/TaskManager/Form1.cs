using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        Process p;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            label3.Text = "";
            Process[] processes = Process.GetProcesses();
            textBox1.Text = String.Format("{0, -25} | {1, -10} | {2, -30} |\r\n", "NAME", "PID", "MAINWINDOW TITLE");
            foreach (Process p in processes)
            {
                InfoProcess(p);                                                         
            }                                   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            p = null;
            if (ComprobarPID(textBox2.Text))
            {                
                if (ExistProcess(ref p) && p!=null)
                {
                    textBox1.Text = String.Format("{0, -25} | {1, -10} | {2, -30} |\r\n", "NAME", "PID", "MAINWINDOW TITLE");
                    InfoProcess(p);
                    try
                    {
                        ProcessThreadCollection threads = p.Threads;
                        textBox1.Text += String.Format("\r\nSUBPROCESS\r\n{0, -10} | {1, -19} |\r\n", "PID", "START TIME");
                        foreach (ProcessThread t in threads)
                        {
                            textBox1.Text += String.Format("{0, -10} | {1, -19} |\r\n",
                                t.Id, t.StartTime);
                        }

                        ProcessModuleCollection modules = p.Modules;
                        textBox1.Text += String.Format("\r\n\r\n{0, -30}| {1, -50} |\r\n",
                                "MODULE NAME", "FILE NAME");
                        foreach (ProcessModule m in modules)
                        {
                            textBox1.Text += String.Format("{0, -30}| {1, -50} |\r\n",
                                m.ModuleName.Length > 30? m.ModuleName.Substring(0,27)+"..." : m.ModuleName,
                                m.FileName.Length > 50 ? m.FileName.Substring(0, 47) + "..." : m.FileName);
                        }
                    }
                    catch (Win32Exception)
                    {
                        label2.Text = "Access denied";
                    }                                                                       
                }
                else
                {
                    textBox1.Text = "";
                    label2.Text = "PID not found";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            p = null;
            if (ComprobarPID(textBox2.Text))
            {
                if(ExistProcess(ref p) && p != null)
                {
                    try{
                        if (p.CloseMainWindow()) {
                            label3.Text = "Closure request has been sent";
                        }
                        else
                        {
                            label2.Text = "Process could not be closed. It does not have a main window or the main window is disabled";
                        }
                    }
                    catch (Win32Exception)
                    {
                        label2.Text = "This process can not be closed";
                    }                    
                }
                else
                {
                    textBox1.Text = "";
                    label2.Text = "PID not found";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            p = null;
            if (ComprobarPID(textBox2.Text))
            {
                if (ExistProcess(ref p) && p != null)
                {
                    try{
                        p.Kill();
                        label3.Text = "Process killed correctly";
                    }
                    catch (Win32Exception)
                    {
                        label2.Text = "This process can not be killed";
                    }
                }
                else
                {
                    textBox1.Text = "";
                    label2.Text = "PID not found";
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            label3.Text = "";
            if (textBox2.TextLength > 0)
            {                
                try
                {
                    Process p = Process.Start(textBox2.Text);
                }
                catch (Win32Exception)
                {
                    label2.Text = "This app can not be started";
                }
                catch (FileNotFoundException)
                {
                    label2.Text = "App not found";
                }
            }
            else
            {
                label2.Text = "Enter App name";
            }
        }


        private void InfoProcess(Process p)
        {
            textBox1.Text += String.Format("{0, -25} | {1, -10} | {2, -30} |\r\n",
                    p.ProcessName.Length > 25 ? p.ProcessName.Substring(0, 22) + "..." : p.ProcessName, p.Id,
                    p.MainWindowTitle.Length > 30 ? p.MainWindowTitle.Substring(0, 27) + "..." : p.MainWindowTitle);
        }

        private bool ExistProcess(ref Process p)
        {
            int pid = Convert.ToInt32(textBox2.Text);
            Process[] processes = Process.GetProcesses();
            foreach (Process pro in processes)
            {
                if (pro.Id == pid)
                {
                    p = Process.GetProcessById(pid);
                    return true;
                }
            }
            return false;
        }

        private bool ComprobarPID(string tb)
        {
            label3.Text = "";
            try
            {
                if (tb.Length == 0)
                {
                    throw new ArgumentException();
                }
                int pid = Convert.ToInt32(tb);
            }
            catch (FormatException)
            {
                label2.Text = "The data entered must be a number";
                return false;
            }
            catch (ArgumentException)
            {
                label2.Text = "Enter PID process";
                return false;
            }
            catch (OverflowException)
            {
                label2.Text = "Invalid data";                
                return false;
            }
            label2.Text = "";
            return true;
        }        
    }     
}
