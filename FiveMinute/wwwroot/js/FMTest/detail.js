console.log('Test is open:', testIsOpen); // Should log true or false
console.log('Test ID:', testId); // Should log the string value of test ID
console.log('Number of questions:', questionCount); // Should log the number of questions


document.addEventListener('DOMContentLoaded', function () {

	// Bind the submit function to the Save button's click event
	$('#submit').click(function (e) {
		verifyDataBeforeSubmit(e); // Call the save function
	});

	initializeQuestionCheckboxes('IdToUninclude');
	toggleEndTimeInput();
	toggleStartTimeInput();

	document.addEventListener('click', function (e) {
		if (e.target.classList.contains('mark-correct-btn')) {
			var questionId = e.target.getAttribute("data-question-id");

			var answerId = e.target.getAttribute('data-answer-id');
			var isCorrect = e.target.getAttribute('data-is-correct') === 'true';

			var testId = e.target.getAttribute("data-test-id");
			var text = e.target.getAttribute("data-answer-text");


			fetch('@Url.Action("UpdateAnswerCorrectness", "FiveMinuteTest")', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify({
					answerId: parseInt(answerId), isCorrect: isCorrect,
					questionId: parseInt(questionId), testId: parseInt(testId),
					text: text
				}),
			})
				.then(response => {
					if (!response.ok) throw new Error('Network response was not ok');
					return response.json().catch(() => null);
				})
				.then(data => {
					alert('Ответ обновлён!');
					var answerBlock = e.target.closest('.answer-block');
					if (answerBlock) {
						answerBlock.style.backgroundColor = isCorrect ? '#d4edda' : '#f8d7da';
					}
				})
				.catch(error => {
					console.error('Fetch error:', error);
					alert('Ошибка при обновлении ответа.');
				});
		}
	});

	let closeButton = document.getElementById('closeTest');

	if (closeButton) {
		closeButton.addEventListener('click', async () => {
			UpdateTestStatus('/FiveMinuteTest/DeactivateTest');
		});
	}


	let openButton = document.getElementById('openTest');

	if (openButton) {
		openButton.addEventListener('click', async () => {
			UpdateTestStatus('/FiveMinuteTest/ActivateTest');
		});
	}
});


async function UpdateTestStatus(input) {
	try {
		const response = await fetch(input, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				'X-Requested-With': 'XMLHttpRequest' // Ensures it’s an AJAX request
			},
			body: JSON.stringify({ Id: testId }) // Adjust if your action requires parameters
		});
		const result = await response.json();
		if (result.success) {
			showPopup("Статус теста обновлен!", "notification");
			testIsOpen = !testIsOpen;

			//buttonCloseTest.className = 'btn btn-danger btn-delete';
			buttonCloseTest.value = 'Закрыть';
			buttonOpenTest.value = 'Открыть';
				
			if (testIsOpen) {
				buttonCloseTest.hidden = false;
				buttonOpenTest.hidden = true;
			} else {
				buttonCloseTest.hidden = true;
				buttonOpenTest.hidden = false;	
			}

			// Update status badge
			const statusBadge = document.querySelector('.badge');
			statusBadge.className = `badge ${result.statusClass} text-white`; // Update CSS class
			statusBadge.textContent = result.statusText; // Update text content

		} else {
			alert(result.exception);
		}
	} catch (error) {
		console.error('Fetch error:', error);
		alert('Something went wrong!');
	}
}

function toggleStartTimeInput() {
	var startPlanned = document.getElementById('startPlanned').checked;
	document.getElementById('startTimeInput').style.display = startPlanned ? 'block' : 'none';
}

function toggleEndTimeInput() {
	var endPlanned = document.getElementById('endPlanned').checked;
	document.getElementById('endTimeInput').style.display = endPlanned ? 'block' : 'none';
}

function verifyDataBeforeSubmit(event) {
	// Get all the checkboxes with the class 'question-checkbox'
	const checkboxes = document.querySelectorAll('.question-checkbox');

	// Filter the checkboxes where checked is false and count them
	const uncheckedCount = Array.from(checkboxes).filter(checkbox => !checkbox.checked).length;


	if (questionCount === uncheckedCount) {
		event.preventDefault();
		showPopup("В тесте должен быть хотя бы один вопрос!", "error");
	}
}