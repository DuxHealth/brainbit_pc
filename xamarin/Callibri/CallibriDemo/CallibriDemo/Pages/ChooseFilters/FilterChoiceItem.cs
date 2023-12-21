using System.ComponentModel;
using System.Runtime.CompilerServices;

using Neurotech.Filters;

namespace CallibriDemo.Pages.ChooseFilters;

public class FilterChoiceItem : INotifyPropertyChanged
{
    private string _description = "";

    private bool _isSelected;

    private string      _name = "";
    private IIRFilterParam _param;

    public FilterChoiceItem(IIRFilterParam type)
    {
        Param      = type;
        IsSelected = false;
    }

    public FilterChoiceItem(IIRFilterParam type, bool isSelected)
    {
        Param      = type;
        IsSelected = isSelected;
    }

    public IIRFilterParam Param
    {
        get => _param;

        set
        {
            _param = value;
            Name   = _param.type.ToString();
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;

        set
        {
            _name = value;
            Description = _name
                        + " - "
                        + Param.samplingFreq
                        + " hz"
                        + " - "
                        + Param.cutoffFreq
                        + " hz";
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;

        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;

        set
        {
            _isSelected = value;
            OnPropertyChanged();
        }
    }

#region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
#endregion

    public void OnPropertyChanged([CallerMemberName] string propName = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}
