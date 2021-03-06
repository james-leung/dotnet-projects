using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using FunFacts.Context;
using FunFacts.Dtos;
using FunFacts.Entities.User;

namespace FunFacts.Infrastructure.UserLogic
{
    public interface IProfileService
    {
        Task<User> GetProfile(string username);
        Task<User> GetUserProfile();
        Task<User> EditProfile(JsonPatchDocument<AppUser> patchEntity);
        Task<List<User>> GetAllUserProfiles();
    }

    public class ProfileService : IProfileService
    {
        private readonly FunFactsContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ProfileService(FunFactsContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }

        // Auto-login with token
        public async Task<User> GetUserProfile()
        {
            //var user = await _userManager.FindByNameAsync(input.Username);

            var user = await _userAccessor.GetCurrentAppUser();
            return _mapper.Map<AppUser, User>(user);
        }

        public async Task<List<User>> GetAllUserProfiles()
        {
            var users = await _userAccessor.GetAllAppUsers();
            return _mapper.Map<List<AppUser>, List<User>>(users);
        }

        public async Task<User> GetProfile(string username)
        {
            var user = await _userAccessor.GetAppUser(username);
            return _mapper.Map<AppUser, User>(user);
        }

        public async Task<User> EditProfile(JsonPatchDocument<AppUser> patchEntity)
        {
            var user = await _userAccessor.GetCurrentAppUser();
            patchEntity.ApplyTo(user);
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AppUser, User>(user);
        }
    }
}
