/**
* 名称: Audio
* 作者: lizhongxiang
* 时间: 2015-06-23
* 版本: 1
* 说明: 音频实体类
*       
* 历史:
* 版本		时间			修改人		说明
* 1		2015-06-24		lizhongxiang    音频信息构成参数
*/

using System;

namespace MikiMusicPlayer
{
    /// <summary>
    /// 音频实体的构造参数
    /// </summary>
    internal class Audio
    {
        /// <summary>
        /// 音频文件在播放列表中的索引
        /// </summary>
        public int AudioIndex { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 音频时长
        /// </summary>
        public TimeSpan AudioTime { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 歌手
        /// </summary>
        public string Singer { get; set; }

        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// 比特率
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileLayout { get; set; }

        /// <summary>
        /// mm:ss格式的音频时间
        /// </summary>
        public string TimeString { get; set; }

        public string TimePast { get; set; }
    }
}