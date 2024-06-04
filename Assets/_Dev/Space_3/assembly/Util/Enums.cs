using SpatialSys.UnitySDK;
using UnityEngine;
public enum SEQUENCE_STATE
{
    BEFORE,
    FOCUS,
    AFTER,
}


public enum eBuildScaffold
{
    stump = 0,
    piiler, //받침판
    stump_piller,
    catgut, //안전그물
    work_scaffolding,
    railing,
    wall_jont,
    ladder, //조인트
    bracing, //파이프
    net, //사다리
    //wall,
}

public enum RemoteEventIDs : byte
{
    //기본 spatial eventId
    SendMagicNumber = 0,
    PrivateMessage = 1,

    //custom eventId
    //내가 어떤 섹션에 있는지?
    SpaceState,

    //섹션별로
    CheckEnvironment,
    SafetyTools,
    Install,
    CheckInstall,
    Work,
    Uninstall,
}

public enum RemoteEventSubIDs_Space : byte
{
    before=0,
    install=1,
    after=2,
    world=10,
}
public enum BlendMode
{
    Opaque = 0,
    Cutout,
    Fade,
    Transparent
}

public enum eServiceMode
{
    None = 0,
    Network,
    Camera,
    Input,
    UI,
}

public enum ePanelAnimation
{
    None,
    Left,
    Right,
    Top, 
    Bottom,
}

public enum eVirtualCameraState
{
    none,
    vcam_before,
    vcam_install,
    vcam_after,
    vcam_안전모,
    vcam_보안경,
    vcam_안전대,
    vcam_안전화,
}
public enum eActive
{
    idle,
    active,
}
public enum eDBName
{
    db_section,
    db_check_environment,
    db_safetytools,
    db_install,
    db_check_install,
    db_workscaffold,
    db_uninstall,
}
public enum eSectionType
{
    before,
    install,
    after,
}
