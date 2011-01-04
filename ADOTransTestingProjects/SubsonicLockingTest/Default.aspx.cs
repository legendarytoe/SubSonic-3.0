using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SubSonic.DataProviders;
using SubsonicLockingUnitTests;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            LockingTest.All().ToList().ForEach(r => r.Delete());

            LockingTest rec = new LockingTest();

            rec.FirstName = "Brett";
            rec.LastName = "Favre";
            rec.Age = 41;
            rec.Save();

            LockingTest lockingTest = LockingTest.All().ToList()[0];

            Session["selectedLockingTest"] = lockingTest;
        }

    }

    protected void btnUpdateRec1_Click(object sender, EventArgs e)
    {
        using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
        {

            //List<LockingTest> allRecs = LockingTest.All().ToList();

            LockingTest lockingTest = (LockingTest)Session["selectedLockingTest"];

            lockingTest.FirstName = "Brettx";

            lockingTest.Save();


            /*
            LockingTest lockingTest = new LockingTest();
            lockingTest.FirstName = "erase";
            lockingTest.LastName = "me";
            lockingTest.Save();
         */

            sharedConnectionScope.Commit();
        }
    }

    protected void btnUpdateRec2_Click(object sender, EventArgs e)
    {
        LockingTest lockingTest = (LockingTest)Session["selectedLockingTest"];

        lockingTest.FirstName = "Bretty";

        lockingTest.Save();

    }

    protected void btnRepull_Click(object sender, EventArgs e)
    {

        LockingTest lockingTest = LockingTest.All().ToList()[0];

        Session["selectedLockingTest"] = lockingTest;
    }
}
