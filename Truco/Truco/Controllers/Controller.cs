using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Truco.Models;

namespace Truco.Controllers
{
    //[Authorize(Roles = "Administradores")]
    public class Controller : System.Web.Mvc.Controller
    {
        protected TrucoDbContext db = new TrucoDbContext();

        protected string PesquisaKey
        {
            get
            {
                string _areaName = RouteData.DataTokens["area"]?.ToString();
                string _controllerName = RouteData.Values["controller"].ToString();
                string _actionName = RouteData.Values["action"].ToString();

                string pesquisaKey = _areaName + _controllerName;
                return pesquisaKey;
            }
        }
    }
}