﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CarManagement.Core.Services;
using CarManagement.Services;
using WebCarManager.Services;

namespace WebCarManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IInstanceProvider provider = DefaultInstanceProvider.Instance;
            provider.register<IEnrollmentProvider>(() => new DefaultEnrollmentProvider());
            provider.register<IEnrollmentImporter>(() => provider.get<IEnrollmentProvider>());
            provider.register<IVehicleBuilder>(() => new VehicleBuilder(provider.get<IEnrollmentProvider>()));
            provider.register<IVehicleStorage>(()  => new SqlVehicleStorage(
                ConfigurationManager.AppSettings[CONNECTION_STRING_KEY], 
                provider.get<IVehicleBuilder>()));
        }
    }
}
