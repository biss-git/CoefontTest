using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoefontApi.v1
{
    /// <summary>
    /// 辞書データ
    /// </summary>
    public class DictItem
    {
        /// <summary>
        /// 辞書に登録する単語
        /// 1-100文字
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// カテゴリ
        /// non-empty
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 読み
        /// 1-100文字
        /// ひらがな　または　カタカナ
        /// </summary>
        public string yomi { get; set; }

        /// <summary>
        /// アクセント
        /// not required
        /// 1(low) または 2(high) のみで構成されており、yomiと同じ文字数
        /// </summary>
        public string accent { get; set; }

        public string PostValidation()
        {
            // text [1 .. 100] characters
            if (string.IsNullOrWhiteSpace(text) ||
                text.Length == 0 ||
                text.Length > 100)
            {
                return $"Argument Error: {nameof(text)} = {text}";
            }

            // non-empty
            if (string.IsNullOrWhiteSpace(category))
            {
                return $"Argument Error: {nameof(category)} = {category}";
            }

            // non-empty ひらがなまたはカタカナ
            if (string.IsNullOrWhiteSpace(yomi) ||
                yomi.Length == 0 ||
                yomi.Length > 100)
            {
                return $"Argument Error: {nameof(yomi)} = {yomi}";
            }

            // not required
            // 
            if (accent != null &&(
                accent.Length != yomi.Length ||
                accent.Any(c => c != '1' && c != '2')))
            {
                return $"Argument Error: {nameof(accent)} = {accent}";
            }

            return null;
        }

        public string DeleteValidation()
        {
            // text [1 .. 100] characters
            if (string.IsNullOrWhiteSpace(text) ||
                text.Length == 0 ||
                text.Length > 100)
            {
                return $"Argument Error: {nameof(text)} = {text}";
            }

            // non-empty
            if (string.IsNullOrWhiteSpace(category))
            {
                return $"Argument Error: {nameof(category)} = {category}";
            }

            return null;
        }

    }
}
