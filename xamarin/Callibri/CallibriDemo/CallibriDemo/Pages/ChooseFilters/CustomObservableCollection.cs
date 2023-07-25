using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CallibriDemo.Pages.ChooseFilters;

public class CustomObservableCollection<T> : ObservableCollection<T>
{
    private bool _suppressNotification;

    public CustomObservableCollection(IEnumerable<T> collection) : base(collection) { }

    public CustomObservableCollection() { }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!_suppressNotification)
            base.OnCollectionChanged(e);
    }

    public void Refresh() { OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); }

    public void AddRange(IEnumerable<T> data)
    {
        _suppressNotification = true;
        foreach (T item in data) Add(item);

        _suppressNotification = false;
    }
}
