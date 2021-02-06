﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Aurora.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            return Redirect("~/swagger");
        }

        public ActionResult Config()
        {
            return Redirect("/.well-known/openid-configuration");
        }
    }
}