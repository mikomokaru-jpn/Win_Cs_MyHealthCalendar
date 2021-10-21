using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace MyHealthCalendar
{
    public static class HTTPService
    {
        static String url_r10 = "http://192.168.11.3/doc_health_calendar/php/sql_r10.php";
        static String url_r20 = "http://192.168.11.3/doc_health_calendar/php/sql_r20.php"; //日付指定
        static String url_w10 = "http://192.168.11.3/doc_health_calendar/php/sql_w10.php";

        //範囲指定の血圧データ取得
        public static List<Dictionary<string, int>> bpRecordList(int fromYMD, int toYMD)
        {
            var param = String.Format("id=500&from_date={0}&to_date={1}",
                                      fromYMD.ToString(), toYMD.ToString());
            var jsonString = postRequest(url_r10, param);
            try
            {
                return JsonSerializer.Deserialize<List<Dictionary<string, int>>>(jsonString);
            }catch(System.Text.Json.JsonException e)
            {
                Debug.Print(e.ToString());
                return new List<Dictionary<string, int>>();
            }
        }
        //日付指定の血圧データ取得
        public static List<Dictionary<string, int>> bpRecord(int YMD)
        {
            var param = String.Format("id=500&date={0}", YMD.ToString());
            var jsonString = postRequest(url_r20, param);

            var list = JsonSerializer.Deserialize<List<Dictionary<string, int>>>(jsonString);
            return list;
        }

        //血圧データ更新
        static public int bpRecordUpdate(int upperValue, int lowerValue, int YMD, int confirm)
        {
            var param = String.Format("id=500&date={0}&lower={1}&upper={2}&confirm={3}",
                                       YMD.ToString(),
                                       lowerValue.ToString(), upperValue.ToString(),
                                       confirm.ToString());
            var jsonString = postRequest(url_w10, param);
            var list = JsonSerializer.Deserialize<int[]>(jsonString);
            return list[0];
        }
        //HTTPリクエスト＆レスポンス
        static private string postRequest(string url, string param)
        {
            var bodyBytes = Encoding.ASCII.GetBytes(param);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            }
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException exp)
            {
                Debug.WriteLine(exp.ToString());
            }
            return "";
        }

    }
}
