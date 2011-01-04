using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SubSonic.DataProviders
{
    public class SSTransaction //: IDisposable
    {
        //private bool IOwnTransaction = false;

        private object _CurrentHttpContext;
        private object CurrentHttpContext
        {
            get
            {
                if (_CurrentHttpContext == null)
                {
                    string TypeSessionDictionary =
                        "System.Web.HttpContext, " +
                        "System.Web, Version=2.0.0000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

                    Type HttpContextType = Type.GetType(TypeSessionDictionary);

                    _CurrentHttpContext = HttpContextType.GetProperty("Current").GetValue(HttpContextType, null);

                }
                return _CurrentHttpContext;
            }
        }

        private System.Collections.Hashtable _CurrentHttpContextItems;
        private System.Collections.Hashtable CurrentHttpContextItems
        {
            get
            {
                if (_CurrentHttpContextItems == null)
                {


                    _CurrentHttpContextItems = CurrentHttpContext.GetType().GetProperty("Items").GetValue(CurrentHttpContext, null) as System.Collections.Hashtable;

                    return CurrentHttpContextItems;
                }

                return _CurrentHttpContextItems;
                
            }
        }

        public enum eAppType
        {
            Web,
            Forms
        }

        public static eAppType AppType
        {
            get
            {
                // We need to know if the app is web or forms in order to choose between
                //   HTTPContext and CallContext
                if (System.Configuration.ConfigurationManager.AppSettings["AppType"] == null)
                {   // Default to Web
                    return eAppType.Web;
                }
                else
                {
                    string sAppType = System.Configuration.ConfigurationManager.AppSettings["AppType"].ToLower();

                    switch (sAppType)
                    {
                        case "web":
                            return eAppType.Web;

                        case "forms":
                            return eAppType.Forms;

                        case "form":
                            return eAppType.Forms;

                    }

                    // Default to Web
                    return eAppType.Web;
                }
            }
        }

        public  DbCommand GetTrannyCmd(AutomaticConnectionScope scope)
        {
            DbTransaction oTran;

            if (TransRetrieve() != null)
            {
                DbTransaction tran = TransRetrieve();

                DbCommand cmd = scope.Connection.CreateCommand();
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                cmd.Transaction = tran;

                return cmd;
            }
            else
            {
                DbCommand cmd;

                if (scope.IsUsingSharedConnection)
                {
                    oTran = scope.Connection.BeginTransaction();

                    switch (AppType)
                    {
                        case eAppType.Web:
                            //System.Web.HttpContext.Current.Items.Add("SSTrans", oTran);
                            SetTranOnHttpContext(oTran);
                            break;
                        case eAppType.Forms:
                            System.Runtime.Remoting.Messaging.CallContext.SetData("SSTrans", oTran);
                            break;
                    }

                    cmd = scope.Connection.CreateCommand();
                    cmd.Transaction = oTran;
                }
                else
                {
                    cmd = scope.Connection.CreateCommand();
                }
                return cmd;
            }
        }

        public  DbTransaction  GetTranFromHttpContext(string keyName)
        {
            if (CurrentHttpContextItems["SSTrans"] == null)
            {
                return null;
            }
            else
            {
                return (DbTransaction)CurrentHttpContextItems["SSTrans"];   
            }
        }

        public  void RemoveTranFromHttpContext()
        {
            object[] parms = new object[1];
            parms[0] = "SSTrans";

            CurrentHttpContextItems.GetType().GetMethod("Remove").Invoke(CurrentHttpContextItems, parms);

        }

        public  void SetTranOnHttpContext(DbTransaction tran)
        {
            object[] parms = new object[2];
            parms[0] = "SSTrans";
            parms[1] = tran;

            CurrentHttpContextItems.GetType().GetMethod("Add").Invoke(CurrentHttpContextItems, parms);
        }

        public  DbTransaction TransRetrieve()
        {
            DbTransaction tran = null;

            switch (AppType)
            {
                case eAppType.Web:
                    //tran = (DbTransaction)System.Web.HttpContext.Current.Items["SSTrans"];
                    tran = GetTranFromHttpContext("SSTrans");
                    break;
                case eAppType.Forms:
                    tran = (DbTransaction)System.Runtime.Remoting.Messaging.CallContext.GetData("SSTrans");
                    break;
            }

            if (tran == null)
            {
                return null;
            }
            else
            {
                return (DbTransaction)tran;
            }
        }

    }
}
