
public enum SEQUENCE_STATE
{
    BEFORE,
    FOCUS,
    AFTER,
}


public enum eBuildScaffold
{
    stump = 0,
    piiler, //��ħ��
    stump_piller,
    catgut, //�����׹�
    work_scaffolding,
    railing,
    wall_jont,
    ladder, //����Ʈ
    bracing, //������
    net, //��ٸ�
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
