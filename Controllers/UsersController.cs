using BugTrackerAPI.DTO;
using BugTrackerAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;

namespace BugTrackerAPI.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class UsersController : ControllerBase
    {
        private readonly string _connetionString;

        public UsersController(IConfiguration config)
        {
            _connetionString = config.GetConnectionString("DefaultConnection");
        }


        //post request acceptin email and password checking if this exact email and pass is logged in ,in the DB
        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest request) {
            await using var connection = new NpgsqlConnection(_connetionString);

            string enteredEmail = request.Email;
            string enteredPass = request.Password;

            //selecting the exact row where email and pass matches the given one
            string Sql = "SELECT email,password FROM Users WHERE email = @email AND password = @password  " ;

            var user = await connection.QueryFirstOrDefaultAsync<User>(Sql, new { password= enteredPass, email= enteredEmail });

            if (user == null)
            {
                return BadRequest("Wrong email or password");
            }

            return Ok();
        }



        //post request accepts email and pass addin it to the User table in DB if its not already threre
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] LoginRequest request)
        {
            using var connection = new NpgsqlConnection(_connetionString);

            string enteredEmail = request.Email;
            string enteredPass = request.Password;

            string Sql1 ="SELECT email FROM Users WHERE email=@email";
            var IfAlreadyExist = await connection.QueryFirstOrDefaultAsync<User>(Sql1, new { email = enteredEmail });

            if (IfAlreadyExist != null)
            {
                return BadRequest("You are already signed up");
            }


            string Sql = @"INSERT INTO Users (email,password) VALUES (@email,@password)";
            await connection.ExecuteAsync(Sql,new {email= enteredEmail, password= enteredPass });

            return Ok("You have signed up successfully");

        }



        //this func accepts a group number finds if exists in the group table return 200OK if exist
        [HttpPost]
        [Route("JoinGroup")]
        public async Task<IActionResult> JoinGroup([FromBody] JoinGroupRequest request)
        {
            using var connetion = new NpgsqlConnection(_connetionString);

            string enteredGroupNumber = request.GroupNumber;

            String Sql = "SELECT group_number FROM Groups WHERE group_number=@GroupNumber";
            var isThereAGroupNumber = await connetion.QueryFirstOrDefaultAsync<Group>(Sql,new { GroupNumber = enteredGroupNumber });

            if (isThereAGroupNumber != null)
            {
                return Ok();
            }

            return BadRequest("there is no such group");

        }



        //this func generates new group number checks if its unique and send it back in response
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateGroup()
        {
            using var connection = new NpgsqlConnection(_connetionString);

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char[] randomArray = new char[9];
            
            for (int i = 0; i < 9; i++)
            {
                randomArray[i] = chars[Random.Shared.Next(chars.Length)];
            }

            //Convert array of characters into final string
            string GroupNumber = new string(randomArray);

            string Sql1 = "SELECT group_number FROM Groups WHERE group_number = @GroupNumber";
            var IsTheNewGroupNumberInDB = await connection.QueryFirstOrDefaultAsync<Group>(Sql1, new { GroupNumber = GroupNumber });


            //if not null then we find the number inside the DB then we make new one 
            if(IsTheNewGroupNumberInDB != null)
            {

                //if the new number is already exist then make new one and see if exists if yes do another random one until you find the one that isnt in the DB
                while (IsTheNewGroupNumberInDB != null)
                {                

                    for (int i = 0; i < 9; i++)
                    {
                        randomArray[i] = chars[Random.Shared.Next(chars.Length)];
                    }

                    //Convert array of characters into final string
                    GroupNumber = new string(randomArray);

                    IsTheNewGroupNumberInDB = await connection.QueryFirstOrDefaultAsync<Group>(Sql1,new {GroupNumber = GroupNumber });
                }

            }

            //adding the new generated group to the group table 
            string Sql2 = @"INSERT INTO Groups (group_number) VALUES (@GroupNumber)";
            await connection.ExecuteAsync(Sql2, new { GroupNumber = GroupNumber });

            return Ok(GroupNumber);

        }














































    }
}
