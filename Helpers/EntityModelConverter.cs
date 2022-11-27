using authentication.Entities;
using authentication.Models;
using authentication.PostModels.cs;
using authentication.ViewModels;

namespace authentication.Helpers
{
    public class EntityModelConverter
    {
        public static UserViewModel ConvertUserEntityToUserViewModel(User theUser)
       => theUser == null ? new UserViewModel() : new UserViewModel
       {
           Id = theUser.Id,
           FirstName = theUser.FirstName,
           LastName = theUser.LastName,
           Gender = theUser.Gender,
           DateOfBirth = theUser.DateOfBirth,
           Username = theUser.Username,
       };
  
        public static AuthResponse ConvertUserEntityToAuthResponse(User theUser)
        => theUser == null ? new AuthResponse() : new AuthResponse
        {
            FirstName = theUser.FirstName,
            LastName = theUser.LastName,
            Gender = theUser.Gender,
            DateOfBirth = theUser.DateOfBirth.Date,
            Username = theUser.Username,
        };
        public static User ConvertUserPostModelToUserEntity(UserPostModel theUser)
       => theUser == null ? new User() : new User
       {
           FirstName = theUser.FirstName,
           LastName = theUser.LastName,
           Gender = theUser.Gender,
           DateOfBirth = theUser.DateOfBirth,
           Username = theUser.Username,
           PasswordHash = PasswordHasher.HashText(theUser.Password, theUser.Username),
       };
    }
}
