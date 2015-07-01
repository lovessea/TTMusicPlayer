using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weather
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow 
    {
        //Province 省
        public List<Province> ProvinceList { set; get; }
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            
        }
        #region 窗体移动
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }
        #endregion

        private void LoadData()
        {

        }

        
       
    }
}
