using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CustomExtension;
using System.Collections.Specialized;
using CustomExtension;
using System.Text.RegularExpressions;

namespace MVCExtension
{
    /// <summary>
    /// 根据系统特性，定制的一些web控件
    /// 
    /// Author：Phoenix
    /// </summary>
    public static class HtmlValueExtension
    {
        public static MvcHtmlString Value<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            return Value(html, metadata.PropertyName);
        }

        public static MvcHtmlString Value(this HtmlHelper html, string name)
        {
            string attemptedValue = null;
            ModelState modelState;

            if (html.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                if (modelState.Value != null)
                {
                    attemptedValue = modelState.Value.ConvertTo(typeof(string), null /* culture */).ToString();
                }
            }

            return new MvcHtmlString(attemptedValue ?? Convert.ToString(html.ViewData.Eval(name), CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// 从枚举类型生成Checkbox集合，并将选中结果填入表达式中
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TEnum>>> expression) where TEnum : struct
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = metadata.Model as IEnumerable<TEnum>;


            StringBuilder innerhtml = new StringBuilder();
            foreach (Enum item in Enum.GetValues(typeof(TEnum)))
            {
                bool ischecked = (model == null) ? false : model.Any(x => x.ToString() == item.ToString());

                String input = String.Format(@"<input style='font-size: 12px' name='{0}' id='{1}' type='checkbox' " + ((ischecked) ? "checked='checked'" : "") + "  value={2}>", metadata.PropertyName, metadata.PropertyName, item);
                String display = item.GetEnumDescription();
                display = String.Format("<span style='font-size: 12px'>{0}&nbsp;&nbsp;</span>", display);
                innerhtml.Append(input).Append((display ?? Enum.GetName(typeof(TEnum), item).ToString()));
            }
            return new MvcHtmlString(innerhtml.ToString());

        }

        /// <summary>
        /// 从指定的键值对集合生成Select下拉框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<String, String> dictionary)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = metadata.Model as IEnumerable<String>;

            StringBuilder innerhtml = new StringBuilder();
            int index = 0;
            foreach (var item in dictionary)
            {
                bool ischecked = (model == null) ? false : model.Any(x => x.ToString() == item.Key.ToString());
                String input = String.Format(@"<input style='font-size: 12px' name='{0}' id='{1}' type='checkbox' " + ((ischecked) ? "checked='checked'" : "") + "  value={2}>", metadata.PropertyName, metadata.PropertyName, item.Key);
                String display = String.Format("<span style='font-size: 12px'>{0}&nbsp;&nbsp;</span>", item.Value);
                innerhtml.Append(input).Append((display ?? Enum.GetName(typeof(TEnum), item).ToString()));
                index += 1;
                if (index % 13 == 0 && index < dictionary.Count)
                    innerhtml.Append("<br/>");
                else
                    innerhtml.Append("&nbsp;&nbsp;");
            }
            innerhtml.Append("</select>");
            return new MvcHtmlString(innerhtml.ToString());
        }

