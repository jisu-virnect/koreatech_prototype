
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
    None = 0,
    Install = 20,
    Uninstall = 30,
    Checklist = 40,
    Checkout = 50,
}