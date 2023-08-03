using System.Configuration;
using System.Reflection;

namespace EntranceManagement
{
    /// <summary>
    /// 設定ファイル管理
    /// </summary>
    public class ConfigWrapper
    {
        /// <summary>
        /// データベースファイルのディレクトリへのファイルパスが記述されている設定ファイル(App.config)のキー
        /// </summary>
        const string mAppSettingDatabaseFileDirPathKey = "DatabaseDirPath";
        
        /// <summary>
        /// デフォルトのアプリケーションのベンダーディレクトリ名(%USERPROFILE%\AppData\Local以下のディレクトリ)
        /// </summary>
        const string mDefaultAppAuthorDirName = "sigoto_soft";

        /// <summary>
        /// デフォルトのアプリケーション設定ファイルのディレクトリ名(%USERPROFILE%\AppData\Local\sigoto_soft以下のディレクトリ)
        /// </summary>
        const string mDefaultAppDirName = "EntranceManagement";

        /// <summary>
        /// Excelテンプレートファイルを配置するディレクトリ名
        /// </summary>
        const string mDefaultTemplateDirName = "templates";

        /// <summary>
        /// デフォルトのデータベースファイル名
        /// </summary>
        string mDefaultDatabaseFileName = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="databaseFileName">このアプリケーションのデフォルトデータベースファイル名</param>
        public ConfigWrapper(string databaseFileName)
        {
            mDefaultDatabaseFileName = databaseFileName;
        }

        /// <summary>
        /// データベースファイルへのパスを取得する
        /// </summary>
        /// <returns></returns>
        public string getDBFilePath()
        {
            var dirPath = getDBDirPath();
            var dbpath = Path.Combine(dirPath, mDefaultDatabaseFileName);

            return dbpath;
        }

        /// <summary>
        /// データベースファイルのディレクトリへのパスを取得する
        /// </summary>
        /// <returns></returns>
        public string getDBDirPath()
        {
            var dbpath = ConfigurationManager.AppSettings[mAppSettingDatabaseFileDirPathKey];
            if (dbpath == null || dbpath == "")
            {
                return defaultDBFilePath();
            }
            return dbpath;
        }

        /// <summary>
        /// データベースファイルへのパス設定を初期化する
        /// </summary>
        /// <returns></returns>
        private string defaultDBFilePath()
        {
            // データベースファイルの保存先ディレクトリを作成
            var defaultDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                mDefaultAppAuthorDirName,
                mDefaultAppDirName);
            Directory.CreateDirectory(defaultDirPath);

            return defaultDirPath;
        }

        /// <summary>
        /// Excelテンプレートファイルのディレクトリへのパスを返す
        /// </summary>
        /// <returns></returns>
        public string getExcelTemplateDirPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                mDefaultAppAuthorDirName,
                mDefaultAppDirName,
                mDefaultTemplateDirName);
        }
    }
}
