function loadDiagnosisPartials(questionsSetId, diagnosisId) {
    $('#partialQuestionsSet').load("/Diagnosis/questionsSetPartial?questionsSetid=" +
        + questionsSetId,
        function () {
            //jako callback do załadowania zestawu pytań, jest ładowany widok
            //zapisu wyniku/oceny, który pobiera oceny danego zestawu pytań
            //z wyrenderowanego przez widok częściowy _QuestionsSet drzewa DOM
            //stąd te dwie operacje wykonują się sekwencyjnie

            $('#partialResult').load("/Diagnosis/resultPartial?diagnosisId=" +
                diagnosisId + "&questionsSetId=" + questionsSetId);
        });
};