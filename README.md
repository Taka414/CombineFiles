# CombineFiles

あるフォルダ内のすべてのファイルを1つのファイルにまとめるツールです。



## 作成環境

このプロジェクトは以下の環境で作成/動作確認しています。



* VisualStudio2022
* .NET9 + C#13
* Windows11



Windows以外では動作しません。



## 使い方

以下のリンクよりzipをダウンロードし任意のフォルダに解答します。

https://github.com/Taka414/CombineFiles/releases/download/1.0.0/CombineFiles.zip



内容物の「CombineFiles」をコマンドラインから指定し以下のように引数を指定し実行してください。

```
CombineFiles 1 c:\workspace\mycode .cpp;.h;.hpp ignoreDirName1;ignoreDirName2
```



引数の説明

| 引数 | 値の例                        | 説明 |
| ---- | ------------------------------------------------------------ | ---- |
| 1    | 1 |  "1"を固定で指定します。      |
| 2    | c:\workspace\mycode |  まとめたいファイルが格納されるルートフォルダを指定します。    |
| 3    | .cpp;.h;.hpp |  まとめる対象のファイルの拡張子をセミコロンで区切って指定します。大文字/小文字を区別しません。   |
| 4    | ignoreDirName1;ignoreDirName2 | 除外するパターンをセミコロンで区切って指定します。このパターンに一致するフォルダ・ファイル名はまとめる対象から除外されます。 |



このプログラムを実行すると指定しフォルダの親フォルダと同じ階層に、「YYYYMMDDhhmmss.txt」というファイルが作成されエクスプローラーが開きファイルが選択された状態になります。



