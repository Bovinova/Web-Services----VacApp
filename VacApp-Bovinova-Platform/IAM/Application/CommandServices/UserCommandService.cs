using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Aggregates;
using VacApp_Bovinova_Platform.IAM.Domain.Model.Commands;
using VacApp_Bovinova_Platform.IAM.Domain.Repositories;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.Shared.Domain.Repositories;

namespace VacApp_Bovinova_Platform.IAM.Application.CommandServices
{
    public class UserCommandService(
        IUserRepostory userRepository,
        IUnitOfWork unitOfWork,
        IHashingService hashingService,
        ITokenService tokenService
    ) : IUserCommandService
    {
        public async Task<string> Handle(SignUpCommand command)
        {
            var hashedCommand = command with { Password = hashingService.GenerateHash(command.Password) };
            var user = new User(hashedCommand);

            var existingUser = await userRepository.FindByEmailAsync(user.Email);

            if (existingUser != null)
                throw new Exception("User already exists");

            try
            {
                await userRepository.AddAsync(user);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return tokenService.GenerateToken(user);
        }

        public async Task<string> Handle(SignInCommand command)
        {
            User? user = null;

            if (!string.IsNullOrEmpty(command.Email))
            {
                user = await userRepository.FindByEmailAsync(command.Email);
            }
            else if (!string.IsNullOrEmpty(command.UserName))
            {
                user = await userRepository.FindByNameAsync(command.UserName);
            }

            if (user == null || !hashingService.VerifyHash(command.Password, user.Password))
                throw new Exception("Invalid username or password");

            return tokenService.GenerateToken(user);
        }
    }
}