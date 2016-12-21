namespace FairyZeta
{
    public enum SQLiteOperationResult
    {
        /// <summary> 成功 </summary>
        Success,
        /// <summary> 失敗 </summary>
        Failure,
        /// <summary> 警告終了 </summary>
        Warning,
        /// <summary> 対象が存在している </summary>
        Exists,
        /// <summary> 対象が見つからない </summary>
        NotFound
    }
}
