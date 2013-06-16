using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCExtension
{
    public class PagerModel : IPagerModel
    {
        private int _pageIndex = 1;

        public int DefaultPageSize = 20;

        private int pageSize = 0;
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get
            {
                return this._pageIndex < 1 ? 1 : this._pageIndex;
            }
            set
            {
                this._pageIndex = value;
            }
        }

        private int totalRecordCount;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecordCount
        {
            get
            { return totalRecordCount; }
            set
            {
                totalRecordCount = value;

                totalPageCount = GetTotalPageCount(TotalRecordCount, PageSize);
            }
        }

        private int totalPageCount = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return totalPageCount;
            }
        }


        /// <summary>
        /// 每页多少个
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pageSize == 0)
                {
                    pageSize = this.DefaultPageSize;
                }
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        /// <summary>
        /// 获取页面总数
        /// </summary>
        /// <param name="totalRecordCount"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public int GetTotalPageCount(int totalRecordCount, int pagesize)
        {
            int result = totalRecordCount / pagesize;
            if (0 == result)
                return 1;
            if (totalRecordCount % pagesize > 0)
                return result + 1;
            return result;
        }

        //public abstract NameValueCollection GetRequestParameters { get; }

        /// <summary>
        /// 用于详情返回数据
        /// </summary>
        public string QueryUrl { get; set; }

        public NameValueCollection GetRequestParameters()
        {
            return null;
        }

        public int PageStart
        {
            get
            {
                return pageSize * (PageIndex - 1) + 1;
            }
        }

        public int PageEnd
        {
            get
            {
                return PageIndex < TotalPageCount ? pageSize * PageIndex : TotalRecordCount;
            }
        }
    }


    public interface IPagerModel
    {
        NameValueCollection GetRequestParameters();
    }
}
