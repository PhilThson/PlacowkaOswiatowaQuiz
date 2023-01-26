using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class DiagnosisController : Controller
    {
        #region Prywatne pola
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public DiagnosisController(
            IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Lista wszystkich utworzonych formularzy
        public async Task<IActionResult> Index()
        {
            var diagnosis = new List<DiagnosisViewModel>();
            try
            {
                diagnosis = await _httpClient.GetAllItems<DiagnosisViewModel>();
                return View(diagnosis);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać wszystkich" +
                    $"formularzy diagnoz. \nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosis);
            }
        }
        #endregion

        #region Zapis wyniku wybranej oceny zestawu pytań
        [HttpPost]
        public async Task<PartialViewResult> SaveResult(ResultViewModel resultVM)
        {
            if (ModelState.ContainsKey("QuestionsSetRating.RatingDescription"))
            {
                //Manualne usunięcie walidacji niewykorzystywanej właściwości ViewModel'u
                //tzn. jest ComboBox z ocenami zestawu pytań do wyboru (a nie tworzenia)
                ModelState["QuestionsSetRating.RatingDescription"].Errors.Clear();
                ModelState["QuestionsSetRating.RatingDescription"].ValidationState =
                    ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
                return PartialView("_Result", resultVM);

            try
            {
                var createResultDto = new CreateResultDto
                {
                    DiagnosisId = resultVM.DiagnosisId,
                    RatingId = resultVM.QuestionsSetRating.Id,
                    RatingLevel = resultVM.RatingLevel.Value,
                    Notes = resultVM.Notes
                };

                await _httpClient.AddItem(createResultDto);
                ViewBag.SaveSuccess = "Poprawnie zapisno ocenę.";
                return PartialView("_Result", resultVM);
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się zapisać oceny" +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return PartialView("_Result", resultVM);
            }
        }
        #endregion

        #region Wyświetlanie formularza diagnozy (tworzenie/edycja)
        public async Task<IActionResult> Form([FromQuery] int diagnosisId)
        {
            var diagnosis = new DiagnosisViewModel();
            try
            {
                diagnosis = await _httpClient.GetItemById<DiagnosisViewModel>(diagnosisId);

                if (diagnosis.ReportId.HasValue)
                    throw new DataValidationException(
                        "Nie można edytować diagnozy, która posiada wygenerowany raport");

                //jeżeli jest to edycja, to listę identyfikatorów zestawów pytań można
                //wyciągnąć z wyników diagnozy
                if (diagnosis.Results?.Count > 0)
                {
                    diagnosis.IsForEdit = true;
                    diagnosis.QuestionsSetsIds = diagnosis.Results
                        .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();
                }
                else
                {
                    var questionsSets = await _httpClient.GetAllItems<QuestionsSetViewModel>(
                        ("difficultyId", diagnosis.Difficulty.Id.ToString()));

                    //Lista identyfikatorów zestawów pytań, których modele będą pobrane 
                    //asynchronicznie na etapie przełączania zestawów na formularzu diagnozy
                    diagnosis.QuestionsSetsIds = questionsSets.Select(qs => qs.Id).ToList();
                }

                //if(ids?.Count < 1) - nie zwaliduje dla listy = null
                if (!(diagnosis.QuestionsSetsIds?.Count > 0))
                    throw new ArgumentNullException(nameof(diagnosis.QuestionsSetsIds));

                return View(diagnosis);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać formularza diagnozy" +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> QuestionsSetPartial([FromQuery] int questionsSetId)
        {
            try
            {
                var questionsSet =
                    await _httpClient.GetItemById<QuestionsSetViewModel>(questionsSetId);
                return PartialView("_QuestionsSet", questionsSet);
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać zestawu pytań." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return PartialView("_QuestionsSet", new QuestionsSetViewModel());
            }
        }

        public async Task<IActionResult> ResultPartial([FromQuery] int diagnosisId,
            [FromQuery] int questionsSetId)
        {
            var result = new ResultViewModel() { DiagnosisId = diagnosisId };
            try
            {
                result = await _httpClient.GetItemById<ResultViewModel>(null,
                    (nameof(diagnosisId), diagnosisId.ToString()),
                    (nameof(questionsSetId), questionsSetId.ToString()));
                    
                return PartialView("_Result", result);
            }
            catch(Exception e)
            {
                //Tutaj jeżeli nie znaleziono, (api zwraca NotFound),
                //to poprostu wynik nie jest uzupełniany
                return PartialView("_Result", result);
            }
        }
        #endregion

        #region Tworzenie formularza diagnozy
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDiagnosisViewModel diagnosisVM)
        {
            if (!ModelState.IsValid)
                return View(diagnosisVM);
            try
            {
                //DifficultyId jest wymagane na formularzu
                var availableQuestionsSets = await _httpClient.GetAllItems<QuestionsSetViewModel>(
                    ("difficultyId", diagnosisVM.DifficultyId.Value.ToString())) ??
                //await _questionsSetService.GetAllQuestionsSets(diagnosisVM.DifficultyId) ??
                    new List<QuestionsSetViewModel>();

                if (!availableQuestionsSets.Any())
                    throw new DataValidationException(
                        "Brak dostępnych zestawów pytań dla wybranej skali trudności. " +
                        "Proszę dodać zestawy pytań i ponownie utworzyć formularz diagnozy.");

                var emptyQuestionsSetsIds = availableQuestionsSets
                    .Where(qs => qs.Questions.Count() < 1)
                    .Select(qs => qs.Id)
                    .ToList();

                if (emptyQuestionsSetsIds.Count > 0)
                    throw new DataValidationException(
                        "Jeden z zestawów pytań dostępnych dla diagnozy, " +
                        "nie posiada pytań składowych. Proszę uzupełnić pytania " +
                        "i ponownie utworzyć formularz diagnozy. " +
                        "Lista identyfikatorów pustych zestawów pytań: " +
                        $"{string.Join(", ", emptyQuestionsSetsIds)}");

                var createDiagnosisDto = (CreateDiagnosisDto)diagnosisVM;
                var createdDiagnosisId = await _httpClient.AddItem(createDiagnosisDto);
                //Po utworzeniu diagnozy, przekierowanie do formularza (pierwszego zestawu pytań)
                return RedirectToAction(nameof(Form), new { diagnosisId = createdDiagnosisId });
            }
            catch(DataValidationException e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć formularza diagnozy. {e.Message}";
                return View(diagnosisVM);
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć formularza diagnozy." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosisVM);
            }
        }
        #endregion

        #region Podsumowanie/szczegóły formularza diagnozy
        //Przekazywanie parametru przez Route wymaga ręcznego zadeklarowania endpointu
        [HttpGet("Diagnosis/Details/{diagnosisId}")]
        public async Task<IActionResult> Details([FromRoute] int diagnosisId)
        {
            var diagnosisSummary = new DiagnosisSummaryViewModel();
            try
            {
                diagnosisSummary = await GetDiagnosisSummary(diagnosisId);
                return View(diagnosisSummary);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = "Nie udało się pobrać formularza diagnozy o " +
                    $"podanym identyfikatorze {diagnosisId}. Odpowiedź serwera: '{e.Message}'";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Metody prywatne
        private async Task<DiagnosisSummaryViewModel> GetDiagnosisSummary(int diagnosisId)
        {
            var diagnosis = await _httpClient.GetItemById<DiagnosisViewModel>(diagnosisId);

            var askedQuestionSetsIds = new List<int>();
            //QuestionsSetRating (ocena zestawu pytań) jest wymagana do podania,
            //więc na pewno istnieje, jeżeli istnieje powiązany z diagnozą Result (wynik)
            if (diagnosis.Results?.Count > 0)
                askedQuestionSetsIds = diagnosis.Results
                    .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var questionsSets =
                await _httpClient.GetAllItems<QuestionsSetViewModel>(
                    ("askedQuestionSetsIds", string.Join(',', askedQuestionSetsIds))) ??
                //await _questionsSetService.GetQuestionsSetsByIds(askedQuestionSetsIds) ??
                new List<QuestionsSetViewModel>();

            var diagnosisSummary = (DiagnosisSummaryViewModel)diagnosis;
            diagnosisSummary.QuestionsSets = questionsSets;
            return diagnosisSummary;
        }
        #endregion
    }
}

