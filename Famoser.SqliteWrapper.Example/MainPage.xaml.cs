using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Famoser.SqliteWrapper.Example.Models;
using Famoser.SqliteWrapper.Example.Repositories;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.SqliteWrapper.Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ModelRepository _repo = new ModelRepository();

        private MyModel _model = new MyModel()
        {
            GuidProperty = Guid.NewGuid(),
            MyIntProp = 4,
            MyStringProp = "content",
            StringList = new List<string>()
            {
                "hallo",
                "welt"
            },
            VisibilityEnum = Visibility.Collapsed
        };

        private async void SaveButton(object sender, RoutedEventArgs e)
        {
            await _repo.Save(_model);
            InfoTextBlock.Text = "Saved! got Id: " + _model.GetId();
        }

        private async void LoadButton(object sender, RoutedEventArgs e)
        {
            _model = await _repo.GetById(_model.GetId());
            InfoTextBlock.Text = "Inhalt: " + JsonConvert.SerializeObject(_model);
        }

        private async void RemoveButton(object sender, RoutedEventArgs e)
        {
            await _repo.Delete(_model);
            InfoTextBlock.Text = "Removed from Database!";
        }

        private async void AddButton(object sender, RoutedEventArgs e)
        {
            var model = new MyModel();
            await _repo.Save(model);
            InfoTextBlock.Text = "Added! got Id: " + model.GetId();
        }
    }
}
