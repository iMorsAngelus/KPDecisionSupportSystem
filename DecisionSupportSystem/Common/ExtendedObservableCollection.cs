﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DecisionSupportSystem.Common
{
    public sealed class ExtendedObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public ExtendedObservableCollection()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        public ExtendedObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems)
                Add(item);
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            if (e.OldItems == null) return;
            {
                foreach (var item in e.OldItems)
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender,
                IndexOf((T)sender));
            OnCollectionChanged(args);
        }
    }
}