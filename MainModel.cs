using System.Collections.ObjectModel;
using System.ComponentModel;

namespace scriabinWPF
{
    public class MainModel
    {
        public ObservableCollection<MapProfileModel> MapProfiles { get; set; }
        public MainModel() {
            MapProfiles = [
                new MapProfileModel(1, "Profile 1"),
                new MapProfileModel(2, "Profile 2"),
                new MapProfileModel(3, "Profile 3")
                ];
        }
    }
}
