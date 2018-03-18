using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using CCWin.Win32;
using System.Runtime.Remoting.Messaging;
using FileManager.Model;

namespace FileManager
{
    public partial class FrmTipLogin : CCSkinMain
    {
        public FrmTipLogin()
        {
            InitializeComponent();
        }

        public const int AW_SLIDE = 262144;
        public const int AW_VER_NEGATIVE = 8;

        //���ڼ���ʱ
        private void FrmTipUpload_Load(object sender, EventArgs e)
        {
            //��ʼ�����ڳ���λ��
            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.PointToScreen(p);
            this.Location = p;
            NativeMethods.AnimateWindow(this.Handle, 130, AW_SLIDE + AW_VER_NEGATIVE);//��ʼ���嶯��
        }

        //����ʱ6��رյ�����
        private void timShow_Tick(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
 
        private void skinButton_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
        }
    }
}
