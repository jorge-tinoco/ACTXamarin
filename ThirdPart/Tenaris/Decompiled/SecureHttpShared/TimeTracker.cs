// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.TimeTracker
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using System;
using System.Collections.Generic;

namespace SecureHttpShared
{
  public class TimeTracker
  {
    private List<TrackAction> _trackedActions;
    private DateTime _startDate;

    public TimeTracker()
    {
      this._trackedActions = new List<TrackAction>();
      this._startDate = DateTime.Now;
    }

    public TrackAction StartTrackAction(string actionName)
    {
      TrackAction trackAction = new TrackAction();
      this._trackedActions.Add(trackAction);
      trackAction.Start(actionName);
      return trackAction;
    }

    public string GetTrackedInfo()
    {
      try
      {
        if (this._trackedActions == null)
          return string.Empty;
        string str = "";
        foreach (TrackAction trackedAction in this._trackedActions)
          str += trackedAction.TimeTrackedInfo;
        return string.Format(" - Execution Time ({0}ms): {1}", (object) Math.Round(DateTime.Now.Subtract(this._startDate).TotalMilliseconds), (object) str);
      }
      catch
      {
        return string.Empty;
      }
      finally
      {
        this._trackedActions = new List<TrackAction>();
        this._startDate = DateTime.Now;
      }
    }
  }
}
