using JobMagnet.Models;

namespace JobMagnet.Service.Interface;

public interface ISummaryService
{
    public Task<SummaryModel> GetById(int id);
}