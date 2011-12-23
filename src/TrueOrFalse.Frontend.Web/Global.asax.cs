﻿using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Web.JavascriptView;

namespace TrueOrFalse.Frontend.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Global : HttpApplication
    {
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // das hier später per Konvention, siehe: http://mvccontrib.codeplex.com/SourceControl/changeset/view/351a6de404cb#src%2fMVCContrib%2fSimplyRestful%2fSimplyRestfulRouteHandler.cs
            
            routes.MapRoute("Questions", "Questions", new { controller = "Questions", action = "Questions" });
            routes.MapRoute("Question_Create", "Questions/Create/", new { controller = "EditQuestion", action = "Create" });
            routes.MapRoute("Question_Edit", "Questions/Edit/{id}", new { controller = "EditQuestion", action = "Edit" });
            routes.MapRoute("Question_Delete", "Questions/Delete/{id}", new { controller = "Questions", action = "Delete" });
            routes.MapRoute("Question_Answer", "Questions/Answer/{text}/{id}", new { controller = "AnswerQuestion", action = "Answer" });

            routes.MapRoute("Categories", "Categories", new { controller = "Categories", action = "Categories" });
            routes.MapRoute("Categories_Create", "Categories/Create", new { controller = "EditCategory", action = "Create" });
            routes.MapRoute("Categories_Edit", "Categories/Edit/{id}", new { controller = "EditCategory", action = "Edit" });
            routes.MapRoute("Categories_Delete", "Categories/Delete/{id}", new { controller = "Categories", action = "Delete" });
            routes.MapRoute("Categories_AddSubCategoryRow", "Categories/AddSubCategoryRow", new { controller = "EditCategory", action = "AddSubCategoryRow" });
            routes.MapRoute("Categories_EditSubCategoryItems", "Categories/EditSubCategoryItems/{id}", new { controller = "EditSubCategoryItems", action = "Edit" });
            routes.MapRoute("Categories_AddSubCategoryItemRow", "Categories/EditSubCategoryItems/{id}/Add", new { controller = "EditSubCategoryItems", action = "AddSubCategoryItemRow" });

            routes.MapRoute("Knowledge", "Knowledge/{action}", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("News", "News/{action}", new { controller = "News", action = "News" });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic" });          
            
            routes.MapRoute("ApiExport", "Api/Export/{action}", new { controller = "Export", action="Questions" });
            routes.MapRoute("ApiCategory", "Api/Category/{action}", new { controller = "CategoryApi"});

            routes.MapRoute("User", "User/{name}/{id}", new { controller = "UserProfile", action = "Profile" });

            routes.MapRoute("Default", "{controller}/{action}", new { controller = "Welcome", action = "Welcome", id = "" });
        }

        protected void Application_Start()
        {
            InitializeAutofac();

            AreaRegistration.RegisterAllAreas();
            ViewEngines.Engines.Add(new JavaScriptViewEngine());

            RegisterRoutes(RouteTable.Routes);
            
            GlobalFilters.Filters.Add(new GlobalAuthorizationAttribute());

#if DEBUG
            NHibernateProfiler.Initialize();
#endif
        }

        private void InitializeAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModule<AutofacCoreModule>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}