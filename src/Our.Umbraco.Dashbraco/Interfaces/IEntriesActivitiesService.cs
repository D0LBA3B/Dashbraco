using Our.Umbraco.Dashbraco.Models.Dtos;

namespace Our.Umbraco.Dashbraco.Interfaces
{
    public interface IEntriesActivitiesService
    {
        public List<LogEntryDto> GetEntries();
    }
}