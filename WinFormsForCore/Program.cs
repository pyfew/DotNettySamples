//using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsForCore
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Gtk.Application.Init();
            //var win = new Window("hellworld");
            //var lable = new Label("This is a lable");//�½���ǩ
            //win.Add(lable);//����ǩ���뵽����
            //win.ShowAll();//��ʾ����
            //Application.Run();//���д��� 
        }
    }
}
