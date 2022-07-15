using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarabeDataGenerator
{
    class Tools
    {
        //更新スケジュールクラス
        //開始日時,フォルダ
        public class UpdateSchedule
        {
            public DateTime startDate = DateTime.MaxValue;
            public string folder = string.Empty;

            public UpdateSchedule(DateTime start, string dir)
            {
                startDate = start;
                folder = dir;
            }
        }

        //フォルダクラス
        //フォルダNo,問いNo
        public class FolderTbl
        {
            public int fNo = 0;
            public int qNo = 0;

            public FolderTbl(int fno, int qno)
            {
                fNo = fno;
                qNo = qno;
            }
        }


        //フォルダ情報クラス
        //フォルダNo,フォルダ名,かな,英語,補足1,補足2,補足3
        public class FolderInfoTbl
        {
            public int fNo = 0;
            public string name = string.Empty;
            public string kana = string.Empty;
            public string english = string.Empty;
            public string info1 = string.Empty;
            public string info2 = string.Empty;
            public string info3 = string.Empty;

            public FolderInfoTbl(int fno, string n, string k, string e, string i1, string i2, string i3)
            {
                fNo = fno;
                name = n;
                kana = k;
                english = e;
                info1 = i1;
                info2 = i2;
                info3 = i3;
            }
        }

        //問いクラス
        //駅並べ2では補足1は廃線|休止
        //問いNo,作成日,更新日,問い名,かな,英語,補足1,補足2,補足3,url,アイテムリスト<問いアイテムテーブル>
        public class QuestionTbl
        {
            public int qNo = 0;
            public DateTime createdDate = DateTime.MinValue;
            public DateTime updateDate = DateTime.MinValue;
            public string name = string.Empty;
            public string kana = string.Empty;
            public string english = string.Empty;
            public string info1 = string.Empty;
            public string info2 = string.Empty;
            public string info3 = string.Empty;
            public string url = string.Empty;
            public List<QuestionItemTbl> qiList = new List<QuestionItemTbl>();
            //public List<int> qiNoList = new List<int>(); //アプリ側で作成する
        }

        //問いアイテムクラス
        //駅並べ2では補足1は補足、補足2は廃駅|休止、補足3は都道府県名
        //アイテムNo,名称,かな,英語,補足1,補足2,補足3,url
        public class QuestionItemTbl
        {
            public int iNo = 0;
            public string name = string.Empty;
            public string kana = string.Empty;
            public string english = string.Empty;
            public string info1 = string.Empty;
            public string info2 = string.Empty;
            public string info3 = string.Empty;
            public string url = string.Empty;
        }



        public class MessageEventArgs : EventArgs
        {
            public string Message;

            public MessageEventArgs() { }

            public MessageEventArgs(string message)
            {
                Message = message;
            }
        }


        public class MakeJson
        {
            //デリゲート宣言
            public delegate void MessgeEventHandler(object sender, MessageEventArgs e);

            //イベントデリゲート宣言
            public event MessgeEventHandler Message;

            protected virtual void OnMessage(MessageEventArgs e)
            {
                if (Message != null)
                {
                    Message(this, e);
                }
            }

            //更新スケジュール
            public string UpdateSchedule(string jsonName, List<string> file1)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ取得
                var csvData = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (UpdateScheduleCsvCheck(file, foo))
                            {
                                csvData.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = UpdateScheduleJson(csvData);

                return jsonStr;
            }

            //更新スケジュールcsvチェック
            private bool UpdateScheduleCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,開始日時,フォルダ
                string[] foo = SplitCsv(data);
                if (foo.Length < 4 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //開始日時（空文字列は日時最大値としエラーとしない）
                        if (foo[2].Trim() != string.Empty)
                            if (!DateTime.TryParse(foo[2], out _))
                                ok = false;
                        //フォルダ
                        if (foo[3].Trim() == string.Empty)
                            ok = false;
                        else
                        {
                            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
                            if (foo[3].Trim().IndexOfAny(invalidChars) >= 0)
                                ok = false;
                        }
                    }
                }
                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //更新スケジュールjson作成
            private string UpdateScheduleJson(List<string> csvData)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                var dicUs = new SortedDictionary<DateTime, UpdateSchedule>();

                foreach (var item in csvData)
                {
                    //No,有効無効,開始日時,フォルダ
                    string[] csvItem = SplitCsv(item);
                    //有効無効 空文字列はtrueとする
                    var foo = csvItem[1] == string.Empty ? true : bool.Parse(csvItem[1]);
                    if (foo)
                    {
                        DateTime bar = DateTime.MaxValue;
                        if (csvItem[2].Trim() != string.Empty)
                            bar = DateTime.Parse(csvItem[2]);
                        //同じ開始日があった場合は最初が有効
                        if (!dicUs.ContainsKey(bar))
                            dicUs.Add(bar, new UpdateSchedule(bar, csvItem[3]));
                        else
                        {
                            var msg = $"{currMethod} -> エラー -> 同一開始日あり -> {item}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                    }

                }
                //同じフォルダがあるかチェック
                {
                    var foo = new HashSet<string>();
                    foreach (var item in dicUs)
                    {
                        if (foo.Contains(item.Value.folder))
                        {
                            var msg = $"{currMethod} -> エラー -> 同一フォルダあり -> {item.Value.folder}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item.Value.folder);
                    }
                }
                //件数ログ出力
                {
                    var msg = $"{currMethod} -> 更新スケジュール数:{dicUs.Count}";
                    OnMessage(new MessageEventArgs(msg));
                }
                //日付降順にします
                var baz = new List<UpdateSchedule>();
                foreach (var item in dicUs)
                    baz.Add(item.Value);
                baz.Reverse();

                //インデント有り
                string jsonStr = JsonConvert.SerializeObject(baz, Formatting.Indented);
                //インデント無し
                //string jsonStr = JsonConvert.SerializeObject(baz, Formatting.None);

                return jsonStr;
            }

            //駅並べ2フォルダ
            public string Eki2Folder(string jsonName, List<string> file1)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ取得
                var csvData = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (Eki2FolderCsvCheck(file, foo))
                            {
                                csvData.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = Eki2FolderJson(csvData);

                return jsonStr;
            }

            //駅並べ2フォルダcsvチェック
            private bool Eki2FolderCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-999,0-99,0-9,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                //ここでは名称以降未使用
                string[] foo = SplitCsv(data);
                if (foo.Length < 9 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //フォルダNoレベル0-3
                        //0-99以外はエラーただし空文字列は0とする
                        for (int i = 0; i < 4; i++)
                            if (foo[i + 2].Trim() != string.Empty)
                            {
                                int bar = 0;
                                if (int.TryParse(foo[2 + i], out bar))
                                {
                                    if (bar < 0 || bar > 99)
                                        ok = false;
                                }
                                else
                                    ok = false;
                            }
                        //問いNo0-999,0-99,0-9以外エラーただし空文字列は0とする
                        if (foo[6].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[6], out bar))
                            {
                                if (bar < 0 || bar > 999)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[7].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[7], out bar))
                            {
                                if (bar < 0 || bar > 99)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[8].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[8], out bar))
                            {
                                if (bar < 0 || bar > 9)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                    }
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //駅並べ2フォルダjson作成
            private string Eki2FolderJson(List<string> csvData)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                var fList = new List<FolderTbl>();

                foreach (var item in csvData)
                {
                    //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-999,0-99,0-9,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                    //ここでは名称以降未使用
                    string[] csvItem = SplitCsv(item);
                    //有効無効 空文字列はtrueとする
                    var foo = csvItem[1] == string.Empty ? true : bool.Parse(csvItem[1]);
                    if (foo)
                    {
                        //空文字列は0とする
                        int lv0 = csvItem[2].Trim() == string.Empty ? 0 : int.Parse(csvItem[2]) * 1000000;
                        int lv1 = csvItem[3].Trim() == string.Empty ? 0 : int.Parse(csvItem[3]) * 10000;
                        int lv2 = csvItem[4].Trim() == string.Empty ? 0 : int.Parse(csvItem[4]) * 100;
                        int lv3 = csvItem[5].Trim() == string.Empty ? 0 : int.Parse(csvItem[5]);
                        int fNo = lv0 + lv1 + lv2 + lv3;
                        //空文字列は0とする
                        int q3 = csvItem[6].Trim() == string.Empty ? 0 : int.Parse(csvItem[6]) * 1000;
                        int q2 = csvItem[7].Trim() == string.Empty ? 0 : int.Parse(csvItem[7]) * 10;
                        int q1 = csvItem[8].Trim() == string.Empty ? 0 : int.Parse(csvItem[8]);
                        int qNo = q3 + q2 + q1;

                        fList.Add(new FolderTbl(fNo, qNo));
                    }
                }
                //同一フォルダNoチェック
                {
                    var foo = new HashSet<int>();
                    foreach (var item in fList)
                    {
                        if (foo.Contains(item.fNo))
                        {
                            var msg = $"{currMethod} -> エラー -> 同一フォルダNoあり -> {item.fNo}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item.fNo);
                    }
                }
                //件数ログ出力
                {
                    var msg = $"{currMethod} -> フォルダ数:{fList.Count}";
                    OnMessage(new MessageEventArgs(msg));
                }

                //インデント有り
                string jsonStr = JsonConvert.SerializeObject(fList, Formatting.Indented);
                //インデント無し
                //string jsonStr = JsonConvert.SerializeObject(fList, Formatting.None);

                return jsonStr;
            }

            //駅並べ2フォルダ情報
            public string Eki2FolderInfo(string jsonName, List<string> file1)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ取得
                var csvData = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (Eki2FolderInfoCsvCheck(file, foo))
                            {
                                csvData.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = Eki2FolderInfoJson(csvData);

                return jsonStr;
            }

            //駅並べ2フォルダ情報csvチェック
            private bool Eki2FolderInfoCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-999,0-99,0-9,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                string[] foo = SplitCsv(data);
                if (foo.Length < 9 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //フォルダNoレベル0-3
                        //0-99以外はエラーただし空文字列は0とする
                        for (int i = 0; i < 4; i++)
                            if (foo[i + 2].Trim() != string.Empty)
                            {
                                int bar = 0;
                                if (int.TryParse(foo[2 + i], out bar))
                                {
                                    if (bar < 0 || bar > 99)
                                        ok = false;
                                }
                                else
                                    ok = false;
                            }
                        //問いNo0-999,0-99,0-9以外エラーただし空文字列は0とする
                        if (foo[6].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[6], out bar))
                            {
                                if (bar < 0 || bar > 999)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[7].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[7], out bar))
                            {
                                if (bar < 0 || bar > 99)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[8].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[8], out bar))
                            {
                                if (bar < 0 || bar > 9)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        //名称レベル0[9]
                        //名称レベル1[10]
                        //名称レベル2[11]
                        //名称レベル3[12]
                        //かな[13]
                        //英語[14]
                        //補足[15]
                    }
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //駅並べ2フォルダ情報json作成
            private string Eki2FolderInfoJson(List<string> csvData)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                var fiList = new List<FolderInfoTbl>();

                foreach (var item in csvData)
                {
                    //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-999,0-99,0-9,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                    string[] csvItem = SplitCsv(item);
                    //有効無効 空文字列はtrueとする
                    var foo = csvItem[1] == string.Empty ? true : bool.Parse(csvItem[1]);
                    if (foo)
                    {
                        //空文字列は0とする
                        int lv0 = csvItem[2].Trim() == string.Empty ? 0 : int.Parse(csvItem[2]) * 1000000;
                        int lv1 = csvItem[3].Trim() == string.Empty ? 0 : int.Parse(csvItem[3]) * 10000;
                        int lv2 = csvItem[4].Trim() == string.Empty ? 0 : int.Parse(csvItem[4]) * 100;
                        int lv3 = csvItem[5].Trim() == string.Empty ? 0 : int.Parse(csvItem[5]);
                        int fNo = lv0 + lv1 + lv2 + lv3;
                        //空文字列は0とする
                        int q3 = csvItem[6].Trim() == string.Empty ? 0 : int.Parse(csvItem[6]) * 1000;
                        int q2 = csvItem[7].Trim() == string.Empty ? 0 : int.Parse(csvItem[7]) * 10;
                        int q1 = csvItem[8].Trim() == string.Empty ? 0 : int.Parse(csvItem[8]);
                        int qNo = q3 + q2 + q1;

                        //問いNoが0の場合データ作成
                        if (qNo <= 0)
                        {
                            var name = csvItem[9].Trim() + csvItem[10].Trim() + csvItem[11].Trim() + csvItem[12].Trim();
                            fiList.Add(new FolderInfoTbl
                                (fNo, name, csvItem[13].Trim(), csvItem[14].Trim(), csvItem[15].Trim(), string.Empty, string.Empty));
                        }
                    }
                }
                //同一フォルダNoチェック
                {
                    var foo = new HashSet<int>();
                    foreach (var item in fiList)
                    {
                        if (foo.Contains(item.fNo))
                        {
                            var msg = $"{currMethod} -> エラー -> 同一フォルダNoあり -> {item.fNo}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item.fNo);
                    }
                }
                //件数ログ出力
                {
                    var msg = $"{currMethod} -> フォルダ数:{fiList.Count}";
                    OnMessage(new MessageEventArgs(msg));
                }

                //インデント有り
                string jsonStr = JsonConvert.SerializeObject(fiList, Formatting.Indented);
                //インデント無し
                //string jsonStr = JsonConvert.SerializeObject(fList, Formatting.None);

                return jsonStr;
            }

            //駅並べ2問い
            public string Eki2Question(string jsonName, List<string> file1, List<string> file2, bool base64, bool aes32, string aesPass)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ1取得
                var csvData1 = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (Eki2QuestionCsvCheck(file, foo))
                            {
                                csvData1.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //csvデータ2取得
                var csvData2 = new List<string>();
                for (int i = 0; i < file2.Count; i++)
                {
                    var filePath = file2[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (Eki2QuestionItemCsvCheck(file, foo))
                            {
                                csvData2.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = Eki2QuestionJson(csvData1, csvData2, base64, aes32, aesPass);

                return jsonStr;
            }

            //駅並べ2問いcsvチェック
            private bool Eki2QuestionCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,作成日,更新日,事業者名,路線No0-999,0-99,0-9,路線名,かな,英語,補足1（廃線）,補足2,補足3,url
                string[] foo = SplitCsv(data);
                if (foo.Length < 15 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //作成日,更新日 日時として妥当性がなければエラーただし空文字列は20220601とする
                        if (foo[2].Trim() != string.Empty)
                            if (!DateTime.TryParse(foo[2], out _))
                                ok = false;
                        if (foo[3].Trim() != string.Empty)
                            if (!DateTime.TryParse(foo[3], out _))
                                ok = false;
                        //事業者名[4]
                        //路線No0-999,0-99,0-9以外エラーただし空文字列は0とする
                        if (foo[5].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[5], out bar))
                            {
                                if (bar < 0 || bar > 999)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[6].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[6], out bar))
                            {
                                if (bar < 0 || bar > 99)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[7].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[7], out bar))
                            {
                                if (bar < 0 || bar > 9)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        //路線名[8]
                        //かな[9]
                        //英語[10]
                        //補足1（廃線）treu false 以外はそのまま通すのでチェックしない
                        //if (foo[11] != string.Empty && foo[11].ToLower() != "true" && foo[11].ToLower() != "false")
                        //    ok = false;
                        //補足2[12]
                        //補足3[13]
                        //url[14]
                    }
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //駅並べ2問いアイテムcsvチェック
            private bool Eki2QuestionItemCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,路線名,路線No0-999,0-99,0-9,有効無効,駅No0-99,0-9,接続駅No,駅名,かな,英語,補足1,補足2（廃駅）,補足3（都道府県）,url
                string[] foo = SplitCsv(data);
                if (foo.Length < 16 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //路線名[1]
                    //路線No0-999,0-99,0-9以外エラーただし空文字列は0とする
                    if (foo[2].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[2], out bar))
                        {
                            if (bar < 0 || bar > 999)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    if (foo[3].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[3], out bar))
                        {
                            if (bar < 0 || bar > 99)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    if (foo[4].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[4], out bar))
                        {
                            if (bar < 0 || bar > 9)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[5] != string.Empty && foo[5].ToLower() != "true" && foo[5].ToLower() != "false")
                        ok = false;
                    //駅No0-99,0-9以外エラーただし空文字列は0とする
                    if (foo[6].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[6], out bar))
                        {
                            if (bar < 0 || bar > 99)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    if (foo[7].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[7], out bar))
                        {
                            if (bar < 0 || bar > 9)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    //接続駅No[8]
                    //駅名[9]
                    //かな[10]
                    //英語[11]
                    //補足1[12]
                    //補足2（廃駅）treu false 以外はそのまま通すのでチェックしない
                    //if (foo[13] != string.Empty && foo[13].ToLower() != "true" && foo[13].ToLower() != "false")
                    //    ok = false;
                    //補足3[14]
                    //url[15]
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //駅並べ2問いjson作成
            private string Eki2QuestionJson(List<string> csvData1, List<string> csvData2, bool base64, bool aes32, string aesPass)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //問いアイテムdic作成<問いNo,List<問いアイテム>>
                var dicItem = new Dictionary<int, List<QuestionItemTbl>>();
                foreach (var item in csvData2)
                {
                    //No,路線名,路線No0-999,0-99,0-9,有効無効,駅No0-99,0-9,接続駅No,駅名,かな,英語,補足1,補足2（廃線）,補足3（都道府県）,url
                    string[] staItem = SplitCsv(item);

                    int iNo = int.Parse(staItem[2]) * 1000 + int.Parse(staItem[3]) * 10
                        + (staItem[4].Trim() == string.Empty ? 0 : int.Parse(staItem[4]));
                    if (!dicItem.ContainsKey(iNo))
                        dicItem.Add(iNo, new List<QuestionItemTbl>());

                    var foo = staItem[5] == string.Empty ? true : bool.Parse(staItem[5]); //有効無効 空文字列はtrueとする
                    if (foo)
                    {
                        //問いアイテムNo,名称,かな,英語,補足1,補足2（廃駅）,補足3（都道府県）,url
                        var qi = new QuestionItemTbl();
                        qi.iNo = int.Parse(staItem[6]) * 10 + (staItem[7].Trim() == string.Empty ? 0 : int.Parse(staItem[7]));
                        qi.name = staItem[9];
                        qi.kana = staItem[10];
                        qi.english = staItem[11];
                        qi.info1 = staItem[12];
                        //true false 以外はそのまま
                        if (staItem[13].Trim().ToLower() == "true")
                            qi.info2 = "廃駅";
                        else if (staItem[13].Trim().ToLower() == "false")
                            qi.info2 = string.Empty;
                        else
                            qi.info2 = staItem[13];
                        qi.info3 = staItem[14];
                        qi.url = staItem[15];
                        //同じアイテムNoがあっても追加されます
                        dicItem[iNo].Add(qi);
                    }
                }

                //同じ問いアイテムNoがあるかチェック
                foreach (var item in dicItem)
                {
                    var foo = new HashSet<int>();
                    foreach (var item2 in item.Value)
                    {
                        if (foo.Contains(item2.iNo))
                        {
                            var msg = $"{currMethod} -> エラー -> lineNo:{item.Key} 同一問いNoで問いアイテムNo重複 -> {item2.iNo}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item2.iNo);
                    }
                }

                //問い作成
                List<QuestionTbl> qList = new List<QuestionTbl>();
                foreach (var item in csvData1)
                {
                    //No,有効無効,作成日,更新日,事業者名,路線No0-999,0-99,0-9,路線名,かな,英語,補足1（廃線）,補足2,補足3,url
                    string[] lineItem = SplitCsv(item);

                    var foo = lineItem[1] == string.Empty ? true : bool.Parse(lineItem[1]); //有効無効 空文字列はtrueとする
                    if (foo)
                    {
                        //問いNo,作成日,更新日,問い名,かな,英語,補足1,補足2,補足3,url,問いアイテムリスト<問いアイテム>
                        var q = new QuestionTbl();
                        //問いNo
                        int qNo = int.Parse(lineItem[5]) * 1000 + int.Parse(lineItem[6]) * 10
                            + (lineItem[7].Trim() == string.Empty ? 0 : int.Parse(lineItem[7]));

                        //問いアイテムリスト
                        var qiList = new List<QuestionItemTbl>();
                        if (dicItem.ContainsKey(qNo))
                            qiList = new List<QuestionItemTbl>(dicItem[qNo]);
                        else
                        {
                            var msg = $"{currMethod} -> エラー -> qNo:{qNo} 問いアイテムに問いNo無し";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        q.qNo = qNo;
                        if (lineItem[2].Trim() != string.Empty)
                        {
                            if (!DateTime.TryParse(lineItem[2], out q.createdDate))
                            {
                                var msg = $"{currMethod} -> エラー -> 作成日:{lineItem[2]} 作成日に妥当性無し";
                                OnMessage(new MessageEventArgs(msg));
                            }
                        }
                        else
                            q.createdDate = DateTime.Parse("2022-06-01T00:00:00");
                        if (lineItem[3].Trim() != string.Empty)
                        {
                            if (!DateTime.TryParse(lineItem[3], out q.updateDate))
                            {
                                var msg = $"{currMethod} -> エラー -> 更新日:{lineItem[2]} 更新日に妥当性無し";
                                OnMessage(new MessageEventArgs(msg));
                            }
                        }
                        else
                            q.updateDate = DateTime.Parse("2022-06-01T00:00:00");
                        q.name = lineItem[8];
                        q.kana = lineItem[9];
                        q.english = lineItem[10];
                        //true false 以外はそのまま
                        if (lineItem[11].Trim().ToLower() == "true")
                            q.info1 = "廃線";
                        else if (lineItem[11].Trim().ToLower() == "false")
                            q.info1 = string.Empty;
                        else
                            q.info1 = lineItem[11];
                        if (q.info1 != "")
                            q.info1 += " ";
                        q.info1 += lineItem[12]; //補足1と補足2はアプリ側では補足2
                        q.info2 = lineItem[13]; //補足3はアプリ側では補足2
                        q.info3 = ""; //未使用
                        q.url = lineItem[14];
                        q.qiList = new List<QuestionItemTbl>(qiList);

                        qList.Add(q);
                    }

                }

                //件数ログ出力
                {
                    var qCount = qList.Count; //問い数
                    var qiCount = 0; //問いアイテム数
                    var qiListCount = new List<int>(); //問い別アイテム数
                    var qcCount = 0; //廃線数
                    var qicCount = 0; //廃駅数
                    var aicListCount = new List<int>(); //問い別廃駅数
                    foreach (var item in qList)
                    {
                        qiCount += item.qiList.Count;
                        qiListCount.Add(item.qiList.Count);
                        if (item.info1 != string.Empty)
                            qcCount++;
                        var bar = 0;
                        foreach (var item2 in item.qiList)
                        {
                            if (item2.info2 != string.Empty)
                                bar++;
                        }
                        qicCount += bar;
                        aicListCount.Add(bar);
                    }
                    {
                        var msg = $"{currMethod} -> 問い数:{qCount} 問いアイテム数:{qiCount}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    {
                        var foo = string.Empty;
                        foreach (var item in qiListCount)
                            foo += item.ToString() + " ";
                        var msg = $"{currMethod} -> 問い毎アイテム数:{foo.Trim()}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    {
                        var msg = $"{currMethod} -> 廃線数:{qcCount} 廃駅数:{qicCount}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    {
                        var foo = string.Empty;
                        foreach (var item in aicListCount)
                            foo += item.ToString() + " ";
                        var msg = $"{currMethod} -> 問い毎廃駅数:{foo.Trim()}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                string jsonStr = string.Empty;
                if (!base64)
                {
                    //プレーンテキスト
                    jsonStr = JsonConvert.SerializeObject(qList, Formatting.Indented);//インデント有り
                    //jsonStr = JsonConvert.SerializeObject(qList, Formatting.None);//インデント無し
                }
                else if (!aes32)
                {
                    //問い毎にBase64
                    EncryptDecrypt ed = new EncryptDecrypt();
                    List<string> b64List = new List<string>();

                    foreach (var item in qList)
                        b64List.Add(ed.ToBase64(JsonConvert.SerializeObject(item, Formatting.Indented)));

                    jsonStr = JsonConvert.SerializeObject(b64List, Formatting.Indented);
                }
                else
                {
                    //問い毎にAES32
                    //12文字のpassをbase64して16文字にする
                    //iv大文字、key小文字
                    //例 :
                    //iv :EKINARABE002
                    //key:ekinarabe002
                    EncryptDecrypt ed = new EncryptDecrypt();
                    List<string> b64List = new List<string>();

                    //string pass = "Ekinarabe002";
                    string pass = (aesPass + new string('0', 12)).Substring(0, 12);
                    //初期化ベクトル 16文字 1byte=8bit 8bit*16=128bit
                    string iv = ed.ToBase64(pass.ToUpper());
                    //暗号化鍵 16文字 8bit*16文字=128bit
                    string key = ed.ToBase64(pass.ToLower());

                    foreach (var item in qList)
                        b64List.Add(ed.Encrypt(iv, key, JsonConvert.SerializeObject(item, Formatting.Indented)));

                    jsonStr = JsonConvert.SerializeObject(b64List, Formatting.Indented);
                }

                return jsonStr;
            }


            //並べぇフォルダ
            public string NarabeFolder(string jsonName, List<string> file1)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ取得
                var csvData = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (NarabeFolderCsvCheck(file, foo))
                            {
                                csvData.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = NarabeFolderJson(csvData);

                return jsonStr;
            }

            //並べぇフォルダcsvチェック
            private bool NarabeFolderCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-9,0-99,0-999,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                //ここでは名称以降未使用
                string[] foo = SplitCsv(data);
                if (foo.Length < 9 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //フォルダNoレベル0-3
                        //0-99以外はエラーただし空文字列は0とする
                        for (int i = 0; i < 4; i++)
                            if (foo[i + 2].Trim() != string.Empty)
                            {
                                int bar = 0;
                                if (int.TryParse(foo[2 + i], out bar))
                                {
                                    if (bar < 0 || bar > 99)
                                        ok = false;
                                }
                                else
                                    ok = false;
                            }
                        //問いNo0-9,0-99,0-999以外エラーただし空文字列は0とする
                        if (foo[6].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[6], out bar))
                            {
                                if (bar < 0 || bar > 9)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[7].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[7], out bar))
                            {
                                if (bar < 0 || bar > 99)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[8].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[8], out bar))
                            {
                                if (bar < 0 || bar > 999)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                    }
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //並べぇフォルダjson作成
            private string NarabeFolderJson(List<string> csvData)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                var fList = new List<FolderTbl>();

                foreach (var item in csvData)
                {
                    //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-9,0-99,0-999,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                    //ここでは名称以降未使用
                    string[] csvItem = SplitCsv(item);
                    //有効無効 空文字列はtrueとする
                    var foo = csvItem[1] == string.Empty ? true : bool.Parse(csvItem[1]);
                    if (foo)
                    {
                        //空文字列は0とする
                        int lv0 = csvItem[2].Trim() == string.Empty ? 0 : int.Parse(csvItem[2]) * 1000000;
                        int lv1 = csvItem[3].Trim() == string.Empty ? 0 : int.Parse(csvItem[3]) * 10000;
                        int lv2 = csvItem[4].Trim() == string.Empty ? 0 : int.Parse(csvItem[4]) * 100;
                        int lv3 = csvItem[5].Trim() == string.Empty ? 0 : int.Parse(csvItem[5]);
                        int fNo = lv0 + lv1 + lv2 + lv3;
                        //空文字列は0とする
                        int q3 = csvItem[6].Trim() == string.Empty ? 0 : int.Parse(csvItem[6]) * 100000;
                        int q2 = csvItem[7].Trim() == string.Empty ? 0 : int.Parse(csvItem[7]) * 1000;
                        int q1 = csvItem[8].Trim() == string.Empty ? 0 : int.Parse(csvItem[8]);
                        int qNo = q3 + q2 + q1;

                        fList.Add(new FolderTbl(fNo, qNo));
                    }
                }
                //同一フォルダNoチェック
                {
                    var foo = new HashSet<int>();
                    foreach (var item in fList)
                    {
                        if (foo.Contains(item.fNo))
                        {
                            var msg = $"{currMethod} -> エラー -> 同一フォルダNoあり -> {item.fNo}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item.fNo);
                    }
                }
                //件数ログ出力
                {
                    var msg = $"{currMethod} -> フォルダ数:{fList.Count}";
                    OnMessage(new MessageEventArgs(msg));
                }

                //インデント有り
                string jsonStr = JsonConvert.SerializeObject(fList, Formatting.Indented);
                //インデント無し
                //string jsonStr = JsonConvert.SerializeObject(fList, Formatting.None);

                return jsonStr;
            }

            //並べぇフォルダ情報
            public string NarabeFolderInfo(string jsonName, List<string> file1)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ取得
                var csvData = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (NarabeFolderInfoCsvCheck(file, foo))
                            {
                                csvData.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = NarabeFolderInfoJson(csvData);

                return jsonStr;
            }

            //並べぇフォルダ情報csvチェック
            private bool NarabeFolderInfoCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-9,0-99,0-999,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                string[] foo = SplitCsv(data);
                if (foo.Length < 9 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //フォルダNoレベル0-3
                        //0-99以外はエラーただし空文字列は0とする
                        for (int i = 0; i < 4; i++)
                            if (foo[i + 2].Trim() != string.Empty)
                            {
                                int bar = 0;
                                if (int.TryParse(foo[2 + i], out bar))
                                {
                                    if (bar < 0 || bar > 99)
                                        ok = false;
                                }
                                else
                                    ok = false;
                            }
                        //問いNo0-999,0-99,0-9以外エラーただし空文字列は0とする
                        if (foo[6].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[6], out bar))
                            {
                                if (bar < 0 || bar > 9)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[7].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[7], out bar))
                            {
                                if (bar < 0 || bar > 99)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[8].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[8], out bar))
                            {
                                if (bar < 0 || bar > 999)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        //名称レベル0[9]
                        //名称レベル1[10]
                        //名称レベル2[11]
                        //名称レベル3[12]
                        //かな[13]
                        //英語[14]
                        //補足[15]
                    }
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //並べぇフォルダ情報json作成
            private string NarabeFolderInfoJson(List<string> csvData)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                var fiList = new List<FolderInfoTbl>();

                foreach (var item in csvData)
                {
                    //No,有効無効,フォルダNoレベル0,レベル1,レベル2,レベル3,問いNo0-9,0-99,0-999,名称レベル0,レベル1,レベル2,レベル3,かな,英語,補足
                    string[] csvItem = SplitCsv(item);
                    //有効無効 空文字列はtrueとする
                    var foo = csvItem[1] == string.Empty ? true : bool.Parse(csvItem[1]);
                    if (foo)
                    {
                        //空文字列は0とする
                        int lv0 = csvItem[2].Trim() == string.Empty ? 0 : int.Parse(csvItem[2]) * 1000000;
                        int lv1 = csvItem[3].Trim() == string.Empty ? 0 : int.Parse(csvItem[3]) * 10000;
                        int lv2 = csvItem[4].Trim() == string.Empty ? 0 : int.Parse(csvItem[4]) * 100;
                        int lv3 = csvItem[5].Trim() == string.Empty ? 0 : int.Parse(csvItem[5]);
                        int fNo = lv0 + lv1 + lv2 + lv3;
                        //空文字列は0とする
                        int q3 = csvItem[6].Trim() == string.Empty ? 0 : int.Parse(csvItem[6]) * 100000;
                        int q2 = csvItem[7].Trim() == string.Empty ? 0 : int.Parse(csvItem[7]) * 1000;
                        int q1 = csvItem[8].Trim() == string.Empty ? 0 : int.Parse(csvItem[8]);
                        int qNo = q3 + q2 + q1;

                        //問いNoが0の場合データ作成
                        if (qNo <= 0)
                        {
                            var name = csvItem[9].Trim() + csvItem[10].Trim() + csvItem[11].Trim() + csvItem[12].Trim();
                            fiList.Add(new FolderInfoTbl
                                (fNo, name, csvItem[13].Trim(), csvItem[14].Trim(), csvItem[15].Trim(), string.Empty, string.Empty));
                        }
                    }
                }
                //同一フォルダNoチェック
                {
                    var foo = new HashSet<int>();
                    foreach (var item in fiList)
                    {
                        if (foo.Contains(item.fNo))
                        {
                            var msg = $"{currMethod} -> エラー -> 同一フォルダNoあり -> {item.fNo}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item.fNo);
                    }
                }
                //件数ログ出力
                {
                    var msg = $"{currMethod} -> フォルダ数:{fiList.Count}";
                    OnMessage(new MessageEventArgs(msg));
                }

                //インデント有り
                string jsonStr = JsonConvert.SerializeObject(fiList, Formatting.Indented);
                //インデント無し
                //string jsonStr = JsonConvert.SerializeObject(fList, Formatting.None);

                return jsonStr;
            }

            //並べぇ問い
            public string NarabeQuestion(string jsonName, List<string> file1, List<string> file2, bool base64, bool aes32, string aesPass)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //csvデータ1取得
                var csvData1 = new List<string>();
                for (int i = 0; i < file1.Count; i++)
                {
                    var filePath = file1[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (NarabeQuestionCsvCheck(file, foo))
                            {
                                csvData1.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //csvデータ2取得
                var csvData2 = new List<string>();
                for (int i = 0; i < file2.Count; i++)
                {
                    var filePath = file2[i];
                    if (System.IO.File.Exists(filePath))
                    {
                        var ok = 0;
                        var ng = 0;

                        string file = System.IO.Path.GetFileName(filePath);
                        StreamReader sr = new StreamReader(filePath);
                        while (!sr.EndOfStream)
                        {
                            string foo = sr.ReadLine();
                            if (NarabeQuestionItemCsvCheck(file, foo))
                            {
                                csvData2.Add(foo);
                                ok++;
                            }
                            else
                            {
                                ng++;
                            }
                        }

                        var msg = $"{currMethod} -> 正常{ok}件 エラー{ng}件 -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    else
                    {
                        var msg = $"{currMethod} -> No file -> {filePath}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                //json作成
                var jsonStr = NarabeQuestionJson(csvData1, csvData2, base64, aes32, aesPass);

                return jsonStr;
            }

            //並べぇ問いcsvチェック
            private bool NarabeQuestionCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,有効無効,作成日,更新日,分類名,問いNo0-9,0-99,0-999,問い名,かな,英語,補足1,補足2,補足3,url
                string[] foo = SplitCsv(data);
                if (foo.Length < 15 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[1] != string.Empty && foo[1].ToLower() != "true" && foo[1].ToLower() != "false")
                        ok = false;
                    else if (foo[1].ToLower() != "false")
                    {
                        //作成日,更新日 日時として妥当性がなければエラーただし空文字列は20220601とする
                        if (foo[2].Trim() != string.Empty)
                            if (!DateTime.TryParse(foo[2], out _))
                                ok = false;
                        if (foo[3].Trim() != string.Empty)
                            if (!DateTime.TryParse(foo[3], out _))
                                ok = false;
                        //分類名[4]
                        //問いNo0-9,0-99,0-999以外エラーただし空文字列は0とする
                        if (foo[5].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[5], out bar))
                            {
                                if (bar < 0 || bar > 9)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[6].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[6], out bar))
                            {
                                if (bar < 0 || bar > 99)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        if (foo[7].Trim() != string.Empty)
                        {
                            int bar = 0;
                            if (int.TryParse(foo[7], out bar))
                            {
                                if (bar < 0 || bar > 999)
                                    ok = false;
                            }
                            else
                                ok = false;
                        }
                        //問い名[8]
                        //かな[9]
                        //英語[10]
                        //補足1[11]
                        //補足2[12]
                        //補足3[13]
                        //url[14]
                    }
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //並べぇ問いアイテムcsvチェック
            private bool NarabeQuestionItemCsvCheck(string file, string data)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                bool ok = true;
                //No,問い名,問いNo0-9,0-99,0-999,有効無効,アイテムNo0-99,0-9,アイテム名,かな,英語,補足1,補足2,補足3,url
                string[] foo = SplitCsv(data);
                if (foo.Length < 15 || !int.TryParse(foo[0], out _))
                {
                    //必要項目数以下、Noが数値以外は無効な行と判断
                    ok = false;
                }
                else
                {
                    //問い名[1]
                    //問いNo0-9,0-99,0-999以外エラー
                    //if (foo[2].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[2], out bar))
                        {
                            if (bar < 0 || bar > 9)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    //if (foo[3].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[3], out bar))
                        {
                            if (bar < 0 || bar > 99)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    //if (foo[4].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[4], out bar))
                        {
                            if (bar < 0 || bar > 999)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    //有効無効 空文字列（trueと判断）true false 以外エラー 
                    if (foo[5] != string.Empty && foo[5].ToLower() != "true" && foo[5].ToLower() != "false")
                        ok = false;
                    //アイテムNo0-99,0-9以外エラーただし空文字列は0とする
                    if (foo[6].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[6], out bar))
                        {
                            if (bar < 0 || bar > 99)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    if (foo[7].Trim() != string.Empty)
                    {
                        int bar = 0;
                        if (int.TryParse(foo[7], out bar))
                        {
                            if (bar < 0 || bar > 9)
                                ok = false;
                        }
                        else
                            ok = false;
                    }
                    //アイテム名[8]
                    //かな[9]
                    //英語[10]
                    //補足1[11]
                    //補足2[12]
                    //補足3[13]
                    //url[14]
                }

                if (!ok)
                {
                    var msg = $"{currMethod} -> エラー -> {file} -> {data}";
                    OnMessage(new MessageEventArgs(msg));
                }

                return ok;
            }

            //並べぇ問いjson作成
            private string NarabeQuestionJson(List<string> csvData1, List<string> csvData2, bool base64, bool aes32, string aesPass)
            {
                var currMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

                //問いアイテムdic作成<問いNo,List<問いアイテム>>
                var dicItem = new Dictionary<int, List<QuestionItemTbl>>();
                foreach (var item in csvData2)
                {
                    //No,問い名,問いNo0-9,0-99,0-999,有効無効,アイテムNo0-99,0-9,アイテム名,かな,英語,補足1,補足2,補足3,url
                    string[] staItem = SplitCsv(item);

                    int iNo = int.Parse(staItem[2]) * 100000 + int.Parse(staItem[3]) * 1000
                        + (staItem[4].Trim() == string.Empty ? 0 : int.Parse(staItem[4]));
                    if (!dicItem.ContainsKey(iNo))
                        dicItem.Add(iNo, new List<QuestionItemTbl>());

                    var foo = staItem[5] == string.Empty ? true : bool.Parse(staItem[5]); //有効無効 空文字列はtrueとする
                    if (foo)
                    {
                        //問いアイテムNo,名称,かな,英語,補足1,補足2,補足3,url
                        var qi = new QuestionItemTbl();
                        qi.iNo = int.Parse(staItem[6]) * 10 + (staItem[7].Trim() == string.Empty ? 0 : int.Parse(staItem[7]));
                        qi.name = staItem[8];
                        qi.kana = staItem[9];
                        qi.english = staItem[10];
                        qi.info1 = staItem[11];
                        qi.info2 = staItem[12];
                        qi.info3 = staItem[13];
                        qi.url = staItem[14];
                        //同じアイテムNoがあっても追加されます
                        dicItem[iNo].Add(qi);
                    }
                }

                //同じ問いアイテムNoがあるかチェック
                foreach (var item in dicItem)
                {
                    var foo = new HashSet<int>();
                    foreach (var item2 in item.Value)
                    {
                        if (foo.Contains(item2.iNo))
                        {
                            var msg = $"{currMethod} -> エラー -> lineNo:{item.Key} 同一問いNoで問いアイテムNo重複 -> {item2.iNo}";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        foo.Add(item2.iNo);
                    }
                }

                //問い作成
                List<QuestionTbl> qList = new List<QuestionTbl>();
                foreach (var item in csvData1)
                {
                    //No,有効無効,作成日,更新日,分類名,問いNo0-9,0-99,0-999,問い名,かな,英語,補足1,補足2,補足3,url
                    string[] lineItem = SplitCsv(item);

                    var foo = lineItem[1] == string.Empty ? true : bool.Parse(lineItem[1]); //有効無効 空文字列はtrueとする
                    if (foo)
                    {
                        //問いNo,作成日,更新日,問い名,かな,英語,補足1,補足2,補足3,url,問いアイテムリスト<問いアイテム>
                        var q = new QuestionTbl();
                        //問いNo
                        int qNo = int.Parse(lineItem[5]) * 100000 + int.Parse(lineItem[6]) * 1000
                            + (lineItem[7].Trim() == string.Empty ? 0 : int.Parse(lineItem[7]));

                        //問いアイテムリスト
                        var qiList = new List<QuestionItemTbl>();
                        if (dicItem.ContainsKey(qNo))
                            qiList = new List<QuestionItemTbl>(dicItem[qNo]);
                        else
                        {
                            var msg = $"{currMethod} -> エラー -> qNo:{qNo} 問いアイテムに問いNo無し";
                            OnMessage(new MessageEventArgs(msg));
                        }
                        q.qNo = qNo;
                        if (lineItem[2].Trim() != string.Empty)
                        {
                            if (!DateTime.TryParse(lineItem[2], out q.createdDate))
                            {
                                var msg = $"{currMethod} -> エラー -> 作成日:{lineItem[2]} 作成日に妥当性無し";
                                OnMessage(new MessageEventArgs(msg));
                            }
                        }
                        else
                            q.createdDate = DateTime.Parse("2022-06-01T00:00:00");
                        if (lineItem[3].Trim() != string.Empty)
                        {
                            if (!DateTime.TryParse(lineItem[3], out q.updateDate))
                            {
                                var msg = $"{currMethod} -> エラー -> 更新日:{lineItem[2]} 更新日に妥当性無し";
                                OnMessage(new MessageEventArgs(msg));
                            }
                        }
                        else
                            q.updateDate = DateTime.Parse("2022-06-01T00:00:00");
                        q.name = lineItem[8];
                        q.kana = lineItem[9];
                        q.english = lineItem[10];
                        q.info1 = lineItem[11];
                        if (q.info1 != "")
                            q.info1 += " ";
                        q.info1 += lineItem[12]; //補足1と補足2はアプリ側では補足2
                        q.info2 = lineItem[13]; //補足3はアプリ側では補足2
                        q.info3 = ""; //未使用
                        q.url = lineItem[14];
                        q.qiList = new List<QuestionItemTbl>(qiList);

                        qList.Add(q);
                    }

                }

                //件数ログ出力
                {
                    var qCount = qList.Count; //問い数
                    var qiCount = 0; //問いアイテム数
                    var qiListCount = new List<int>(); //問い別アイテム数
                    foreach (var item in qList)
                    {
                        qiCount += item.qiList.Count;
                        qiListCount.Add(item.qiList.Count);
                    }
                    {
                        var msg = $"{currMethod} -> 問い数:{qCount} 問いアイテム数:{qiCount}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                    {
                        var foo = string.Empty;
                        foreach (var item in qiListCount)
                            foo += item.ToString() + " ";
                        var msg = $"{currMethod} -> 問い毎アイテム数:{foo.Trim()}";
                        OnMessage(new MessageEventArgs(msg));
                    }
                }

                string jsonStr = string.Empty;
                if (!base64)
                {
                    //プレーンテキスト
                    jsonStr = JsonConvert.SerializeObject(qList, Formatting.Indented);//インデント有り
                    //jsonStr = JsonConvert.SerializeObject(qList, Formatting.None);//インデント無し
                }
                else if (!aes32)
                {
                    //問い毎にBase64
                    EncryptDecrypt ed = new EncryptDecrypt();
                    List<string> b64List = new List<string>();

                    foreach (var item in qList)
                        b64List.Add(ed.ToBase64(JsonConvert.SerializeObject(item, Formatting.Indented)));

                    jsonStr = JsonConvert.SerializeObject(b64List, Formatting.Indented);
                }
                else
                {
                    //問い毎にAES32
                    //12文字のpassをbase64して16文字にする
                    //iv大文字、key小文字
                    //例 :
                    //iv :EKINARABE002
                    //key:ekinarabe002
                    EncryptDecrypt ed = new EncryptDecrypt();
                    List<string> b64List = new List<string>();

                    //string pass = "Ekinarabe002";
                    string pass = (aesPass + new string('0', 12)).Substring(0, 12);
                    //初期化ベクトル 16文字 1byte=8bit 8bit*16=128bit
                    string iv = ed.ToBase64(pass.ToUpper());
                    //暗号化鍵 16文字 8bit*16文字=128bit
                    string key = ed.ToBase64(pass.ToLower());

                    foreach (var item in qList)
                        b64List.Add(ed.Encrypt(iv, key, JsonConvert.SerializeObject(item, Formatting.Indented)));

                    jsonStr = JsonConvert.SerializeObject(b64List, Formatting.Indented);
                }

                return jsonStr;
            }

            private string[] SplitCsv(string line)
            {
                //参考 https://www.sejuku.net/blog/85579

                //読み込んだ1行をカンマ毎に分けて配列に格納する
                string[] values = line.Split(',');

                //配列からリストに格納する
                List<string> lists = new List<string>();
                lists.AddRange(values);

                //項目分繰り返す
                for (int i = 0; i < lists.Count; ++i)
                {
                    //1文字の半角スペースか1文字の全角スペースの場合はそのまま
                    if (lists[i] != " " && lists[i] != "　")
                        //先頭のスペースを除去して、ダブルクォーテーションが入っていないか判定する
                        if (lists[i] != string.Empty && lists[i].TrimStart()[0] == '"')
                        {
                            //もう1回ダブルクォーテーションが出てくるまで要素を結合
                            while (lists[i].TrimEnd()[lists[i].TrimEnd().Length - 1] != '"')
                            {
                                lists[i] = lists[i] + "," + lists[i + 1];

                                //結合したら要素を削除する
                                lists.RemoveAt(i + 1);
                            }
                        }
                }

                //項目の前後のダブルクォーテーションは取る
                for (int i = 0; i < lists.Count; ++i)
                    lists[i] = lists[i].Trim('"');

                return lists.ToArray();
            }

            //private string[] SplitCsv(string line)
            //{
            //    //参考 https://www.sejuku.net/blog/85579

            //    //読み込んだ1行をカンマ毎に分けて配列に格納する
            //    string[] values = line.Split(',');

            //    //配列からリストに格納する
            //    List<string> lists = new List<string>();
            //    lists.AddRange(values);

            //    //項目分繰り返す
            //    for (int i = 0; i < lists.Count; ++i)
            //    {
            //        //先頭のスペースを除去して、ダブルクォーテーションが入っていないか判定する
            //        if (lists[i] != string.Empty && lists[i].TrimStart()[0] == '"')
            //        {
            //            //もう1回ダブルクォーテーションが出てくるまで要素を結合
            //            while (lists[i].TrimEnd()[lists[i].TrimEnd().Length - 1] != '"')
            //            {
            //                lists[i] = lists[i] + "," + lists[i + 1];

            //                //結合したら要素を削除する
            //                lists.RemoveAt(i + 1);
            //            }
            //        }
            //    }

            //    //項目の前後のダブルクォーテーションは取る
            //    for (int i = 0; i < lists.Count; ++i)
            //        lists[i] = lists[i].Trim('"');

            //    return lists.ToArray();
            //}


        }



    }
}
