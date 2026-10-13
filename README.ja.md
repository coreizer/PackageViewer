# PS3 Package Viewer

A PS3 Package Viewer and Extraction

[English](README.md) | **日本語**

PS3 の PKG ファイルを開いて、中身の一覧表示と抽出を行う Windows 用ツールです。

> [!NOTE]
> このツールが復号するのは、PKG 全体にかかっている外側の暗号（AES-128-CTR）だけです。
> 取り出した `EBOOT.BIN` や `*.self` は NPDRM SELF のまま（暗号化された実行ファイル）です。

## 主な機能

- PKG ファイルを開き、フォルダツリーとファイル一覧を表示
  - Name / Size / Type / Offset / Flag を一覧表示
  - ファイルの種類に応じたアイコン表示
  - フォルダのダブルクリック（または Enter）でその階層へ移動
- PKG 全体の抽出（`Tools → Extraction`）
- ファイル一覧の右クリックから、ファイル／フォルダ単体の抽出
- 抽出中の進捗をステータスバーに表示

### 対応している PKG

| 種類 | revision | 状態 |
|---|---|---|
| PS3 Retail（Finalized） | `0x8000` | 対応 |
| PS3 Debug（Non-finalized） | `0x0000` | 実装済み・未検証 |
| PSP / PS Vita | — | 未対応 |

## 動作環境

- Windows 10 / 11
- [.NET 10](https://dotnet.microsoft.com/)（Windows Desktop Runtime）

## ビルド

1. Visual Studio で `PackageViewer.slnx` を開く
2. NuGet パッケージを復元する（ビルド時に自動で復元されます）
3. ビルドして実行する

## プロジェクト構成

```
PackageViewer/
├── Extensions/
│   └── StreamExtensions.cs   … ビッグエンディアンの読み書き（Stream 拡張メソッド）
├── IO/
│   ├── Decoder.cs            … PKG の復号（ヘッダー・アイテム表の解読、AES-CTR / Debug キーストリーム）
│   ├── FileEntry.cs          … アイテム表の 1 件（ItemRecord + 復号したファイル名）
│   ├── FileType.cs           … ファイルの種類（EBOOT / SELF / Directory / Image など）
│   ├── ItemRecord.cs         … PKG_ITEM_RECORD（0x20 バイト）
│   ├── PackageConstants.cs   … PKG 形式の定数（シグネチャなど）
│   ├── PackageHeader.cs      … PKG ヘッダー（0xC0 バイト）の読み書き
│   └── Unpacker.cs           … ディスクへの抽出
├── Package.cs                … 解読したパッケージの情報
├── RuntimeHelpers.cs         … アプリ全体で使う補助（アプリ名、エラーダイアログ）
├── frmMain.cs                … メイン画面
└── frmAbout.cs               … バージョン情報
Resources/
├── appicon.ico
└── icon/                     … ファイル一覧のアイコン
```

## PKG の構造（概要）

| 範囲 | 内容 |
|---|---|
| `0x000`〜`0x0BF` | ヘッダー（マジック `\x7FPKG`、revision、type、データ位置、Content ID、IV など） |
| `0x0C0`〜 | メタデータ（DRM の種類、コンテンツの種類など） |
| `data_offset`〜 | 暗号化されたデータ領域（アイテム表 → ファイル名 → ファイルの中身） |
| 末尾 `0x20` バイト | ファイル全体の SHA-1 |

- すべての値はビッグエンディアンです。
- Retail の PKG は、`AES-128-ECB(NPDRM PKG PS3 AES key, pkg_data_riv + ブロック番号)` をキーストリームとする AES-CTR で暗号化されています。
- 詳細は [PS3 Developer Wiki – PKG files](https://www.psdevwiki.com/ps3/PKG_files) を参照してください。

## 今後の予定

- [ ] PKG の新規作成（CFW 向け Debug 形式）
- [ ] 開いた PKG のファイル差し替え

## クレジット

- **[PS3 Developer Wiki](https://www.psdevwiki.com/ps3/)**
  - [PKG files](https://www.psdevwiki.com/ps3/PKG_files) — PKG のヘッダー、アイテムレコード、メタデータ、暗号化方式の仕様
- **[ByteSize](https://github.com/omar/ByteSize)**（ByteSizeLib） by Omar Khudeira — MIT License
  - ファイルサイズの表示に使用

## ライセンス

[MIT License](LICENSE)

本ソフトウェアは無保証です。使用によって生じたいかなる損害についても、作者は責任を負いません。

---

© 2026 coreizer