        /// <summary>
        /// 从枚举类型生成Radiobutton集合
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression) where TEnum : struct
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = (TEnum)metadata.Model;

            StringBuilder innerhtml = new StringBuilder();
            int index = 0;
            foreach (Enum item in Enum.GetValues(typeof(TEnum)).Cast<Enum>().OrderBy(p => p))
            {
                String inputId = metadata.PropertyName + index;
                Boolean ischecked = model.ToString() == item.ToString();
                String input = String.Format("<input name='{0}' id='{1}' type='radio' " + ((ischecked) ? "checked='checked'" : "") + "  value='{2}'/>", metadata.PropertyName, inputId, item);
                String display = String.Format("<label style='display:inline' for='{0}'>{1}{2}&nbsp;&nbsp;</label>", inputId, input, item.GetEnumDescription());
                innerhtml.Append(display);
                index += 1;
            }
            return new MvcHtmlString(innerhtml.ToString());
        }

        /// <summary>
        /// 从枚举类型生成Select下拉框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        //public static MvcHtmlString SelectFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, List<TEnum> notShowEnums, string style = "", string ClassCss = "")
        //{
        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    var model = (TEnum)metadata.Model;
        //    String propertyName = metadata.PropertyName;

        //    StringBuilder innerhtml = new StringBuilder(String.Format("<select id = '{0}' name = '{1}' style='{2}' class='{3}'>", propertyName, propertyName, style, ClassCss));
        //    innerhtml.Append(string.Format("<option value='{0}' selected='selected'>{1}</option>", EnumHelper.NotLimited, CommonResource.Common.All));

        //    List<Enum> items = Enum.GetValues(typeof(TEnum)).Cast<Enum>().ToList();
        //    if (items.Exists(i => i.IsDefinedAttribute<OrderByAttribute>()))
        //    {
        //        items = items.OrderBy(i => i.GetEnumOrderByValue()).ToList();
        //    }
        //    else
        //    {
        //        items = items.OrderBy(i => i).ToList();
        //    }

        //    foreach (Enum item in items)
        //    {
        //        if (item.IsDefinedAttribute<HiddenInputEnumAttribute>())
        //        {
        //            Object[] objs = typeof(TEnum).GetField(item.ToString()).GetCustomAttributes(typeof(HiddenInputEnumAttribute), false);
        //            HiddenInputTargets DisplayValue = (objs.First() as HiddenInputEnumAttribute).DisplayValue;
        //            if ((DisplayValue & HiddenInputTargets.Select) == HiddenInputTargets.Select)
        //                continue;
        //        }

        //        if (notShowEnums != null)
        //        {
        //            if (notShowEnums.Where(e => e.ToString() == item.ToString()).Count() > 0)
        //                continue;
        //        }

        //        String isSelect = model.ToString() == item.ToString() ? "selected='selected'" : "";
        //        innerhtml.AppendFormat("<option value='{0}' {2}>{1}</option>", item.ToString(), item.GetEnumDescription(), isSelect);
        //    }
        //    innerhtml.Append("</select>");
        //    return new MvcHtmlString(innerhtml.ToString());
        //}

        public static MvcHtmlString SelectFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string style = "", string ClassCss = "")
        {
            return SelectFor<TModel, TEnum>(htmlHelper, expression, null, style, ClassCss);
        }


        /// <summary>
        /// 从指定的键值对集合生成Select下拉框
        /// 若要设置name或id，那么name和id必须都要设置，否则无效
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static MvcHtmlString SelectFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<String, String> dictionary, String style = "", string id = "", string name = "")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = (TEnum)metadata.Model;
            StringBuilder innerhtml = null;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(id))
            {
                String propertyName = metadata.PropertyName;
                id = propertyName;
                name = propertyName;
            }
            innerhtml = new StringBuilder(String.Format("<select id = '{0}' name = '{1}' style='{2}'>", id, name, style));

            foreach (var item in dictionary)
            {
                String isSelect = (null == model) ? "" : model.ToString() == item.Key.ToString() ? "selected='selected'" : "";
                innerhtml.AppendFormat("<option value='{0}' {2}>{1}</option>", item.Key.ToString(), item.Value, isSelect);
            }
            innerhtml.Append("</select>");
            return new MvcHtmlString(innerhtml.ToString());
        }


        #region  分页控件
        /// <summary>
        /// 生成跳转分页链接
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="rootUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, string rootUrl, NameValueCollection parameters) 
        {
            StringBuilder parameterStr = new StringBuilder("");
            foreach (var p in parameters.AllKeys)
            {
                if (!string.IsNullOrEmpty(parameterStr.ToString()))
                    parameterStr.Append("&");
                parameterStr.Append(string.Format("{0}={1}", p, parameters[p]));
            }
            string requestUrl = string.Format("{0}?{1}", rootUrl, parameterStr.ToString());
            string pageRedirectUrlFormat = requestUrl + "&pageIndex={0}";
            return WrapPagerGenerate(htmlHelper, parameters, pageRedirectUrlFormat);
        }


        /// <summary>
        ///  js分页控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="jsMethod"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static MvcHtmlString PagerJSLoad(this HtmlHelper htmlHelper, string jsMethod, NameValueCollection parameters) 
        {
            PagerModel model = (PagerModel)htmlHelper.ViewData.Model;
            StringBuilder parameterStr = new StringBuilder("");
            for (int i = 0; i < parameters.Count; i++)
            {
                if (i != 0)
                    parameterStr.Append(",");
                parameterStr.Append(string.Format("\"{0}\"", parameters[i]));
            }
            string requestUrl = string.Format("javascript:{0}({1},{2});", jsMethod, parameterStr.ToString(), "{0}");
            string pageRedirectUrlFormat = requestUrl;// +"&pageIndex={0}";
            return WrapPagerGenerate(htmlHelper, parameters, pageRedirectUrlFormat);
        }



        private static MvcHtmlString WrapPagerGenerate(HtmlHelper htmlHelper, NameValueCollection parameters, string pageRedirectUrlFormat) 
        {
            PagerModel model = (PagerModel)htmlHelper.ViewData.Model;

            string pagePrevious, pageNext, pageLink;
            if (model.PageIndex > 1)
            {
                string url = string.Format(pageRedirectUrlFormat, model.PageIndex - 1);
                pagePrevious = string.Format("<li ><a href='{0}'>«</a></li>", url);
            }
            else
            {
                pagePrevious = "<li class='disabled'><a href='javascript:void(0);'>«</a></li>";
            }
            if (model.PageIndex < model.TotalPageCount)
            {
                string url = string.Format(pageRedirectUrlFormat, model.PageIndex + 1);
                pageNext = string.Format("<li ><a href='{0}'>»</a></li>", url);
            }
            else
            {
                pageNext = "<li class='disabled'><a href='javascript:void(0);'>»</a></li>";
            }
            //纯数字页码
            string pageText = GetPageLink(model.PageIndex, model.TotalPageCount);
            pageLink = FormatPageLink(pageText, pageRedirectUrlFormat);

            return new MvcHtmlString(pagePrevious + pageLink + pageNext);
        }


        private static string GetPageLink(int PageIndex, int PageMount)
        {
            string pageText = string.Empty;
            if (PageIndex == 0)
            { PageIndex = 1; }
            if (PageMount <= 5)
            {
                for (int page = 1; page <= PageMount; page++)
                {
                    pageText += page.ToString() + ",";
                }
            }
            else
            {
                if (PageMount == 6)
                {
                    pageText = "1,2,3,4,5,6,";
                }
                else if (PageIndex <= 2)
                {
                    pageText = "1,2,3,4,#," + PageMount + ",";
                }
                else if (PageIndex >= PageMount - 4)
                {
                    pageText = string.Format("1,#,{0},{1},{2},{3},{4},", PageMount - 4, PageMount - 3,
                        PageMount - 2, PageMount - 1, PageMount);
                }
                else if (PageIndex <= 4 && PageMount > 6)
                {
                    pageText = string.Format("1,2,3,4,5,6,#,{0},", PageMount);//
                }
                else
                {
                    pageText = string.Format("1,#,{0},{1},{2},{3},{4},#,{5},", PageIndex - 2, PageIndex - 1, PageIndex, PageIndex + 1,
                        PageIndex + 2, PageMount);
                }
            }
            if (PageIndex <= 1)
            {
                pageText = pageText.Substring(1, pageText.Length - 1);
                pageText = "[1]" + pageText;
            }
            else
            {
                pageText = Regex.Replace(pageText, "," + PageIndex.ToString() + ",", "," + "[" + PageIndex + "],");
            }
            return pageText;
        }


        private static string FormatPageLink(string pageText, string pageRedirectUrlFormat)
        {
            pageText = Regex.Replace(pageText, @"\d+,", new MatchEvaluator(match => AddLink(match, pageRedirectUrlFormat)));
            pageText = Regex.Replace(pageText, "#,", "<li class='active'><a href='javascript:void(0);' >...</a></li>");
            pageText = Regex.Replace(pageText, @"\[\d+\],", new MatchEvaluator(AddCurentPageLabel));
            return pageText;
        }


        private static string AddLink(Match m, string urlFormat)
        {
            string Index = m.Value.Replace(",", string.Empty);
            string url = string.Format(urlFormat, Index);
            return string.Format("<li><a href='{0}'>{1}</a></li>", url, Index);
        }



        private static string AddCurentPageLabel(Match m)
        {
            return string.Format("<li class='active'><a href='javascript:void(0);'>{0}</a></li>", Regex.Replace(m.Value, @"[^\d]", string.Empty));
        }

        #endregion
    }
}
