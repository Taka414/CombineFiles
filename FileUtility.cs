using System.Text;
using System.Text.RegularExpressions;

namespace CombineFiles
{
    public partial class FileUtility
    {
        /// <summary>
        /// 指定したディレクトリ内にあるファイルの全ての拡張子を取得する。
        /// </summary>
        public static IEnumerable<string> GetExtensions(string rootDir)
        {
            Dictionary<string, string> tmpTable = new();
            foreach (var path in Directory.GetFileSystemEntries(rootDir, "*.*", SearchOption.AllDirectories))
            {
                string extension = Path.GetExtension(path).ToLower();
                if (!tmpTable.ContainsKey(extension))
                {
                    tmpTable[extension] = extension;
                }
            }
            return tmpTable.Keys;
        }

        /// <summary>
        /// 指定したフォルダパス以下の対象ファイルを全て一つにまとめます。
        /// </summary>
        public static void PackFiles(string rootDir, string extensions, string ignores, string outFilePath)
        {
            extensions = extensions.ToLower(); // 全て小文字で判定する
            string[] extArray = extensions.Split(';', StringSplitOptions.RemoveEmptyEntries);

            ignores = ignores.ToLower();
            string[] ignoreArray = ignores.Split(';', StringSplitOptions.RemoveEmptyEntries);

            StreamWriter sw = null;

            DirectoryInfo di = new(rootDir);
            foreach (var path in Directory.GetFileSystemEntries(rootDir, "*.*", SearchOption.AllDirectories))
            {
                string fileExt = Path.GetExtension(path).ToLower();
                bool isHit = Array.Exists(extArray, ext => ext == fileExt); // 拡張子でフィルター
                if (!isHit)
                {
                    continue;
                }
                FileInfo fi = new(path);
                string relatibveFilePath = fi.FullName[(di.FullName.Length + 1)..];

                string lr = relatibveFilePath.ToLower();
                bool isIgnore = Array.Exists(ignoreArray, lr.Contains); // 除外パターンでフィルター
                if (isIgnore)
                {
                    continue;
                }

                Console.WriteLine(relatibveFilePath);

                // 出力先のファイルに書き込み
                sw ??= new StreamWriter(outFilePath, false);
                sw.WriteLine("```");
                sw.WriteLine("// " + relatibveFilePath);
                sw.WriteLine();

                // 結果が文字化けしないようにエンコードを簡易判定する
                Encoding enc = EncodeUtil.GetEncoding(File.ReadAllBytes(path));
                enc ??= Encoding.UTF8;

                foreach (var line in File.ReadAllLines(path, enc))
                {
                    string tmpLine = line.Trim(); // 行頭・行末のスペースなどを除去(AIが読むときは必要ない
                    if (tmpLine == "")
                    {
                        continue; // 空行をまとめない
                    }

                    // 複数連続するスペースとタブを一つにまとめる
                    string p = Reg1().Replace(tmpLine, " ");
                    p = Reg2().Replace(p, " ");

                    sw.WriteLine(p);
                }

                sw.WriteLine("```");
                sw.WriteLine();
            }

            sw?.Dispose();
            string body = File.ReadAllText(outFilePath);
            Console.WriteLine($"Length={body.Length}");
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex Reg1();
        [GeneratedRegex(@"\t+")]
        private static partial Regex Reg2();
    }
}
