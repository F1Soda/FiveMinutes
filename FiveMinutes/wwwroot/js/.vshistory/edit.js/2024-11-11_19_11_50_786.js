var fmt = JSON.parse(document.getElementById('modelData').value);
let questionCount = 0;

function addQuestion() {
    questionCount++;
    const questionHtml = createQuestionHtml(questionCount);
    document.getElementById('questions-container').insertAdjacentHTML('beforeend', questionHtml);
}

function createQuestionHtml(questionCount) {
    return `
        <div class="card mt-3">
          <div class="card-body">
            <h5 class="card-title">Вопрос ${questionCount}</h5>
            <div class="form-group">
              <label>Текст вопроса:</label>
              <input type="text" name="Questions[${questionCount - 1}].QuestionText" class="form-control" required />
            </div>
            <div class="form-group">
              <label>Тип ответа:</label>
              <select name="Questions[${questionCount - 1}].ResponseType" class="form-control" onchange="handleResponseTypeChange(this)">
                <option value="0">Один вариант</option>
                <option value="1">Несколько вариантов</option>
                <option value="2">Текстовый ответ</option>
              </select>
            </div>
            <div class="answers-container mt-2">
              <!-- Здесь будут динамически добавляться варианты ответов -->
            </div>
            <button type="button" class="btn btn-secondary mt-2" onclick="addAnswer(this)">Добавить вариант ответа</button>
          </div>
        </div>
      `;
}

function handleResponseTypeChange(select) {
    const answersContainer = select.closest('.card-body').querySelector('.answers-container');
    const addAnswerButton = select.closest('.card-body').querySelector('button');

    answersContainer.innerHTML = '';
    if (select.value === "2") { // Текстовый ответ
        addAnswerButton.style.display = 'none';
    } else {
        addAnswerButton.style.display = 'block';
    }
}

function addAnswer(button) {
    const questionCard = button.closest('.card');
    const questionIndex = Array.from(questionCard.parentNode.children).indexOf(questionCard);
    const answersContainer = questionCard.querySelector('.answers-container');
    const answerCount = answersContainer.children.length;

    const answerHtml = createAnswerHtml(questionIndex, answerCount);
    answersContainer.insertAdjacentHTML('beforeend', answerHtml);
}

function createAnswerHtml(questionIndex, answerCount) {
    return `
        <div class="form-group">
          <label>Вариант ответа ${answerCount + 1}:</label>
          <input type="text" name="Questions[${questionIndex}].Answers[${answerCount}].Text" class="form-control" required />
          <div class="form-check">
            <input class="form-check-input" type="checkbox" name="Questions[${questionIndex}].Answers[${answerCount}].IsCorrect" id="Questions[${questionIndex}].Answers[${answerCount}].isCorrect${questionIndex}_${answerCount}" value="true">
            <label class="form-check-label" for="isCorrect${questionIndex}_${answerCount}">
              Правильный ответ
            </label>
          </div>
        </div>
      `;
}

function initQuestions() {
    const questionsContainer = document.getElementById('questions-container');
    questionsContainer.innerHTML = ''; // Clear any existing questions

    fmt["questions"].forEach((question, questionIndex) => {
        const questionHtml = createQuestionHtml(questionIndex + 1);
        questionsContainer.insertAdjacentHTML('beforeend', questionHtml);

        const answersContainer = questionsContainer.querySelectorAll('.answers-container')[questionIndex];
        question["answers"].forEach((answer, answerIndex) => {
            const answerHtml = createAnswerHtml(questionIndex, answerIndex);
            answersContainer.insertAdjacentHTML('beforeend', answerHtml);
        });
        questionCount++;
    });
}

// Call the initQuestions function when the page loads
document.addEventListener('DOMContentLoaded', initQuestions);

document.getElementById('add-question').addEventListener('click', addQuestion);

$(document).ready(function () {
    $('#edit-form').submit(function (e) {
        e.preventDefault(); // Prevent the default form submission behavior

        // Serialize the form data into a JSON object
        var formData = $(this).serializeArray();
        var jsonData = {};
        $.each(formData, function () {
            jsonData[this.name] = this.value;
        });

        // Send the data to your controller using AJAX
        $.ajax({
            url: '@Url.Action("Save")',
            type: 'POST',
            data: JSON.stringify(jsonData),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (response) {
                // Handle the success response here
                console.log('Test saved successfully');
                showPopup();
            },
            error: function (xhr, status, error) {
                // Handle the error response here
                console.error('Error saving test:', error);
            }
        });
    });
});

function showPopup() {
    const popup = document.getElementById("popupSave");
    popup.style.display = "block";

    // Automatically close the popup after 3 seconds (3000 ms)
    setTimeout(function () {
        popup.style.display = "none";
    }, 3000);
}
