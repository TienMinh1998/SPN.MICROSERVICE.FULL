using MediatR;
using SPNApplication.Authentication;
using SPNApplication.Queries;
using SPNApplication.Services;

namespace SPNApplication.Handler
{
    public class LoginQueryHandler : IRequestHandler<LoginApplicationQuery, string>
    {

        private IDapperService _dapper;
        public LoginQueryHandler(IDapperService dapper)
        {
            _dapper = dapper;
        }

        public async Task<string> Handle(LoginApplicationQuery request, CancellationToken cancellationToken)
        {
            var userID = request.user_id;
            string sql = "SELECT p.\"Id\"  FROM usr.\"User\" u \r\nINNER JOIN usr.\"UserRole\" ur ON u.\"Id\" = ur.\"FK_UserID\" " +
                "INNER JOIN usr.rolepermission rp ON ur.\"FK_RoleID\" = rp.\"FK_RoleID\" " +
                $"INNER JOIN usr.\"permission\" p ON rp.\"FK_PermissionID\" = p.\"Id\" WHERE u.\"Id\" = {userID} ";
            var permistion = (await _dapper.GetAllAsync<string>(sql)).ToArray();
            JWTHandler jwtHandler = new JWTHandler();
            var newToken = jwtHandler.CreateToken(request.username, request.user_id, permistion);
            return newToken;
        }
    }
}
