using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Model.Queries;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Services;

namespace VacApp_Bovinova_Platform.CampaignManagement.Application.Internal.QueryServices;

public class CampaignQueryService(ICampaignRepository campaignRepository)
:ICampaignQueryService
{
    public async Task<Campaign?> Handle(GetCampaignByIdQuery query)
    {
        return await campaignRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Campaign>> Handle(GetAllCampaignsQuery query)
    {
        return await campaignRepository.ListAsync();
    }

    public async Task<IEnumerable<Goal>> Handle(GetGoalsFromCampaignIdQuery query)
    {
        return await campaignRepository.FindByCampaignId(query.CampaignId);
    }

    public async Task<IEnumerable<Channel>> Handle(GetChannelsFromCampaignIdQuery query)
    {
        return await campaignRepository.FindChannelsByCampaignId(query.CampaignId);
    }
}