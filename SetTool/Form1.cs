using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
namespace SetTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SetReg.Checked == true)
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey("SYSTEM", true);
                key = key.OpenSubKey("ControlSet001", true);
                key = key.OpenSubKey("Control", true);
                key = key.OpenSubKey("GraphicsDrivers", true);
                key = key.OpenSubKey("Configuration", true);   //一直要定位到这一子键 
                RegEdit(key);  //递归方法
                key.Close();//关闭注册表
            }
            if (Setfolder.Checked == true)
            {
                SetFolder();
            }
        }
        /// <summary>
        ///通过修改注册表，调整分辨率 
        /// </summary>
        /// <param name="key"></param>
        public void RegEdit(RegistryKey key)
        {
            try
            {
                string[] str = key.GetSubKeyNames();    //列出子项下面的所有的子项  
                if (str.Count() > 0)
                {
                    //成功获取到了子项，检查子项下面还有没有子项，有的话调用方法继续检查
                    for (int i = 0; i < str.Count(); i++)
                    {
                        key = key.OpenSubKey(str[i], true);   //定位到Configuration下的子项
                        string[] ste = key.GetSubKeyNames();  //获取子项下面的子项
                        if (ste.Count() > 0)
                        {
                            RegEdit(key); //子项下面还有子项，利用递归继续调用该方法
                        }
                        else
                        {
                            //没有子项了，开始检查项值
                            string[] stc = key.GetValueNames();  //获取键的名字
                            foreach (var item in stc)
                            {
                                if (item == "Scaling")
                                {
                                    key.SetValue("Scaling", 3);    //根据键的名字修改键值               
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 窗体加载时，默认选中两个选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SetReg.Checked = true;
            Setfolder.Checked = true;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        public void SetFolder()
        {
            string path1 = "D:\\Develop";
            string path2 = "D:\\Game";
            string path3 = "D:\\Program";
            string path4 = "D:\\Program\\Other";
            string path5 = "E:\\Virtual Machines";
            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);
            Directory.CreateDirectory(path3);
            Directory.CreateDirectory(path4);
            Directory.CreateDirectory(path5);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Messages mes = new Messages();
            mes.Show();
        }
    }
}
