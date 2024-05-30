using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager instance;

    public List<CheckEnvironment> CheckEnvironments = new List<CheckEnvironment>();
    public List<SafetyTools> SafetyToolies = new List<SafetyTools>();
    public List<Install> Installs = new List<Install>();
    public List<CheckInstall> CheckInstalls = new List<CheckInstall>();
    public List<Work> Works = new List<Work>();
    public List<Uninstall> Uninstalls = new List<Uninstall>();

    private void Awake()
    {
        instance = this;
        GetDB();
    }

    private void GetDB()
    {
        CheckEnvironments = Util.FromJsonList<CheckEnvironment>(GetText(eDBName.db_before_check_environment.ToString()));
        SafetyToolies = Util.FromJsonList<SafetyTools>(GetText(eDBName.db_before_check_environment.ToString()));
        Installs = Util.FromJsonList<Install>(GetText(eDBName.db_work_install.ToString()));
        CheckInstalls = Util.FromJsonList<CheckInstall>(GetText(eDBName.db_work_check_install.ToString()));
        Works = Util.FromJsonList<Work>(GetText(eDBName.db_after_work.ToString()));
        Uninstalls = Util.FromJsonList<Uninstall>(GetText(eDBName.db_after_uninstall.ToString()));
    }
    private string GetText(string dbname)
    {
       return ResourceManager.instance.LoadData<TextAsset>(dbname).text;
    }
}
