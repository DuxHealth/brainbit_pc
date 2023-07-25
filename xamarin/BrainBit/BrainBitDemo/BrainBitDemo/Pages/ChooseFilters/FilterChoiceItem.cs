using System.ComponentModel;
using System.Runtime.CompilerServices;

using Neurotech.Filters;

namespace BrainBitDemo.Pages.ChooseFilters;

public class FilterChoiceItem : INotifyPropertyChanged
{
    private string _description = "";

    public string Description
    {
        get => _description;

        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;

        set
        {
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    private string _name = "";

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

    private FilterParam _param;

    public FilterParam Param
    {
        get => _param;

        set
        {
            _param = value;
            Name   = _param.type.ToString();
            OnPropertyChanged();
        }
    }

    public FilterChoiceItem(FilterParam type)
    {
        Param      = type;
        IsSelected = false;
    }

    public FilterChoiceItem(FilterParam type, bool isSelected)
    {
        Param      = type;
        IsSelected = isSelected;
    }

#region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
#endregion

    public void OnPropertyChanged([CallerMemberName] string propName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
