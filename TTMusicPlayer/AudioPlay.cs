/**
* 名称: AudioPlay
* 作者: lizhongxiang
* 时间: 2015-06-23
* 版本: 1
* 说明: 音频播放（播放、暂停、继续、停止、终止）
*       
* 历史:
* 版本		时间			修改人		      说明
* 1		2015-06-24		lizhongxiang		音乐播放逻辑
*/

using System;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;

namespace MikiMusicPlayer
{
    /// <summary>
    /// 音乐播放类
    /// </summary>
    public class AudioPlay
    {
        #region 音频输出实例
        /// <summary>
        /// 音频输出
        /// </summary>
        private readonly ISoundOut _soundOut = new WasapiOut();
        #endregion

        #region 音量
        /// <summary>
        /// 属性 音量
        /// </summary>
        public float Volume { get; set; }
        #endregion

        #region 播放

        /// <summary>
        ///  播放
        /// </summary>
        /// <param name="source">音频文件路径</param>
        public void Play(string source)
        {
            try
            {
                //对当前音频文件的播放状态进行判断
                if (_soundOut.PlaybackState == PlaybackState.Paused || _soundOut.PlaybackState == PlaybackState.Playing)
                //暂停或播放状态
                {
                    //继续播放
                    _soundOut.Resume();

                }
                else
                {
                    //将string 类型转换成音频文件资源
                    IWaveSource audioSource = CodecFactory.Instance.GetCodec(source);
                    //实例化音频输出

                    //初始化音频输出
                    _soundOut.Initialize(audioSource);
                    _soundOut.Volume = Volume;
                    //播放
                    _soundOut.Play();

                }
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

        #region 暂停

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (_soundOut.PlaybackState == PlaybackState.Playing)
            {
                _soundOut.Pause(); //暂停播放
            }
        }

        #endregion

        #region 继续

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume()
        {
            _soundOut.Resume(); //继续播放
        }

        #endregion

        #region 停止

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _soundOut.Stop(); //停止音频输出
        }

        #endregion

        #region 终止音频输出

        /// <summary>
        /// 释放音频资源
        /// </summary>
        public void Dispose()
        {
            _soundOut.Dispose(); //释放占用的音频资源
        }

        #endregion

        #region 设置音量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentVolume"></param>
        public void SetVolumeValue(float currentVolume)
        {
            _soundOut.Volume = currentVolume / 100;
            Volume = currentVolume / 100;
        }
        #endregion

        #region 当前播放的状态
        /// <summary>
        /// 当前音频播放的状态
        /// </summary>
        public PlaybackState PlaybackState { get; set; }
        #endregion
    }
}