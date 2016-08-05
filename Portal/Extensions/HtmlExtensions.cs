using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Portal.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString ActionButton(this HtmlHelper html, string linkText, string action, string controllerName, string iconClass)
        {
            //<a href="/@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText"><i class="@lLink.IconClass"></i><span class="">@lLink.LinkText</span></a>
            var lURL = new UrlHelper(html.ViewContext.RequestContext);

            // build the <span class="">@lLink.LinkText</span> tag
            var lSpanBuilder = new TagBuilder("span");
            lSpanBuilder.MergeAttribute("class", "");
            lSpanBuilder.SetInnerText(linkText);
            string lSpanHtml = lSpanBuilder.ToString(TagRenderMode.Normal);

            // build the <i class="@lLink.IconClass"></i> tag
            var lIconBuilder = new TagBuilder("i");
            lIconBuilder.MergeAttribute("class", iconClass);
            string lIconHtml = lIconBuilder.ToString(TagRenderMode.Normal);

            // build the <a href="@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText">...</a> tag
            var lAnchorBuilder = new TagBuilder("a");
            lAnchorBuilder.MergeAttribute("href", lURL.Action(action, controllerName));
            lAnchorBuilder.InnerHtml = lIconHtml + lSpanHtml; // include the <i> and <span> tags inside
            string lAnchorHtml = lAnchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(lAnchorHtml);
        }

        /// <summary>
        /// 多选框(checkbox)组。
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="groupName">组名,即name属性</param>
        /// <param name="items">具体条目，确保是否选中属性已经赋值</param>
        /// <param name="isReadOnly">是否只读</param>
        /// <param name="groupClz">多选组整体样式</param>
        /// <param name="labelClz">包裹checkbox的label样式</param>
        /// <param name="itemClz">checkbox的样式</param>
        /// <returns></returns>
        public static MvcHtmlString CheckboxGroup(this HtmlHelper helper, string groupName, List<SelectListItem> items, bool isReadOnly = false, string groupClz = "btn-group", string labelClz = "btn btn-default", string itemClz = "")
        {
            //    <div class="btn-group" id="color" data-toggle="buttons">
            //       <label class="btn btn-default">
            //         <input type="radio" class="toggle" value="1"> 红色
            //       </label>
            //       <label class="btn btn-default">
            //         <input type="radio" class="toggle" value="2"> 绿色
            //       </label>
            //       <label class="btn btn-default">
            //         <input type="radio" class="toggle" value="3"> 蓝色
            //       </label>
            //   </div>
            StringBuilder sb = new StringBuilder(500);
            sb.Append(string.Format("<div class=\"{0}\" data-toggle=\"\">", groupClz));
            foreach (var item in items)
            {
                sb.Append(string.Format("<label class=\"{0}\">", labelClz));
                sb.Append(string.Format("  <input type=\"checkbox\" class=\"{0}\" {4} value=\"{1}\" {2} /> {3}", itemClz, item.Value, item.Selected ? "checked" : "", item.Text, isReadOnly ? "disabled" : ""));
                sb.Append("</label>");
            }
            sb.Append(string.Format("<input type=\"hidden\" value=\"\" name=\"{0}\" />", groupName));
            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 多选框(checkbox)组。
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="groupName">组名,即name属性</param>
        /// <param name="items">具体条目，确保是否选中属性已经赋值</param>
        /// <param name="isReadOnly">是否只读</param>
        /// <param name="groupClz">多选组整体样式</param>
        /// <param name="labelClz">包裹checkbox的label样式</param>
        /// <param name="itemClz">checkbox的样式</param>
        /// <returns></returns>
        public static MvcHtmlString RadioGroup(this HtmlHelper helper, string groupName, List<SelectListItem> items, bool isReadOnly = false, string groupClz = "btn-group", string labelClz = "btn btn-default", string itemClz = "")
        {
            //    <div class="btn-group" id="color" data-toggle="buttons">
            //       <label class="btn btn-default">
            //         <input type="radio" class="toggle" value="1"> 红色
            //       </label>
            //       <label class="btn btn-default">
            //         <input type="radio" class="toggle" value="2"> 绿色
            //       </label>
            //       <label class="btn btn-default">
            //         <input type="radio" class="toggle" value="3"> 蓝色
            //       </label>
            //   </div>
            StringBuilder sb = new StringBuilder(500);
            sb.Append(string.Format("<div class=\"{0}\" data-toggle=\"\">", groupClz));
            foreach (var item in items)
            {
                sb.Append(string.Format("<label class=\"{0}\">", labelClz));
                sb.Append(string.Format("  <input type=\"radio\" class=\"{0}\" name=\"{4}\" {5} value=\"{1}\" {2} /> {3}", itemClz, item.Value, item.Selected ? "checked" : "", item.Text, groupName, isReadOnly ? "disabled" : ""));
                sb.Append("</label>");
            }
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 根据枚举值生成下拉列表
        /// </summary>
        /// <typeparam name="T">必须是枚举类型</typeparam>
        /// <param name="helper"></param>
        /// <param name="selectedValue">枚举值在序列中的顺序</param>
        /// <param name="groupName">组名</param>
        /// <param name="defaultOption">默认选项显示内容</param>
        /// <param name="clzs">select的样式</param>
        /// <returns></returns>
        public static MvcHtmlString EnumToDropdownList<T>(this HtmlHelper helper, int selectedValue, string groupName, string clzs = "", string defaultOption = "")
        {
            StringBuilder sb = new StringBuilder(500);

            if (typeof(T).IsEnum)
            {
                var names = Enum.GetNames(typeof(T));
                sb.Append(string.Format("<select id='{2}' name='{0}' class='{1}'>", groupName, clzs, groupName));
                if (!string.IsNullOrEmpty(defaultOption))
                {
                    sb.Append(String.Format("<option value='{0}' >{1}</option>", "-1", defaultOption));
                }
                for (int i = 0; i < names.Length; i++)
                {
                    sb.Append(String.Format("<option value='{0}' {1}>{2}</option>", i, (i == selectedValue ? "selected" : ""), names[i]));
                }
                sb.Append("</select>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 根据枚举值生成下拉列表
        /// </summary>
        /// <typeparam name="T">必须是枚举类型</typeparam>
        /// <param name="helper"></param>
        /// <param name="selectedValue">枚举值在序列中的顺序</param>
        /// <param name="groupName">组名</param>
        /// <param name="defaultOption">默认选项显示内容</param>
        /// <param name="clzs">select的样式</param>
        /// <returns></returns>
        public static MvcHtmlString EnumToDropdownList<T>(this HtmlHelper helper, T selectedValue, string groupName, string clzs = "", string defaultOption = "")
        {
            StringBuilder sb = new StringBuilder(500);

            if (typeof(T).IsEnum)
            {
                var names = Enum.GetNames(typeof(T));
                var selVal = Convert.ToInt32(selectedValue);
                sb.Append(string.Format("<select id='{2}' name='{0}' class='{1}'>", groupName, clzs, groupName));
                if (!string.IsNullOrEmpty(defaultOption))
                {
                    sb.Append(String.Format("<option value='{0}' >{1}</option>", "-1", defaultOption));
                }
                for (int i = 0; i < names.Length; i++)
                {
                    var val = (int)Enum.Parse(typeof(T), names[i]);
                    sb.Append(String.Format("<option value='{0}' {1}>{2}</option>", val, (val == selVal ? "selected" : ""), names[i]));
                }
                sb.Append("</select>");
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}