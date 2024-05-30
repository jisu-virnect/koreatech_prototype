using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9999)]
public class DBManager : MonoBehaviour
{
    public static DBManager instance;

    public List<Section>            Sections            = new List<Section>();

    //setcion1
    public List<CheckEnvironment>   CheckEnvironments   = new List<CheckEnvironment>();
    public List<SafetyTools>        SafetyToolies       = new List<SafetyTools>();

    //section2
    public List<Install>            Installs            = new List<Install>();
    public List<CheckInstall>       CheckInstalls       = new List<CheckInstall>();

    //section3
    public List<WorkScaffold>       WorkScaffolds       = new List<WorkScaffold>();
    public List<Uninstall>          Uninstalls          = new List<Uninstall>();

    private void Awake()
    {
        instance = this;
        GetDB();
    }

    private void GetDB()
    {
        Sections            = Util.FromJsonList<Section>            (GetText(eDBName.db_section.ToString()));

        CheckEnvironments   = Util.FromJsonList<CheckEnvironment>   (GetText(eDBName.db_before_check_environment.ToString()));
        SafetyToolies       = Util.FromJsonList<SafetyTools>        (GetText(eDBName.db_before_check_environment.ToString()));

        Installs            = Util.FromJsonList<Install>            (GetText(eDBName.db_work_install.ToString()));
        CheckInstalls       = Util.FromJsonList<CheckInstall>       (GetText(eDBName.db_work_check_install.ToString()));

        WorkScaffolds       = Util.FromJsonList<WorkScaffold>       (GetText(eDBName.db_after_workscaffold.ToString()));
        Uninstalls          = Util.FromJsonList<Uninstall>          (GetText(eDBName.db_after_uninstall.ToString()));
    }

    private string GetText(string dbname)
    {
        string text = ResourceManager.instance.LoadData<TextAsset>(dbname).text;
        return text;
    }
}
