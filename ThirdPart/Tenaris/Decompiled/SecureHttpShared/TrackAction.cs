// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.TrackAction
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using System;

namespace SecureHttpShared
{
  public class TrackAction
  {
    private string _actionName;
    private DateTime _startDate;
    private bool _isRunning;
    public string TimeTrackedInfo;

    public void Start(string actionName)
    {
      this._actionName = actionName;
      this._startDate = DateTime.Now;
      this._isRunning = true;
    }

    public void Stop()
    {
      if (!this._isRunning)
        return;
      try
      {
        this.TimeTrackedInfo += string.Format("{0}={1}ms. ", (object) this._actionName, (object) Math.Round(DateTime.Now.Subtract(this._startDate).TotalMilliseconds));
      }
      catch (Exception ex)
      {
        Logger.LogError("TrackAction.Stop", ex.Message);
      }
      this._isRunning = false;
    }
  }
}
