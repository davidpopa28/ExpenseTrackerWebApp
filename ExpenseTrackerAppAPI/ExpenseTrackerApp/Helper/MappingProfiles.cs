using AutoMapper;
using ExpenseTrackerApp.DTO;
using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountDTO, Account>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Record, RecordDTO>();
            CreateMap<RecordDTO, Record>();
            CreateMap<Subcategory, SubcategoryDTO>();
            CreateMap<SubcategoryDTO, Subcategory>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserAccount, UserAccountDTO>();
            CreateMap<UserAccountDTO,  UserAccount>();
        }
    }
}
