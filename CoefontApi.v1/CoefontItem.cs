using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoefontApi.v1
{
    /// <summary>
    /// Coefont のデータ構造
    /// </summary>
    public class CoefontItem
    {
        /// <summary>
        /// Coefont Id
        /// </summary>
        public string coefont { get; set; }

        /// <summary>
        /// Coefont の名前
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Coefont の説明
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Icon の url
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// ボイスサンプル の Url
        /// </summary>
        public SampleItem[] sample { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public string[] tags { get; set; }
    }
}
