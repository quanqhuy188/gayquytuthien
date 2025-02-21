using AutoMapper;
using GayQuyTuThien.DataContext.Entity;
using GayQuyTuThien.DTOs;
using GayQuyTuThien.Enums;
using GayQuyTuThien.RequestViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;

namespace GayQuyTuThien
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


			#region Function
			CreateMap<Function, FunctionDto>();
			#endregion
			#region Role
			CreateMap<ApplicationRole, RoleDto>();
			#endregion
			#region User
			CreateMap<ApplicationUser, UserDto>();
			CreateMap<ApplicationUser, UserUpdateViewModel>();
			CreateMap<UserAddViewModel, ApplicationUser>();
			#endregion
			#region Picture
			CreateMap<Picture, PictureDto>();

			CreateMap<PictureRequest, Picture>();
			#endregion
			#region SubmitForm
			CreateMap<SubmitForm, SubmitFormDto>();

			CreateMap<SubmitFormRequest, SubmitForm>();
            #endregion
            #region Html Content
            CreateMap<HtmlContent, HtmlContentDto>();

            CreateMap<HtmlContentRequest, HtmlContent>();
            #endregion
        }
    }
}
