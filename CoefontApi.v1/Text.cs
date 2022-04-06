using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoefontApi.v1
{
    public  class Text
    {
        /// <summary>
        /// 音声変換を行うcoefontのID。coefont詳細画面のurlに表示される個別のuuidを参照。
        /// </summary>
        public string coefont { get; set; } = "b0655711-b398-438f-83e3-4c3c3ed746dd";

        /// <summary>
        /// 音声変換するテキスト
        /// string: [0, 1000] characters
        /// </summary>
        public string text { get; set; } = string.Empty;

        private double _speed = 1;
        /// <summary>
        /// 音声の速度 [倍]
        /// [0.1, 10]
        /// default: 1
        /// </summary>
        public double speed
        {
            get => _speed;
            set => _speed = Math.Clamp(value, 0.1, 10);
        }

        private double _pitch = 0;
        /// <summary>
        /// 音声のピッチ。+1200 で 1オクターブ変化 [cent]
        /// [-3000, 3000]
        /// default: 0
        /// </summary>
        public double pitch
        {
            get => _pitch;
            set => _pitch = Math.Clamp(value, -3000, 3000);
        }

        private double _kuten = 0.7;
        /// <summary>
        /// 句点の感覚 [秒]
        /// [0, 5]
        /// default: 0.7
        /// </summary>
        public double kuten
        {
            get => _kuten;
            set => _kuten = Math.Clamp(value, 0, 5);
        }

        private double _toten = 0.4;
        /// <summary>
        /// 読点の感覚 [秒]
        /// [0.2, 2]
        /// default: 0.4
        /// </summary>
        public double toten
        {
            get => _toten;
            set => _toten = Math.Clamp(value, 0.2, 2);
        }

        private double _volume = 1;
        /// <summary>
        /// 音量 [倍]
        /// [0.2, 2]
        /// default: 1
        /// </summary>
        public double volume
        {
            get => _volume;
            set => _volume = Math.Clamp(value, 0.2, 2);
        }

        private double _intonation = 1;
        /// <summary>
        /// 抑揚 [倍]
        /// [0, 2]
        /// default: 1
        /// </summary>
        public double intonation
        {
            get => _intonation;
            set => _intonation = Math.Clamp(value, 0, 2);
        }

        /// <summary>
        /// wav or mp3
        /// </summary>
        [JsonConverter(typeof(EnumJsonConverter<FileType>))]
        public FileType format { get; set; } = FileType.wav;

        public string PostValidation()
        {
            if (string.IsNullOrWhiteSpace(coefont))
            {
                return $"Argument Error: {nameof(coefont)} = {coefont}";
            }

            if (string.IsNullOrWhiteSpace(coefont))
            {
                return $"Argument Error: {nameof(coefont)} = {coefont}";
            }

            return null;
        }
    }
}
