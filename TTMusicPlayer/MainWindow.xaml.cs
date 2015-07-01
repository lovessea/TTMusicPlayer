/**
* 名称: 音乐播放器主窗体逻辑处理类
* 作者: lizhongxiang
* 时间: 2015-06-23
* 版本: 1
* 说明: 音乐播放器主窗体
*       
* 历史:
* 版本		时间			修改人		说明
* 1		2015-06-24		lizhongxiang		创建本文件
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CSCore;
using CSCore.Codecs;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace MikiMusicPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        #region 播放器样式及全局变量

        #region 播放器状态样式
        /// <summary>
        /// 允许通过非工作区域的暴露部分窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 播放态--显示暂停按钮
        /// </summary>
        private void ShowPlayState()
        {
            BtnPlayMusic.Visibility = Visibility.Collapsed;
            BtnPauseMusic.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 暂停态--显示播放按钮
        /// </summary>
        private void ShowPauseState()
        {
            BtnPauseMusic.Visibility = Visibility.Collapsed;
            BtnPlayMusic.Visibility = Visibility.Visible;
        }

        #endregion

        #region 音量调节框--鼠标移上去出现，鼠标离开 音量调节框消失
        /// <summary>
        /// 获取焦点 显示音量调节框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sound_OnMouseMove(object sender, MouseEventArgs e)
        {
            SoundBorder.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// 鼠标移除 隐藏音量调节框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundBorder_OnMouseLeave(object sender, MouseEventArgs e)
        {
            SoundBorder.Visibility = Visibility.Collapsed;

        }
        #endregion

        #region 禁音非禁音 样式

        /// <summary>
        /// 显示禁音按钮
        /// </summary>
        private void ShowSoundMuteButton()
        {
            SoundMuteButton.Visibility = Visibility.Visible;
            SoundButton.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 显示声音按钮
        /// </summary>
        private void ShowSoundButton()
        {
            SoundButton.Visibility = Visibility.Visible;
            SoundMuteButton.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region 全局变量

        //全局变量--记录当前博凡的歌曲索引
        private int _index;

        /// <summary>
        /// 实例化音频播放类
        /// </summary>
        private readonly AudioPlay _audioPlay = new AudioPlay();

        /// <summary>
        /// 设置歌曲列表保存路径
        /// </summary>
        readonly string _listpath = AppDomain.CurrentDomain.BaseDirectory + "Musict.lst";

        /// <summary>
        /// 歌曲播放时间：播放完自动换下一首歌
        /// </summary>
        readonly DispatcherTimer _musicStartToEndTimer = new DispatcherTimer();

        //TODO 计时开始 
        /// <summary>
        /// 计时开始 
        /// </summary>
        //private DispatcherTimer _timer;

        //TODO 播放进度
        /// <summary>
        /// 播放进度
        /// </summary>
        //private ProcessCount _processCount;


        #endregion

        #endregion

        #region 窗口初始化
        /// <summary>
        /// 窗口初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;


        }
        #endregion

        #region 窗体load事件
        /// <summary>
        /// 窗体load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //加载播放列表--默认选定项为第一项
            //判断本地播放列表文件是否存在
            string musiclist = null;
            if (File.Exists(_listpath))
            {
                //存在
                //读取文件 如果文件非空
                using (StreamReader sr = new StreamReader(_listpath))
                {
                    musiclist = sr.ReadToEnd();
                }
                //Json装list对象
                List<MusicInfo> playlists = new List<MusicInfo>();
                playlists = JsonConvert.DeserializeObject<List<MusicInfo>>(musiclist);

                //获取文件路径为一个集合
                List<string> filespaths = new List<string>();
                foreach (MusicInfo info in playlists)
                {
                    filespaths.Add(info.FilePath);
                }
                LoadFileList(filespaths);
            }

            PlayLists.SelectedIndex = 1;
        }


        #endregion

        #region 窗口关闭释放音频输出

        /// <summary>
        /// 窗口关闭释放音频输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _audioPlay.Dispose();
        }

        #endregion

        #region 打开文件

        private void AddMusic_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); //实例化文件打开对话框

            //判断文件打开路径是否存在 不存在则创建
            string appStartPath = AppDomain.CurrentDomain.BaseDirectory + "Music";

            if (!Directory.Exists(appStartPath))
            {
                Directory.CreateDirectory(appStartPath);
            }

            ofd.Title = "打开"; //设置对话框Title
            ofd.InitialDirectory = appStartPath; //设置对话框初始化路径
            ofd.RestoreDirectory = true; //设置对话框记忆打开的文件夹路径
            ofd.Multiselect = true; //设置对话框获取文件允许多个文件
            ofd.Filter = "所有音频文件|*.mp3;*.wma;*.wav;*.flac;*.aac;*.mid |MP3 音频文件(*.mp3)|*.mp3"; //设置对话框允许打开的文件类型

            if (ofd.ShowDialog() == true)
            {
                //采用多线程加载数据


                //转换歌曲数组为列表
                List<String> filespaths = new List<String>(ofd.FileNames);
                //将获取到的文件添加到列表中
                LoadFileList(filespaths);


                //以Json的形式保存本地播放列表
                SaveMusicList(filespaths);
            }
        }




        #region 加载文件到list列表
        /// <summary>
        /// 加载文件到list列表
        /// </summary>
        /// <param name="filespaths"></param>
        private void LoadFileList(List<string> filespaths)
        {
            List<Audio> listaudios = new List<Audio>();
            int index = 1; //设置初始歌曲Inddex 为1

            //遍历文件获取文件的文件流信息（时间长度、文件名）
            foreach (string filepath in filespaths)
            {
                //TODO 判断文件是否错误--非音频文件等错误
                //

                TimeSpan timeSpan = GetMusicTimeSpan(filepath);
                string fileName = Path.GetFileNameWithoutExtension(filepath); //获取歌曲名
                Audio ad = new Audio();

                ad.AudioIndex = index;

                ad.FileName = fileName;

                ad.AudioTime = timeSpan;//获取音频时间间隔

                ad.FilePath = filepath;

                ad.SampleRate = GetSampleRate(filepath);

                ad.TimeString = Convert.ToDateTime(timeSpan.ToString()).ToString("mm:ss"); //将时间转换为字符串
                //动态加载歌曲信息
                listaudios.Add(ad);
                index++;

            }
            MusicListView.ItemsSource = listaudios;
        }

        #region 保存播放列表

        /// <summary>
        /// 
        /// </summary>
        private void SaveMusicList(List<string> filePaths)
        {
            List<MusicInfo> playlists = new List<MusicInfo>();
            foreach (var filepath in filePaths)
            {
                TimeSpan timeSpan = GetMusicTimeSpan(filepath);
                MusicInfo audio = new MusicInfo
                {
                    FileName = Path.GetFileNameWithoutExtension(filepath), //歌曲名
                    FileLength = Convert.ToDateTime(timeSpan.ToString()).ToString("mm:ss"),//歌曲时长
                    FilePath = filepath     //歌曲路径
                };
                playlists.Add(audio);
            }
            string filelist = JsonConvert.SerializeObject(playlists);


            //判断文件是否存在
            if (File.Exists(_listpath))
            {
                File.Delete(_listpath);

            }

            StreamWriter sw = new StreamWriter(_listpath);

            //写入数据流
            sw.Write(filelist);

            //清空数据缓存
            sw.Flush();

            //关闭写入
            sw.Close();

        }

        #endregion

        #region 获取音频比特率
        /// <summary>
        /// 获取音频比特率
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private int GetSampleRate(string filepath)
        {
            IWaveSource source = CodecFactory.Instance.GetCodec(filepath);
            int sampleRate = source.WaveFormat.SampleRate;
            return sampleRate;
        }
        #endregion


        #region 获取音频文件开始到结束的时间间隔

        /// <summary>
        /// 获取音频文件开始到结束的时间间隔
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>返回时间间隔</returns>
        private TimeSpan GetMusicTimeSpan(string filepath)
        {
            IWaveSource source = CodecFactory.Instance.GetCodec(filepath);
            TimeSpan musicTimeSpan = source.GetLength();
            return musicTimeSpan;
        }

        #endregion

        #endregion

        #endregion

        #region 打开文件夹

        #endregion

        #region 播放-继续

        /// <summary>
        /// 播放--默认播放列表第一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlayMusic_OnClick(object sender, RoutedEventArgs e)
        {
            if (MusicListView.ItemsSource != null)
            {

                Audio audiofile = MusicListView.Items[0] as Audio;

                if (audiofile != null)
                {

                    Play(audiofile.FilePath, audiofile.AudioTime);

                }
                ShowPlayState();
            }

        }

        /// <summary>
        /// 处理播放有关的业务
        /// </summary>
        /// <param name="filepath">音频文件路径</param>
        /// <param name="timeSpan">音频文件时长</param>
        private void Play(string filepath, TimeSpan timeSpan)
        {
            //TODO 计时
            //计时开始




            //播放歌曲、更改播放状态、更改当前播放的时间、倒计时播放下一首
            _audioPlay.Play(filepath);

            //展示当前播放状态
            ShowPlayState();

            //倒计时播放歌曲，当歌曲播放结束后自动切换到下一首
            _musicStartToEndTimer.Interval = timeSpan;//初始化 歌曲播放完触发
            _musicStartToEndTimer.Start();
            _musicStartToEndTimer.Tick += musicStartToEndTimer_Tick;

        }

        /// <summary>
        /// 时间控制：播放时间结束 切换下一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void musicStartToEndTimer_Tick(object sender, EventArgs e)
        {
            if (_index < MusicListView.Items.Count - 1)
            {
                Next();
            }
            else
            {
                _musicStartToEndTimer.Stop();
            }
        }
        #endregion

        #region 暂停

        /// <summary>
        /// 暂停按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPauseMusic_OnClick(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private void Pause()
        {
            _audioPlay.Pause();
            ShowPauseState();
        }
        #endregion

        #region 双击播放

        /// <summary>
        /// 双击歌曲播放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MusicListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Audio audiofile = MusicListView.SelectedItem as Audio;
            _index = MusicListView.SelectedIndex;
            if (audiofile != null)
            {

                _audioPlay.Stop();
                Play(audiofile.FilePath, audiofile.AudioTime);

            }
        }

        #endregion

        #region 上一首

        /// <summary>
        /// 上一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousMusic_OnClick(object sender, RoutedEventArgs e)
        {
            Previous();
        }

        /// <summary>
        /// 上一首
        /// </summary>
        private void Previous()
        {
            if (_index > 0)
            {
                _index--;
                Audio audiofile = MusicListView.Items[_index] as Audio;
                if (audiofile != null)
                {
                    _audioPlay.Stop();
                    _audioPlay.Play(audiofile.FilePath);
                }
            }
        }
        #endregion

        #region 下一首

        /// <summary>
        /// 下一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextMusic_OnClick(object sender, RoutedEventArgs e)
        {
            Next();
        }

        /// <summary>
        /// 下一首
        /// </summary>
        private void Next()
        {

            if (_index < MusicListView.Items.Count - 1)
            {
                _index++;
                Audio audiofile = MusicListView.Items[_index] as Audio;
                if (audiofile != null)
                {
                    _audioPlay.Stop();
                    Play(audiofile.FilePath, audiofile.AudioTime);

                }
            }
        }
        #endregion

        #region 停止
        /// <summary>
        /// 停止
        /// </summary>
        //private void Stop()
        //{
        //    _audioPlay.Stop();
        //}

        #endregion

        #region 设置音量
        /// <summary>
        /// 改变音量（设置音量事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVolumeValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShowSoundButton();
            float volumeValue = (float)BtnVolumeValue.Value;
            _audioPlay.SetVolumeValue(volumeValue);
        }
        #endregion

        #region 禁音--非禁音 事件
        /// <summary>
        /// 禁音 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowSoundMuteButton();
            //静音
            _audioPlay.SetVolumeValue(0.0f);
        }


        /// <summary>
        /// 有声
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundMuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowSoundButton();

            float currentVolume = (float)BtnVolumeValue.Value;
            _audioPlay.SetVolumeValue(currentVolume);

        }
        #endregion

    }
}