using System;
using System.Windows.Controls;
using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using robotManager.Products;
using WholesomeDungeons.Bot;
using WholesomeDungeons.Connection;
using WholesomeDungeons.GUI;
using WholesomeDungeons.Helper;
using WholesomeDungeons.States;
using wManager.Plugin;

public class Main : IProduct {
    private static readonly Engine Fsm = new Engine();
    private static bool _isStarted;

    private UserControl1 _settingsUserControl;

    public static string ProductName = "Wholesome Dungeons";

    public bool IsStarted => _isStarted;

    public UserControl Settings {
        get {
            try {
                return _settingsUserControl ?? (_settingsUserControl = new UserControl1());
            } catch (Exception e) {
                Logger.LogError("WholesomeSettings > Main > Settings(): " + e);
            }

            return null;
        }
    }

    public void Initialize() {
        try 
        {
            WholesomeDungeons.Bot.WholesomeDungeonsSettings.Load();
        } 
        catch (Exception e)
        {
            Logger.LogError("Main > Initialize():" + e);
        }
    }

    public void Start() {
        try {
            _isStarted = true;
            if (WholesomeDungeons.Bot.WholesomeDungeons.InitialSetup()) {
                PluginsManager.LoadAllPlugins();
                Logging.Status = "[WholesomeDungeons] Started";
                Logger.Log("Started");
                DungeonLogic.OnMapChanged();
            } else {
                _isStarted = false;
                Logging.Status = "[WholesomeDungeons] Failed to start";
                Logger.Log("Failed to start");

            }
        } catch (Exception e) {
            _isStarted = false;
            Logger.LogError("WholesomeDungeons > Main > Start(): " + e);
        }
    }

    public void Stop() {
        try {
            WholesomeDungeonsSettings.CurrentSetting.BotStopped = true;
            WholesomeDungeonsSettings.CurrentSetting.Save();
            if (WholesomeDungeonsSettings.CurrentSetting.ServerClient)
            Server.StopServer();

            WholesomeDungeons.Bot.WholesomeDungeons.Dispose();
            _isStarted = false;
            PluginsManager.DisposeAllPlugins();
            Logging.Status = "[WholesomeDungeons] Stopped";
            Logger.Log("[WholesomeDungeons] Stopped");
        } catch (Exception e) {
            Logger.LogError("WholesomeDungeons > Main > Stop(): " + e);
        }
    }

    public void Dispose() {
        try { } catch { }
    }
}