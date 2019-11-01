//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlTools.Utility;
namespace Microsoft.SqlTools.ServiceLayer.ObjectExplorer.SmoModel
{
    /// <summary>
    /// Internal for testing purposes only. This class provides wrapper functionality
    /// over SMO objects in order to facilitate unit testing
    /// </summary>
    internal class SmoWrapper
    {
        public virtual Server CreateServer(ServerConnection serverConn)
        {
            return serverConn == null ? null : new Server(serverConn);
        }

        public virtual bool IsConnectionOpen(SmoObjectBase smoObj)
        {
            try
            {
                SqlSmoObject sqlObj = smoObj as SqlSmoObject;
                bool result = sqlObj != null
                    && sqlObj.ExecutionManager != null
                    && sqlObj.ExecutionManager.ConnectionContext != null
                    && sqlObj.ExecutionManager.ConnectionContext.IsOpen;
                sqlObj.ExecutionManager.ConnectionContext.Connect();

                System.Diagnostics.Debug.WriteLine(sqlObj.ExecutionManager.ConnectionContext.IsOpen);
                var xyz = sqlObj.ExecutionManager.ConnectionContext.SqlConnectionObject.State;
                return result;
            }
            catch (Exception ex)
            {
                Logger.Write(TraceEventType.Information, "Failed to check if connection was open: " + ex.Message);
                return false;
            }
        }

        public virtual void OpenConnection(SmoObjectBase smoObj)
        {
            SqlSmoObject sqlObj = smoObj as SqlSmoObject;
            if (sqlObj != null
                && sqlObj.ExecutionManager != null
                && sqlObj.ExecutionManager.ConnectionContext != null)
            {
                sqlObj.ExecutionManager.ConnectionContext.Connect();
            }
        }
    }
}