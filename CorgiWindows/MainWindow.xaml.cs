using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CorgiWindows
{
    public sealed partial class MainWindow : Window
    {
        private readonly WebsiteBlockerModel _model = new();
        private readonly FocusSession _focusSession;

        public ObservableCollection<BlockedWebsite> BlockedSites { get; } = new();

        public MainWindow()
        {
            this.InitializeComponent();
            _focusSession = new FocusSession(_model, false);
            SeedSampleData();
            FocusToggle.IsOn = _focusSession.IsActive;
        }

        private void SeedSampleData()
        {
            if (!_model.BlockedWebsites.Any())
            {
                _model.AddBlockedWebsite("youtube");
                _model.AddBlockedWebsite("facebook");
            }

            foreach (var site in _model.BlockedWebsites)
            {
                BlockedSites.Add(site);
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button)?.Tag as string;
            
            FocusView.Visibility = Visibility.Collapsed;
            BlockerView.Visibility = Visibility.Collapsed;
            AboutView.Visibility = Visibility.Collapsed;

            switch (tag)
            {
                case "block":
                    BlockerView.Visibility = Visibility.Visible;
                    break;
                case "about":
                    AboutView.Visibility = Visibility.Visible;
                    break;
                case "focus":
                default:
                    FocusView.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void FocusToggle_Toggled(object sender, RoutedEventArgs e)
        {
            _focusSession.StartStop();
            FocusToggle.IsOn = _focusSession.IsActive;
        }

        private void AddWebsite_Click(object sender, RoutedEventArgs e)
        {
            var url = NewWebsiteTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            if (BlockedSites.Any(w => w.Url.Equals(url, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var site = new BlockedWebsite
            {
                Id = Guid.NewGuid(),
                Url = url
            };

            BlockedSites.Add(site);
            _model.BlockedWebsites.Add(site);
            NewWebsiteTextBox.Text = string.Empty;
        }

        private void RemoveWebsite_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid id)
            {
                var item = BlockedSites.FirstOrDefault(w => w.Id == id);
                if (item != null)
                {
                    BlockedSites.Remove(item);
                    var modelItem = _model.BlockedWebsites.FirstOrDefault(w => w.Id == id);
                    if (modelItem != null)
                    {
                        _model.BlockedWebsites.Remove(modelItem);
                    }
                }
            }
        }
    }
}
