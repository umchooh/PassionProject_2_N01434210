﻿using System.Web;
using System.Web.Mvc;

namespace PassionProject_2_N01434210
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
