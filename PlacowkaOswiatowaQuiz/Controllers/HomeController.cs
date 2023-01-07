﻿using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Models;
using PlacowkaOswiatowaQuiz.Services;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using System.Diagnostics;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDiagnosisService _diagnosisService;
        private readonly IQuestionsSetService _questionsSetService;

        public HomeController(ILogger<HomeController> logger,
            IDiagnosisService diagnosisService,
            IQuestionsSetService questionsSetService)
        {
            _logger = logger;
            _diagnosisService = diagnosisService;
            _questionsSetService = questionsSetService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var model = GetInvoiceModel();
            return View(model);
        }

        public async Task<IActionResult> DiagnosisSummary()
        {
            var diagnosisToPdf = await GetDiagnosisPdf(1002);

            return View(diagnosisToPdf);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private InvoiceViewModel GetInvoiceModel()
        {
            var invoiceViewModel = new InvoiceViewModel
            {
                OrderDate = DateTime.Now,
                OrderId = 1234567890,
                DeliveryDate = DateTime.Now.AddDays(10),
                Products = new List<Product>()
                {
                    new Product
                    {
                        ItemName = "Hosting (12 months)",
                        Price = 200
                    },
                    new Product
                    {
                        ItemName = "Domain name (1 year)",
                        Price = 12
                    },
                    new Product
                    {
                        ItemName = "Website design",
                        Price = 1000

                    },
                    new Product
                    {
                        ItemName = "Maintenance",
                        Price = 300
                    },
                    new Product
                    {
                        ItemName = "Customization",
                        Price = 400
                    },
                }
            };

            invoiceViewModel.TotalAmount = invoiceViewModel.Products.Sum(p => p.Price);

            return invoiceViewModel;
        }

        private async Task<DiagnosisToPdfViewModel> GetDiagnosisPdf(int diagnosisId)
        {
            var diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);

            var askedQuestionSetsIds = new List<int>();

            if (diagnosis.Results?.Count > 0)
                askedQuestionSetsIds = diagnosis.Results
                    .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var questionsSets =
                await _questionsSetService.GetQuestionsSetsByIds(askedQuestionSetsIds);

            var masteredQSIds = diagnosis.Results.Where(d => d.RatingLevel > 4)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();
            var toImproveQSIds = diagnosis.Results.Where(d => d.RatingLevel < 5)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            return new DiagnosisToPdfViewModel
            {
                Id = diagnosis.Id,
                Student = diagnosis.Student,
                Employee = diagnosis.Employee,
                CreatedDate = diagnosis.CreatedDate,
                Difficulty = diagnosis.Difficulty,
                SchoolYear = diagnosis.SchoolYear,
                Results = diagnosis.Results,
                QuestionsSetsMastered =
                    questionsSets.Where(qs => masteredQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList(),
                QuestionsSetsToImprove =
                    questionsSets.Where(qs => toImproveQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList(),
            };
        }
    }
}