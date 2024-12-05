using System.Collections.ObjectModel;

namespace PasswordMaker;

public class PasswordMakerOptionsCollection : ObservableCollection<PasswordMakerOptions>
{
    public PasswordMakerOptions AddNew(string name)
    {
        PasswordMakerOptions pmo = new() { Name = name };
        Add(pmo);
        return pmo;
    }
}