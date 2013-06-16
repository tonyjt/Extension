using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCExtension
{
    public static class PageHelper
    {

        public static int[] GetIds(List<int> allIds, int pageIndex, int pageSize)
        {
            return GetIds<int>(allIds, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据页码和页大小从集合中获取当前页内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static T[] GetIds<T>(List<T> allIds, int pageIndex, int pageSize)
        {
            T[] ids;
            int start, end, total, pageCount;
            start = pageIndex * pageSize;
            total = allIds.Count;
            pageCount = total / pageSize;
            if ((total % pageSize) > 0)
                pageCount++;

            if (start > total - 1)
            {
                start = (pageCount - 1) * pageSize;
            }
            if (start < 0)
                start = 0;
            end = start + pageSize - 1;
            if (end > allIds.Count - 1)
                end = allIds.Count - 1;
            ids = new T[end - start + 1];
            allIds.CopyTo(start, ids, 0, ids.Length);

            return ids;
        }


        public static IEnumerable<T> GetDatas<T>(IQueryable<T> query, int pageIndex, int pageSize, out int totalCount)
        {
            IEnumerable<T> list;

            totalCount = query.Count();

            if (totalCount > 0)
            {
                if (pageSize <= 0)
                {
                    list = query.ToList();
                }
                else
                {
                    list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            else
            {
                list = new List<T>();
            }

            return list;
        }

        public static bool CheckPagingParameters(int pageIndex, int pageSize)
        {
            if (pageIndex < 1 || pageSize < 0) return false;
            else return true;
        }
    }
}
