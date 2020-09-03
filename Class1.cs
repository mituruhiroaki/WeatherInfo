using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace WeatherInfo
{
    public class Class1                                 //日本の今日の天気(東京都千代田区)を取得します。
    {
        #region<クラスフィールド>
        readonly static string url = "https://tenki.jp/forecast/3/16/4410/13101/10days.html";       //IEは表示されないようになってるはずです
        static string todayWeather = null;
        static WebClient wc = new WebClient();
        static string todayToString = System.DateTime.Today.ToString();
        #endregion


        #region<ファンクション>
        public static Func<string> html = () => {
            wc.Encoding = Encoding.UTF8;
            return wc.DownloadString(url);
        };

        public static Func<string> capture = () => {
            Regex reg = new Regex(@"\d{2}/\d{2}\s");   //yyyy/mm/dd hh:mm:ss  空白\sを入れて、mm/dd\sを取得
            string today = reg.Match(todayToString).ToString();
            string month = today.Substring(0, 2);                         //mm/ddのmmを取得
            string day = today.Substring(3, 2);                           //mm/ddのddを取得
            reg = new Regex($@"(\<th\>{month}月{day}日.*\n\s+.*=.*=)(.*)(\st.*forecast-telop.*)", RegexOptions.None);     //なかなか抽出に至らなかったため、()でグループ化をしてみたら成功
            wc.Encoding = Encoding.UTF8;                                  //Encoding.UTF8でUTF形式にし、文字化けを解消
            Match m = reg.Match(wc.DownloadString(url));
            todayWeather = m.Groups[2].ToString().Replace("\"", "");      //目的の値が入ったグループ2番目を「"」を削って取得　正規表現では\"を上手く扱うことが出来なかったため、こちらで削除
            return todayWeather;
        };
        #endregion
    }
}
