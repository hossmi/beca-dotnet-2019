using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCarManager.Services;

namespace WebCarManager.Controllers
{
    public abstract class AbstractController : Controller
    {
        private readonly IInstanceProvider instanceProvider;

        public AbstractController() :base()
        {
            this.instanceProvider = DefaultInstanceProvider.Instance;
        }

        protected T getService<T>()
        {
            return this.instanceProvider.get<T>();
        }
    }
}