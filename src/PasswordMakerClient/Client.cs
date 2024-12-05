namespace PasswordMakerClient;

public class Client
{
    public Client(PasswordMakerSettings passwordMakerSettings)
    {
        Settings = passwordMakerSettings;
    }

    public PasswordMakerSettings Settings { get; private set; }

    private PasswordMakerVm passwordMakerVm;
    public PasswordMakerVm PasswordMakerVm
    {
        get
        {
            passwordMakerVm ??= new(this);
            return passwordMakerVm;
        }
    }

}
