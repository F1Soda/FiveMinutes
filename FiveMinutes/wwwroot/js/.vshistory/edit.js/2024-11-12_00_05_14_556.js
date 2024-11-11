﻿// Initialize variables
let fmt = null;
let questionCount = 0;
let hasUnsavedChanges = false;

// Function to generate HTML for a question card
function getQuestionHtml(question, questionIndex) {
    return `
        <div class="card mt-3 question-card">
            <div class="card-body">
                <h5 class="card-title">Вопрос ${questionIndex + 1}</h5>
                <button type="button" class="btn btn-danger btn-sm mb-2" onclick="deleteQuestion(this)">Удалить вопрос</button>
                <div class="form-group">
                    <label>Текст вопроса:</label>
                    <input type="text" name="Questions[${questionIndex}].QuestionText" class="form-control" value="${question["questionText"]}" required />
                </div>
                <div class="form-group">
                    <label>Тип ответа:</label>
                    <select name="Questions[${questionIndex}].ResponseType" class="form-control" onchange="handleResponseTypeChange(this)">
                        <option value="0" ${question["responseType"] === 0 ? 'selected' : ''}>Один вариант</option>
                        <option value="1" ${question["responseType"] === 1 ? 'selected' : ''}>Несколько вариантов</option>
                        <option value="2" ${question["responseType"] === 2 ? 'selected' : ''}>Текстовый ответ</option>
                    </select>
                </div>
                <div class="answers-container mt-2"></div>
                <button type="button" class="btn btn-secondary mt-2" onclick="addAnswer(this)" style="${question["responseType"] === 2 ? 'display: none;' : ''}">Добавить вариант ответа</button>
            </div>
        </div>
    `;
}

// Function to generate HTML for an answer item
function getAnswerHtml(answer, questionIndex, answerIndex) {
    return `
        <div class="form-group answer-item">
            <label>Вариант ответа ${answerIndex + 1}:</label>
            <input type="text" name="Questions[${questionIndex}].Answers[${answerIndex}].Text" class="form-control" value="${answer["text"]}" required />
            <div class="form-check">
                <input class="form-check-input" type="checkbox" name="Questions[${questionIndex}].Answers[${answerIndex}].IsCorrect" id="Questions[${questionIndex}].Answers[${answerIndex}].isCorrect${questionIndex}_${answerIndex}" value="true" ${answer["isCorrect"] ? 'checked' : ''}>
                <label class="form-check-label" for="isCorrect${questionIndex}_${answerIndex}">Правильный ответ</label>
            </div>
            <button type="button" class="btn btn-danger btn-sm mt-2" onclick="deleteAnswer(this)">Удалить вариант</button>
        </div>
    `;
}

function addQuestion() {
	// Increment question count
	questionCount++;

	// Define a default question object to pass to getQuestionHtml
	const newQuestion = {
		questionText: '',
		responseType: 0, // Default to "Один вариант"
		answers: [] // Start with no answers
	};

	// Generate the HTML for a new question
	const questionHtml = getQuestionHtml(newQuestion, questionCount - 1);

	// Insert the new question HTML into the questions container
	document.getElementById('questions-container').insertAdjacentHTML('beforeend', questionHtml);
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
	// Find the closest question card and get the question index
	const questionCard = button.closest('.question-card');
	const questionIndex = Array.from(questionCard.parentNode.children).indexOf(questionCard);

	// Find the answers container within this question card
	const answersContainer = questionCard.querySelector('.answers-container');
	const answerCount = answersContainer.children.length; // Count current answers

	// Define a default answer object to pass to getAnswerHtml
	const newAnswer = {
		text: '',
		isCorrect: false // Default to unchecked
	};

	// Generate the HTML for a new answer
	const answerHtml = getAnswerHtml(newAnswer, questionIndex, answerCount);

	// Insert the new answer HTML into the answers container
	answersContainer.insertAdjacentHTML('beforeend', answerHtml);
}