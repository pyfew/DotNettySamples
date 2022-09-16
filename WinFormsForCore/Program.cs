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
            //var lable = new Label("This is a lable");//新建标签
            //win.Add(lable);//将标签加入到窗体
            //win.ShowAll();//显示窗体
            //Application.Run();//运行窗体 
        }
    }
}
