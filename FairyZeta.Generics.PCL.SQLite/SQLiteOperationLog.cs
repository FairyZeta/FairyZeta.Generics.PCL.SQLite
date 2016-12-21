using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyZeta
{
    /// <summary> FZ.GN/PCL.SQLite操作ログ
    /// <para> => SQLiteへの操作情報の記録データ </para>
    /// </summary>
    public class SQLiteOperationLog
    {
        /*--- Property/Field Definitions ------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> ログ記録時間
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary> 操作結果
        /// </summary>
        public SQLiteOperationResult SQLiteOperationResult { get; set; }

        /// <summary> ログメッセージ
        /// </summary>
        public string LogMsg { get; set; }

        /*--- Constructers --------------------------------------------------------------------------------------------------------------------------------------------*/
        
        /// <summary> FZ.GN/PCL.SQLite操作ログ/コンストラクタ
        /// </summary>
        /// <param name="pAutoLogTime"> インスタンス生成時にログの時間を自動入力する場合 <code>True</code> </param>
        public SQLiteOperationLog(bool pAutoLogTime = true)
        {
            if (pAutoLogTime)
                this.LogTime = DateTime.Now;
        }

        /*--- Method: Initialization ----------------------------------------------------------------------------------------------------------------------------------*/
        
        /*--- Method: public ------------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary> 簡易フォーマット済のログ出力メッセージを取得します。
        /// </summary>
        /// <param name="pLogFormat"> カスタム形式でログを受け取る場合は出力形式を指定 </param>
        /// <returns> 簡易ログ文字列 </returns>
        public async Task<string> GwtLogFormatAsync(string pLogFormat = "{0} - {1} - {2}")
        {
            return await Task.Run(() => this.GetLogFormat(pLogFormat));
        }

        /// <summary> 簡易フォーマット済のログ出力メッセージを取得します。
        /// </summary>
        /// <param name="pLogFormat"> カスタム形式でログを受け取る場合は出力形式を指定 </param>
        /// <returns> 簡易ログ文字列 </returns>
        public string GetLogFormat(string pLogFormat = "{0} - {1} - {2}")
        {
            return string.Format(pLogFormat, this.LogTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), this.SQLiteOperationResult.ToString(), this.LogMsg);
        }

        /*--- Method: protected ---------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: internal ----------------------------------------------------------------------------------------------------------------------------------------*/
        /*--- Method: private -----------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
