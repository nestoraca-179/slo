﻿using System;

namespace SLO
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx");
        }
    }
}