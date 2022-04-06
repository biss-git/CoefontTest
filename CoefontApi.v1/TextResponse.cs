using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoefontApi.v1
{
    public class TextResponse
    {
        /// <summary>
        /// 音声合成のリクエスト内容
        /// </summary>
        public Text RequestText { get; set; }

        /// <summary>
        /// 音声合成のリクエスト内容(Json文字列)
        /// </summary>
        public string RequestTextString { get; set; }

        /// <summary>
        /// 音声ファイルのurl
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 音声ファイルのバイナリ
        /// </summary>
        public byte[] Wave { get; set; }

        /// <summary>
        /// wav or mp3
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// Url の有効期限
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }
}
