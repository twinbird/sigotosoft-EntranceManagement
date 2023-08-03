using System.Collections;
using System.Data;
using System.Data.SQLite;

namespace EntranceManagement
{
    /// <summary>
    /// SQLite3用のADOのラッパークラス
    /// </summary>
    internal class SQLiteADOWrapper : IDisposable
    {
        /// <summary>
        /// SQLite3のデータベースファイルへのパス
        /// </summary>
        private string mDBFilepath;

        /// <summary>
        /// SQLiteの接続オブジェクト
        /// </summary>
        private SQLiteConnection mConn;

        /// <summary>
        /// 接続中のトランザクション
        /// </summary>
        private SQLiteTransaction? mTransaction;

        public SQLiteADOWrapper(string dbFilepath)
        {
            mDBFilepath = dbFilepath;
            mConn = new SQLiteConnection("Data Source=" + mDBFilepath);
            mConn.Open();
        }

        /// <summary>
        /// トランザクションを開始する
        /// </summary>
        public void BeginTransaction()
        {
            mTransaction = mConn.BeginTransaction();
        }

        /// <summary>
        /// トランザクションをコミットする
        /// </summary>
        public void Commit()
        {
            if (mTransaction == null)
            {
                MessageBox.Show("Transaction is not set");
                return;
            }
            mTransaction.Commit();
        }

        /// <summary>
        /// トランザクションをロールバックする
        /// </summary>
        public void Rollback()
        {
            if (mTransaction == null)
            {
                MessageBox.Show("Transaction is not set");
                return;
            }
            mTransaction.Rollback();
        }

        /// <summary>
        /// 結果を返さないSQLを実行
        /// </summary>
        /// <param name="query">実行するクエリ</param>
        /// <returns>成功した場合には結果件数。失敗した場合には-1</returns>
        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, new Dictionary<string, object>());
        }

        /// <summary>
        /// 結果を返さないSQLを実行
        /// </summary>
        /// <param name="query">実行するクエリ</param>
        /// <param name="param">パラメータ</param>
        /// <returns>成功した場合には結果件数。失敗した場合には-1</returns>
        public int ExecuteNonQuery(string query, Dictionary<string, object> param)
        {
            try
            {
                // 接続先を指定
                using (var command = mConn.CreateCommand())
                {
                    // コマンドの実行処理
                    command.CommandText = query;

                    // パラメータの設定
                    foreach (var item in param)
                    {
                        var key = "$" + item.Key;
                        var val = item.Value;
                        command.Parameters.AddWithValue(key, val);
                    }

                    var ret = command.ExecuteNonQuery();

                    return ret;
                }
            }
            catch (Exception ex)
            {
                //例外が発生した時はメッセージボックスを表示
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 結果を返すクエリを実行
        /// </summary>
        /// <param name="query">実行するクエリ</param>
        /// <returns>成功した場合には結果。失敗した場合にはnull</returns>
        public DataTable? ExecuteQuery(string query)
        {
            return ExecuteQuery(query, new Dictionary<string, object>());
        }

        /// <summary>
        /// 結果を返すクエリを実行
        /// </summary>
        /// <param name="query">実行するクエリ</param>
        /// <param name="param">パラメータ</param>
        /// <returns>成功した場合には結果。失敗した場合にはnull</returns>
        public DataTable? ExecuteQuery(string query, Dictionary<string, object> param)
        {
            try
            {
                // 接続先を指定
                using (var command = mConn.CreateCommand())
                {
                    // コマンドの実行処理
                    command.CommandText = query;

                    // パラメータの設定
                    foreach (var item in param)
                    {
                        var key = "$" + item.Key;
                        var val = item.Value;
                        command.Parameters.AddWithValue(key, val);
                    }

                    DataSet ds = new DataSet();
                    var adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(ds);

                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                //例外が発生した時はメッセージボックスを表示
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// IDisposableの実装
        /// </summary>
        public void Dispose()
        {
            if (mConn != null)
            {
                mConn.Close();
                mConn.Dispose();
            }
        }
    }
}
