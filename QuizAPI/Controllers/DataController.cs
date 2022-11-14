using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Infrastructure;
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

        #region Pracownicy
        [HttpGet("pracownicy")]
        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees()
        {
            var employees = await _dataService.GetAllEmployees();

            return employees;
        }
        #endregion

        #region Uczniowie
        [HttpGet("uczniowie")]
        public async Task<IEnumerable<UczenViewModel>> GetAllStudents()
        {
            var students = await _dataService.GetAllStudents();

            return students;
        }
        #endregion

        #region Pytania
        [HttpGet("pytania")]
        public async Task<IEnumerable<QuestionViewModel>> GetAllQuestions()
        {
            var questions = await _dataService.GetAllQuestions();

            return questions;
        }

        [HttpGet("pytania/{id}", Name = nameof(GetQuestionById))]
        public async Task<IActionResult> GetQuestionById([FromQuery] int id)
        {
            try
            {
                var question = await _dataService.GetQuestionById(id);
                return Ok(question);
            }
            catch(DataNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPost("pytania")]
        public async Task<IActionResult> CreateQuestion(QuestionViewModel pytanieVM)
        {
            try
            {
                var question = await _dataService.AddQuestion(pytanieVM);

                return CreatedAtRoute(nameof(GetQuestionById),
                    new { id = question.Id}, question);
            }
            catch(DataNotFoundException e)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Zestawy pytań
        [HttpGet("zestawyPytan")]
        public async Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets()
        {
            var questionsSets = await _dataService.GetAllQuestionsSets();

            return questionsSets;
        }

        [HttpGet("zestawyPytan/{id}", Name = nameof(GetQuestionsSetById))]
        public async Task<IActionResult> GetQuestionsSetById([FromQuery] int id)
        {
            try
            {
                var questionsSet = await _dataService.GetQuestionsSetById(id);
                return Ok(questionsSet);
            }
            catch (DataNotFoundException e)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
