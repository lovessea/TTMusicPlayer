using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// MainWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        System.ComponentModel.BackgroundWorker backgroundWorker1;  
        public MainWindow2()
        {
            InitializeComponent();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            progressbar1.Maximum = 10;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;//申请后台程序停止,注意本方法使用前,需要将bgw的WorkerSupportsCancellation 值设为true,否则将不起作用.  
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerAsync(); 
        }

        void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DoWork(backgroundWorker1); 
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {

                this.Dispatcher.BeginInvoke((Action)delegate()
                {
                    aaa.Visibility = Visibility.Hidden;
                });

            } 
        }

        void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressbar1.Value = e.ProgressPercentage;
            txtLab.Content = e.UserState.ToString();
        }
        private void DoWork(System.ComponentModel.BackgroundWorker bk)
        {


            for (int i = 0; i <= 10; i++)
            {

                bk.ReportProgress(i, string.Format("数据正在加载：{0} %", i));
                Thread.Sleep(3000);
            }
        }
    }
}
