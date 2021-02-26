using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RequestTest1
{
    class Weather
    {
        //<summary>
        //自中央氣象局的Http,抓Xml版本資料(UTF8)
        //</summary>
        //<param name="datasetDesription">標題</param>
        //<param name="location/locationName">地區</param>
        //<param name="weatherElement/time/startTime&endTime">時間</param>
        //<param name="parameterName">天氣</param>
        //<return>抓資料是否成功?true:false</return>
       //※ URL： https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/{dataid}?Authorization={apikey}&format={format}    
       //{dataid} 為各資料集代碼 (參照：資料清單)  ex.F-A0012-001
       //{apikey} 為會員帳號對應之授權碼  ex.CWB-1234ABCD-78EF-GH90-12XY-IJKL12345678
       //{format} 為資料格式，請參照各資料集頁面確認可下載之檔案格式  ex.XML、CAP、JSON、ZIP、KMZ、GRIB2
       //※ 範例：https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/F-A0012-001?Authorization=CWB-1234ABCD-78EF-GH90-12XY-IJKL12345678&format=XML
       //並請加入快取功能，如上述所示。
         
        public static bool getTaiwanData(string strRegion, ref string strWeather, ref string strTemperature, ref string strImgID)
        {
            string url2019 = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-CF46B11A-62D6-464D-A546-872BE0CF81B5&format=XML";
            string url2013= "http://www.cwb.gov.tw/opendata/forecast/wf36hrC.xml";
            try
            {
                DateTime now = DateTime.Now;
                //特定城市
                //string strXml = InternetClient.HttpGet(url2019 + "&locationName=" + strRegion, Encoding.UTF8);
                string strXml = InternetClient.HttpGet(url2019, Encoding.UTF8);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(strXml);
                xml.Save("Info.xml");
                XmlNode Tilte = xml.SelectSingleNode("dataset/records/datasetDescription");
                Console.WriteLine(Tilte.InnerText);
                XmlNodeList xnList = xml.SelectSingleNode("dataset/records").ChildNodes;
                if (xnList.Count < 1) return false;
                foreach (XmlNode xn in xnList)
                {
                    if (xn.Name == "location")
                    {
                        string locationName = xn.SelectSingleNode("locationName").InnerText;
                        XmlNodeList infoList = xn.SelectNodes("weatherElement/time/parameter/parameterName");
                        string weather = infoList[1].InnerText;
                        string PoP = infoList[4].InnerText;
                        string MinT = infoList[7].InnerText;
                        string MaxT = infoList[13].InnerText;
                        Console.WriteLine($"{locationName} 天氣{weather,4} 降雨機率{PoP,3}% 溫度{MinT}℃~{MaxT}℃");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        static void Main(string[] args)
        {
            string Region = "臺中市";
            string strWeather = "";
            string strTemperature = "";
            string strImgID = "";
            if (getTaiwanData(Region, ref strWeather, ref strTemperature, ref strImgID))
            {
                
                Decimal pricePerOunce = 17.36m;
                String s = String.Format("The current price is {0:E2} per ounce.", pricePerOunce);
                Console.WriteLine(s);
            }
            Console.WriteLine("按任意鍵結束....");
            Console.ReadKey();  //可按任意鍵結束畫面
        }
    }
}
