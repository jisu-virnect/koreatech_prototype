
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
    None = 0,
    Install = 20,
    Uninstall = 30,
    Checklist = 40,
    Checkout = 50,
}