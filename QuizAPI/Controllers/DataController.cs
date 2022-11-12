using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Infrastructure.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("pracownicy")]
        public async Task<IEnumerable<PracownikViewModel>> GetAllEmployees()
        {
            var employees = await _dataService.GetAllEmployees();

            return employees;
        }

        [HttpGet("uczniowie")]
        public async Task<IEnumerable<UczenViewModel>> GetAllStudents()
        {
            var students = await _dataService.GetAllStudents();

            return students;
        }
    }
}
