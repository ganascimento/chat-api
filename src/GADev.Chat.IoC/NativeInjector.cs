using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using GADev.Chat.Identity.Models;
using GADev.Chat.Identity.Repositories;
using GADev.Chat.Application.Business.Implementations;
using GADev.Chat.Application.Business;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Application.Util;
using GADev.Chat.Application.Util.Implementation;
using GADev.Chat.Infrastructure.Repositories;

namespace GADev.Chat.IoC
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services) {
            
            services.AddTransient<IUserStore<ApplicationUser>, Identity.Repositories.UserRepository>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleRepository>();
            services.AddScoped<IMessageBusiness, MessageBusiness>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IInvitationBusiness, InvitationBusiness>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IUserRepository, Infrastructure.Repositories.UserRepository>();
            services.AddScoped<ITalkBusiness, TalkBusiness>();
            services.AddScoped<ITalkRepository, TalkRepository>();
            services.AddScoped<IImageStorage, ImageStorage>();
        }
    }
}