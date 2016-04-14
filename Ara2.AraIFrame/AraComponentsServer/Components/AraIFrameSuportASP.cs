// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Web.UI;

namespace Ara2.Components.IFrame
{
    public static class AraIFrameSuportASP
    {
    //    public static void ActiveAra(System.Web.UI.Page vPage, Action vEventActive)
    //    {
    //        HttpRequest Request = vPage.Request;
    //        HttpResponse Response = vPage.Response;

    //        Ara2.Session Session = Ara2.Sessions.GetSession(vPage, Request.Params["SessionID"]);
    //        Tick Tick = Tick.AddTick(new Tick(Session, vPage, null));

    //        Response.Write(
    //            "<script type='text/javascript'> \n" +
    //            "var Ara = window.parent.Ara; \n"
    //        );

    //        Tick.Script.RumLoad();
    //        AraTools.AsynchronousFunction(vEventActive);
    //        Tick.Script.SendScriptsEnd();
    //        Tick.DellTick(Tick);
    //        Response.Write("</script>\n");
    //    }


        /// <summary>
        /// Verifies that the ARA is active
        /// </summary>
        /// <returns></returns>
        public static bool AraActive()
        {
            try
            {
                return Tick.GetTick() != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Used in pagan ".aspx" calls for AraIFrame to cominicar with AraWindowMain
        /// Warning: Do not use within the ARA.
        /// </summary>
        /// <param name="vData">Srting that will be sent</param>
        /// <param name="vObject">default: window</param>
        public static void postMessage(string vData, string vObject = "window", string targetOrigin="*")
        {
            if (!AraActive())
                HttpContext.Current.Response.Write("<script> " + vObject + ".postMessage('" + AraTools.StringToStringJS(vData) + "','" + AraTools.StringToStringJS(targetOrigin) + "'); </script>");
            else
                throw new Exception("the 'AraIFrameSuportASP.postMessage' method can not be used within the ARA ");
        }
    }
}
