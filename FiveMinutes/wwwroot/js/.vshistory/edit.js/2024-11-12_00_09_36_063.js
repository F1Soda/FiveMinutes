﻿// Initialize variables
let fmt = modelData;
let questionCount = 0;
let hasUnsavedChanges = false;

document.addEventListener('DOMContentLoaded', initQuestions);

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
																<div class="answers-container mt-2">
																	<!-- Answer options will be dynamically added here -->
																</div>
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

function showSaveIcon() {
	const iconElement = $('#save-icon');
	iconElement.stop(true, true).fadeIn(200).delay(1000).fadeOut(200); // Show and fade out after 1 second
}

function deleteAnswer(button) {
	const answerItem = button.closest('.answer-item');
	answerItem.remove();
}

function deleteQuestion(button) {
	const questionCard = button.closest('.question-card');
	questionCard.remove();
}

function initQuestions() {
	const questionsContainer = document.getElementById('questions-container');
	questionsContainer.innerHTML = ''; // Clear any existing questions

	fmt["questions"].forEach((question, questionIndex) => {
		// Insert question HTML
		const questionHtml = getQuestionHtml(question, questionIndex);
		questionsContainer.insertAdjacentHTML('beforeend', questionHtml);

		// Add answers for the question
		const answersContainer = questionsContainer.querySelectorAll('.answers-container')[questionIndex];
		question["answers"].forEach((answer, answerIndex) => {
			const answerHtml = getAnswerHtml(answer, questionIndex, answerIndex);
			answersContainer.insertAdjacentHTML('beforeend', answerHtml);
		});
		questionCount += 1;
	});

}

// Define the save method
function save(isFinalSave = false) {
	console.log('save was called!');

	// Serialize the form data into a JSON object
	const jsonData = {
		Id: $('#Id').val(),
		Name: $('input[name="Name"]').val(),
		ShowInProfile: true,
		Questions: []
	};

	$('#questions-container .card').each(function (index, element) {
		const question = {
			Position: index + 1,
			QuestionText: $(element).find('input[name^="Questions"]').val(),
			ResponseType: parseInt($(element).find('select[name^="Questions"]').val(), 10),
			Answers: []
		};

		$(element).find('.answers-container .answer-item').each(function () {
			const answer = {
				Position: question.Answers.length + 1,
				Text: $(this).find('input[type="text"]').val(),
				IsCorrect: $(this).find('input[type="checkbox"]').is(':checked')
			};
			question.Answers.push(answer);
		});

		jsonData.Questions.push(question);
	});

	$.ajax({
		url: saveUrl, // Use the passed saveUrl variable
		type: 'POST',
		data: JSON.stringify(jsonData),
		contentType: 'application/json; charset=utf-8',
		dataType: "json",
		success: function (response) {
			if (response["success"]) showSaveIcon();
			else showPopup("Произошла ошибка", 'error');
		},
		error: function (xhr, status, error) {
			showPopup("Произошла ошибка", 'error');
		}
	});
}


// Initialize the document and set up event handlers
$(document).ready(function () {
	$('input, select').on('change', function () {
		hasUnsavedChanges = true;
	});

	// Bind the save function to the Save button's click event
	$('#save').click(function (e) {
		e.preventDefault(); // Prevent the default form submission behavior
		save(); // Call the save function
	});

	// Optionally, set up an interval to call the save function every 5 seconds
	setInterval(save, 15000);
});


// Call the initQuestions function when the page loads
document.addEventListener('DOMContentLoaded', initQuestions);

document.getElementById('add-question').addEventListener('click', addQuestion);

window.addEventListener('beforeunload', function (e) {
	if (hasUnsavedChanges) {
		// Show a warning dialog
		const message = "You have unsaved changes. Are you sure you want to leave?";
		e.returnValue = message; // Required for most browsers
		return message; // Some browsers use this to show the message
	}
});


function showPopup(message, type) {
	const popup = document.getElementById("popup");
	const popupMessage = popup.querySelector(".popup-message");

	// Set the popup text to the provided message
	popupMessage.innerText = message;

	// Apply different styles based on the type of popup
	switch (type) {
		case 'error':
			popup.style.backgroundColor = "#f44336"; // Red for errors
			break;
		case 'notification':
			popup.style.backgroundColor = "#4CAF50"; // Green for notifications
			break;
		case 'warning':
			popup.style.backgroundColor = "#ff9800"; // Orange for warnings
			break;
		default:
			popup.style.backgroundColor = "#333"; // Default color
	}

	// Show the popup
	popup.style.display = "block";

	// Automatically close the popup after 3 seconds (3000 ms)
	setTimeout(function () {
		popup.style.display = "none";
	}, 3000);
}