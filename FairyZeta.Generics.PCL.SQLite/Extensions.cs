using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary> FZ／拡張メソッド
/// </summary>
namespace FairyZeta
{
    /// <summary> TASK拡張クラス
    /// </summary>
    public static class TaskEnumerableExtensions
    {
        public static Task WhenAll(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks);
        }

        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks);
        }
    }
}
