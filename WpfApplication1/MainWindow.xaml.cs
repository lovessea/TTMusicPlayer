using System;
using System.Windows;
using System.Windows.Threading;


namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        private Int32 hour;
        private Int32 minute;
        private Int32 second;

        private ProcessCount processCount;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWin_Loaded);
             
        }


        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            //设置定时器
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(100000);   //时间间隔为一秒
            timer.Tick += new EventHandler(timer_Tick);


        }


        /// <summary>
        /// Timer触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (OnCountDown())
            {
                HourArea.Text = processCount.GetHour();
                MinuteArea.Text = processCount.GetMinute();
                SecondArea.Text = processCount.GetSecond();
            }
            else
                timer.Stop();
        }


        /// <summary>
        /// 处理事件
        /// </summary>
        public event CountDownHandler CountDown;
        public bool OnCountDown()
        {
            if (CountDown != null)
               return CountDown();


            return false;
        }
    


    /// <summary>
    /// 处理倒计时的委托
    /// </summary>
    /// <returns></returns>
    public delegate bool CountDownHandler();


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = true;
            //转换成秒数

            hour = Convert.ToInt32(HourArea.Text);
            minute = Convert.ToInt32(MinuteArea.Text);
            second = Convert.ToInt32(SecondArea.Text);


            //处理倒计时的类
            processCount = new ProcessCount(hour * 3600 + minute * 60 + second);
            CountDown += new CountDownHandler(processCount.ProcessCountDown);


            //开启定时器
            timer.Start();


        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = false;
            hour = Convert.ToInt32(HourArea.Text);
            minute = Convert.ToInt32(MinuteArea.Text);
            second = Convert.ToInt32(SecondArea.Text);
            
        }

        private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
        {
            

            //处理倒计时的类
            processCount = new ProcessCount(hour * 3600 + minute * 60 + second);
            CountDown += new CountDownHandler(processCount.ProcessCountDown);


            //开启定时器
            timer.Start();
        }
    }

}
