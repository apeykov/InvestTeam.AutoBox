namespace InvestTeam.AutoBox.Application.Data.DTOs
{
    using AutoMapper;
    using InvestTeam.AutoBox.Application.Common.Mappings;
    using InvestTeam.AutoBox.Domain.Attributes;
    using InvestTeam.AutoBox.Domain.Entities;
    using InvestTeam.AutoBox.Domain.Enums;

    public class VechicleDTO : IMapFrom<Vechicle>
    {
        [Filter]
        public string Number { get; set; }

        [Filter]
        public Color? Color { get; set; }

        [Filter]
        public string Model { get; set; }


        public void TransferUpdatedData(Vechicle vechicle)
        {
            if (!string.IsNullOrEmpty(Number))
            {
                vechicle.Identity = Number;
            }

            if (!string.IsNullOrEmpty(Model))
            {
                vechicle.Model = Model;
            }

            if (this.Color.HasValue)
            {
                vechicle.Color = Color.Value;
            }

        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VechicleDTO, Vechicle>()
                .ForMember(dest => dest.Identity, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.HasValue ? src.Color.Value : Domain.Enums.Color.Black))
                .ReverseMap()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Identity));
        }
    }
}
