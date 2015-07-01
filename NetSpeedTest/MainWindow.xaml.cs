using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using 网络延迟测试;

namespace NetSpeedTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetComboBoxItems();

            LoadData();

        }

        private void LoadData()
        {
            InternetConnected internetConnected = new InternetConnected();
            if (internetConnected.GetConnectedState())
            {
                BtnStart.IsEnabled = true;
                //网络供应商
                ISPName.Content = internetConnected.GetISPName();
                InternetIP.Content = internetConnected.GetWANAddress();

            }
            else
            {
                Delay.Content = "网络连接失败";
            }
        }




        /// <summary>
        /// 加载comboBox列表
        /// </summary>
        private void GetComboBoxItems()
        {
            DNSAddress.Items.Add("艾欧尼亚");
            DNSAddress.Items.Add("祖安");
            DNSAddress.Items.Add("诺克萨斯");
            DNSAddress.Items.Add("班德尔城");
            DNSAddress.Items.Add("百度服务器");
            DNSAddress.Items.Add("网易服务器");
            DNSAddress.Items.Add("新浪服务器");
            DNSAddress.Items.Add("腾讯服务器");
            DNSAddress.SelectedIndex = 0;
        }



        #region 开始测试
        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            timer.Tick += timer_Tick;
            timer.Start();
            DNSAddress.IsEnabled = false;
            timer.Interval = TimeSpan.FromSeconds(1); //执行时间间隔

            BtnStart.Visibility = Visibility.Collapsed;
            BtnStop.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            //远程服务器IP  
            string ipStr = GetDNSAddress();
            DataResult.Items.Clear();

            //构造Ping实例  
            Ping pingSender = new Ping();
            //Ping 选项设置  
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据  
            string data = "lovessea@sina.com";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间  
            const int timeout = 1000;
            //调用同步 send 方法发送数据,将返回结果保存至PingReply实例  
            try
            {
                PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                if (reply != null && reply.Status == IPStatus.Success)
                {
                    DataResult.Items.Add("响应主机地址：" + reply.Address);
                    DataResult.Items.Add("延迟：" + reply.RoundtripTime + " ms");
                    DataResult.Items.Add("生存时间（TTL）：" + reply.Options.Ttl);
                    DataResult.Items.Add("是否控制数据包的分段：" + reply.Options.DontFragment);
                    DataResult.Items.Add("缓冲区大小：" + reply.Buffer.Length);
                    if (reply.RoundtripTime <= 90)
                    {
                        Delay.Content = "延迟：" + reply.RoundtripTime + " ms";
                        Delay.Foreground = Brushes.GreenYellow;
                    }
                    else
                        if (reply.RoundtripTime > 90 && reply.RoundtripTime <= 150)
                        {
                            Delay.Content = "延迟：" + reply.RoundtripTime + " ms";
                            Delay.Foreground = Brushes.Orange;
                        }
                        else
                            if (reply.RoundtripTime > 150)
                            {
                                Delay.Content = "延迟：" + reply.RoundtripTime + " ms";
                                Delay.Foreground = Brushes.Red;
                            }
                }
                else
                {

                    Delay.Content = " 网络超时 ... ";
                    Delay.Foreground = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                Delay.Content = "网络中断 ...";
                Stop_OnClick(null, null);
            }

        }


        /// <summary>
        /// 根据被选combobox获取服务器地址
        /// </summary>
        /// <returns></returns>
        private string GetDNSAddress()
        {
            if (DNSAddress.SelectedItem != null)
            {
                switch (DNSAddress.SelectedItem.ToString())
                {
                    case "艾欧尼亚":
                        return "183.61.224.63";//广东省东莞市 电信
                    case "祖安":
                        return "115.236.97.158";//浙江省杭州市 电信
                    case "诺克萨斯":
                        return "119.147.127.72";//广东省东莞市 电信
                    case "班德尔城":
                        return "182.131.31.14";//四川省成都市 电信
                    case "百度服务器":
                        return "www.baidu.com";
                    case "网易服务器":
                        return "www.163.com";
                    case "新浪服务器":
                        return "www.sina.com";
                    case "腾讯服务器":
                        return "www.qq.com";

                }
            }
            else
            {
                return DNSAddress.Text.Trim();
            }
            return "127.0.0.1";
        }

        #endregion


        #region 停止执行
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Stop_OnClick(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Delay.Foreground = Brushes.White;
            DNSAddress.IsEnabled = true;
            BtnStop.Visibility = Visibility.Collapsed;
            BtnStart.Visibility = Visibility.Visible;
        }
        #endregion

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           base.DragMove();
        }
    }
}
