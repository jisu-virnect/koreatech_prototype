
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
    SendMagicNumber = 0,
    PrivateMessage = 10,
    SpaceState = 11,
    // Add more events here
    Install=22,
    Uninstall = 32,
    Checklist=40,
    Checkout = 50,
}

public enum RemoteEventSubIDs : byte
{
    world,
    install,
    before,
    after,
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
    vcam_install,
    vcam_checkout,
}
public enum eActive
{
    idle,
    active,
}
public enum eDBName
{
    db_section,
    db_before_check_environment,
    db_before_safetytools,
    db_work_install,
    db_work_check_install,
    db_after_workscaffold,
    db_after_uninstall,
}
public enum eSectionType
{
    before,
    install,
    after,
}
