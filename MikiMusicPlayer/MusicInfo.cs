/**
* 名称: MusicInfo
* 作者: lizhongxiang
* 时间: 2015-06-23
* 版本: 1
* 说明: 文件基础信息类
*       
* 历史:
* 版本		时间			修改人		说明
* 1		2015-06-24		lizhongxiang		文件信息构造参数
*/
namespace MikiMusicPlayer
{
     public  class MusicInfo
    {
         /// <summary>
         /// 文件名
         /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        public string FileLength { get; set; }
    }
}
