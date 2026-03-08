using BugTrackerAPI.DTO;
using BugTrackerAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BugTrackerAPI.Controllers
{

    [ApiController] //for declaring that this class is using api controllers
    [Route("api/[Controller]")] //our API bug controller route 
    public class BugsController : ControllerBase
    {
  
        private readonly string _connetionString; //the connetion between the DB and the server

        //this is my connection string to the DB 
        public BugsController(IConfiguration config)
        {
            _connetionString = config.GetConnectionString("DefaultConnection") ;
        }


        [HttpPost]
        [Route("reportBug")]
        public async Task<IActionResult> ReportBug([FromBody] ReportBugRequest request) {

            string enteredFileName = request.file_name;
            string enteredBugName = request.bug_name;
            string enteredBugDesc = request.bug_description;
            int enteredBugSev = request.bug_severity;
            string enteredBugGroupNumber = request.bug_group_number;


            if (enteredBugName.Length < 2 || enteredBugName.Any(char.IsDigit)) {
                return BadRequest("Bug name is not valid");  
            }

            if (enteredBugDesc.Length < 2)
            {
                return BadRequest("Enter a valid description");
            }

            if (enteredFileName.Length < 2)
            {
                return BadRequest("Enter a valid file name");
            }

            if (enteredBugSev > 10 || enteredBugSev < 1)
            {
                return BadRequest("Severity must be between 1 and 10");
            }

            if(enteredBugGroupNumber == null)
            {
                return BadRequest("there is no group found");
            }

            using var connection = new NpgsqlConnection(_connetionString); //oppening a connection between server and DB
            string Sql= @"INSERT INTO Bugs(file_name,bug_name,bug_description,bug_severity,is_deleted,is_solved,bug_group_number)
                        VALUES(@file_name, @bug_name, @bug_description, @bug_severity, false, false, @bug_group_number) "; //querey for inserting a new bug into the DB

            await connection.ExecuteAsync(Sql, request);  //using dapper to write queres into my server with its help i can talk with the database

            return Ok("bug reported successfully"); //return "..." if the bug was inserted to the DB successfully
        }






        //a get request that takes a group number from the user when he enter a new group or create new one and show all the bugs related to that group
        [HttpPost]
        [Route("getAllBugs")]
        public async Task<IActionResult> getAllBugs([FromBody] JoinGroupRequest request)
        {

            string enteredGroupNumber = request.GroupNumber;


            if (string.IsNullOrEmpty(enteredGroupNumber))
            {
                return BadRequest("group number not found");
            }
   
            using var connection = new NpgsqlConnection(_connetionString);
            
            string Sql = "SELECT * FROM bugs WHERE bug_group_number = @GroupNumber AND is_solved = false AND is_deleted=false ORDER BY bug_severity DESC";
            var activeBugs = await connection.QueryAsync<Bug>(Sql,new { GroupNumber = enteredGroupNumber }); //getting all active bugs that there group number = the accepted one

            return Ok(activeBugs);
        }



        //a delete request where it do soft delete by changing the is_deleted to true
        [HttpDelete]
        [Route("DeleteBug/{id}")]
        public async Task<IActionResult> DeleteBug(int id)
        {
            using var connection = new NpgsqlConnection(_connetionString);
            string Sql = "UPDATE Bugs SET is_deleted = true WHERE id=@Id";
            var deletedBug = await connection.ExecuteAsync(Sql,new {Id = id});
            if (deletedBug == 0)
            {
                return NotFound($"there is no such bug with id {id}");
            }

            return Ok("Bug was deleted successfully");
        }



        //if the client checked a bug it will trigger this endpoint so the is_solved will be true
        [HttpPut]
        [Route("Solved/{id}")]
        public async Task<IActionResult> Solved(int id)
        {
            using var connection = new NpgsqlConnection(_connetionString);
            string Sql = "UPDATE Bugs SET is_solved = true WHERE id=@Id";
            var isSolved=await connection.ExecuteAsync(Sql, new { Id = id });
            if (isSolved == 0)
            {
                return NotFound($"There is no bug with id {id}");
            }


            return Ok("Bug Solved and moved to the checkedList");
        }





        //get request taking a group number and show all the solved bugs related to that group
        [HttpPost]
        [Route("getAllSolvedBugs")]
        public async Task<IActionResult> getAllSolvedBugs([FromBody] JoinGroupRequest request)
        {
            using var connection = new NpgsqlConnection(_connetionString);


            string enteredGroupNumber = request.GroupNumber;


            string Sql = "SELECT * FROM Bugs WHERE is_solved = true AND is_deleted = false AND bug_group_number=@GroupNumber";
            var Solved = await connection.QueryAsync<Bug>(Sql, new {GroupNumber= enteredGroupNumber });
            if (!Solved.Any())
            {
                return BadRequest("There is no solved bugs");
            }
            return Ok(Solved);
        }
    }
}











