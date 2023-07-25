using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CallibriDemo.Pages.ChooseFilters;

using Neurotech.Filters;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ChooseFiltersPage : ContentPage
{
    private INavigation _navigation;

    private TaskCompletionSource<List<FilterParam>> _taskCompletionSource;

    public ChooseFiltersPage(List<FilterParam> preselectedFilters)
    {
        InitializeComponent();

        var createdFilters = InitFilters();
        SelectFilters(createdFilters, preselectedFilters);
        FiltersList = new CustomObservableCollection<FilterChoiceItem>();
        FiltersList.AddRange(createdFilters);


        BindingContext                 = this;
        filtersListView.BindingContext = this;
    }

    public CustomObservableCollection<FilterChoiceItem> FiltersList { get; set; }

    public INavigation NavigationStack { get => _navigation ?? Navigation; }

    public Task<List<FilterParam>> Show(INavigation navigation)
    {
        _taskCompletionSource?.TrySetResult(null);
        _taskCompletionSource = new TaskCompletionSource<List<FilterParam>>();
        _navigation           = navigation;

        NavigationStack.PushModalAsync(this, false);
        return _taskCompletionSource.Task;
    }

    private List<FilterChoiceItem> InitFilters()
    {
        var filterParams = PreinstalledFilters.List();
        var filters      = new List<FilterChoiceItem>(filterParams.Length);

        filters.AddRange(filterParams.Select(filterParam => new FilterChoiceItem(filterParam)));

        return filters;
    }

    private static void SelectFilters(IReadOnlyCollection<FilterChoiceItem> createdFilters, List<FilterParam> preselectedFilters)
    {
        if (preselectedFilters == null)
            return;
        foreach (FilterParam filterToSelect in preselectedFilters)
        {
            FilterChoiceItem filter = createdFilters.FirstOrDefault(created => { return created.Param.type == filterToSelect.type && created.Param.cutoffFreq == filterToSelect.cutoffFreq && created.Param.samplingFreq == filterToSelect.samplingFreq; });
            if (filter != null)
                filter.IsSelected = true;
        }
    }

    private void FiltersList_ItemTapped(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is FilterChoiceItem item) SelectItem(item);
    }

    private void SelectItem(FilterChoiceItem item)
    {
        if (item.IsSelected)
        {
            item.IsSelected = false;
            OnPropertyChanged(nameof(FiltersList));
        }
        else
        {
            item.IsSelected = true;
            OnPropertyChanged(nameof(FiltersList));
        }
        filtersListView.SelectedItem = null;
    }

    private async void okButton_Clicked(object sender, EventArgs e)
    {
        var chosen = FiltersList.Where(f => f.IsSelected).Select(f => f.Param).ToList();
        await NavigationStack?.PopModalAsync(false);
        _taskCompletionSource?.TrySetResult(chosen);
    }

    protected override bool OnBackButtonPressed() { return true; }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await NavigationStack?.PopModalAsync(false);
        _taskCompletionSource?.TrySetResult(null);
    }
}
