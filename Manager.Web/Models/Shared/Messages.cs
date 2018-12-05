using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Web.Models.Shared
{
    public class Messages
    {
        public Messages(string _title = "", string _msg = "", bool _isShowCloseBtn = true, TypeAlert _typeAlert = TypeAlert.info)
        {
            this.Title = _title;
            this.Msg = _msg;
            this.IsShowCloseBtn = _isShowCloseBtn;
            this.TypeAlert = _typeAlert;
        }

        public string Title { get; set; }

        public string Msg { get; set; }

        public bool IsShowCloseBtn { get; set; }

        public TypeAlert TypeAlert { get; set; }
    }

    public enum TypeAlert
    {
        success,
        info,
        warning,
        danger
    }
}