using Our.Umbraco.Dashbraco.Models;
using Our.Umbraco.Dashbraco.Models.Dtos;

namespace Our.Umbraco.Dashbraco.Interfaces
{
    public interface IEntriesActivitiesService
    {
        public List<LogEntryDto> GetEntries();
        public StatsOverviewModel GetItemsInRecycleBin();
        public StatsOverviewModel GetTotalMembers();
        public StatsOverviewModel GetTotalMediaItems();
        public StatsOverviewModel GetTotalElements();
    }
}