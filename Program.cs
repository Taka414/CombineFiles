using System.Diagnostics;

namespace CombineFiles
{
    // 
    // 説明:
    // コードを一つのファイルにまとめる
    // Geminiが200万文字まで認識してくれることを期待して特定のフォルダ内にある全てのソースコードを1つのファイルにまとめるツール
    // 
    // Args:
    // [0]: 動作モード, 0: 拡張子を列挙する(args[2]に指定するため) / 1: 指定したファイルにまとめる
    // [1]: 検索対象のフォルダパス
    // [2]: 抽出対象の拡張子リスト(動作モード1のときは必須)
    //      指定例: ".cpp;.h;.hpp" (セミコロン区切りでドットが必要)
    // [3]: 無視パターン(このパターンが含まれるパスは出力対象にしない)
    //      指定例: "ignoreDirName1;ignoreDirName2" (セミコロン区切り)
    // 
    // 使用例:
    // (1)
    // 対象フォルダにどんな拡張子が含まれているのか調べる例
    // 出力をクリップボードにコピーする例
    // xxx.exe 0 c:\xxx\xxx | clip
    //
    // (2)
    // 指定したフォルダ内のファイルを1つにまとめる
    // xxx.exe 1 c:\workspace\sample .cpp;.h;.hpp ignoreDirName1;ignoreDirName2
    // 

    internal class AppMain
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    throw new Exception("動作モードと対象フォルダパスを指定してください。");
                }
                if (!TryGetMode(args[0], out Mode mode))
                {
                    throw new Exception("動作モードを指定してください。args[0]");
                }
                if (!Directory.Exists(args[1]))
                {
                    throw new Exception($"ディレクトリではありません。arg[1]={args[1]}");
                }
                string targetDir = args[1];

                switch (mode)
                {
                    case Mode.ListExtensions:
                    {
                        string allExtensions = string.Join(';', FileUtility.GetExtensions(targetDir));
                        Console.Write(allExtensions);
                        break;
                    }
                    case Mode.PackFiles:
                    {
                        Console.WriteLine("Pack");

                        if (args.Length < 3)
                        {
                            throw new Exception("拡張子リストがありません");
                        }
                        string extensions = args[2];
                        string ignores = args[3];

                        // 出力先のファイルパス
                        DirectoryInfo di = new(targetDir);
                        string outPath = Path.Combine(di.Parent.FullName, $"{DateTime.Now:yyyyMMddHHmmss}.txt");
                        //string outPath = @"c:\temp\temp.txt";

                        FileUtility.PackFiles(targetDir, extensions, ignores, outPath);
                        Process.Start("explorer", $"/select,\"{outPath}\""); // 選択状態でエクスプローラーを開く
                        break;
                    }
                    default:
                    {
                        throw new Exception($"動作モードを指定してください。value={args[2]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました。Message=" + ex.Message);
                DumpHelp();
            }
        }

        /// <summary>
        /// 指定した文字列から動作モードを取得します。
        /// </summary>
        static bool TryGetMode(string value, out Mode mode)
        {
            if (int.TryParse(value, out int result))
            {
                if (result == 0)
                {
                    mode = Mode.ListExtensions;
                    return true;
                }
                else if (result == 1)
                {
                    mode = Mode.PackFiles;
                    return true;
                }
            }
            mode = Mode.Default;
            return false;
        }

        static void DumpHelp()
        {
            Console.WriteLine("説明:");
            Console.WriteLine("コードを一つのファイルにまとめる");
            Console.WriteLine("Geminiが200万文字まで認識してくれることを期待して特定のフォルダ内にある全てのソースコードを1つのファイルにまとめるツール");
            Console.WriteLine("");
            Console.WriteLine("Args:");
            Console.WriteLine("[0]: 動作モード, 0: 拡張子を列挙する(args[2]に指定するため) / 1: 指定したファイルにまとめる");
            Console.WriteLine("[1]: 検索対象のフォルダパス");
            Console.WriteLine("[2]: 抽出対象の拡張子リスト(動作モード1のときは必須)");
            Console.WriteLine("     指定例: \".cpp;.h;.hpp\" (セミコロン区切りでドットが必要)");
            Console.WriteLine("[3]: 無視パターン(このパターンが含まれるパスは出力対象にしない)");
            Console.WriteLine("     指定例: \"OSS;web_client\" (セミコロン区切り)");
            Console.WriteLine("");
            Console.WriteLine("使用例:");
            Console.WriteLine("(1)");
            Console.WriteLine("対象フォルダにどんな拡張子が含まれているのか調べる例");
            Console.WriteLine("出力クリップボードにコピーする例");
            Console.WriteLine("xxx.exe 0 c:\\xxx\\xxx | clip");
            Console.WriteLine("");
            Console.WriteLine("(2)");
            Console.WriteLine("指定したフォルダ内のファイルを1つにまとめる");
            Console.WriteLine("xxx.exe 1 c:\\workspace\\sample .cpp;.h;.hpp OSS;web_client");
        }

        /// <summary>
        /// 動作モードを表します。
        /// </summary>
        enum Mode
        {
            /// <summary>初期値</summary>
            Default = 0,
            /// <summary>拡張子を列挙</summary>
            ListExtensions,
            /// <summary>ファイルをまとめる</summary>
            PackFiles,
        }
    }
}
