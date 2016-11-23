using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected TrucoDbContext db = new TrucoDbContext();
    }
}