document.addEventListener('DOMContentLoaded', function () {





	// Bind the submit function to the Save button's click event
	$('#submit').click(function (e) {
		verifyDataBeforeSubmit(e); // Call the save function
	});

	initializeQuestionCheckboxes('IdToUninclude');
	toggleEndTimeInput();
	toggleStartTimeInput();

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

	$(document).on('click', '#copyUrlButton', function (e) {
		// Construct the URL dynamically
		let encryptedId = document.getElementById('copyUrlButton').getAttribute('encryptedId');

		let url = `${window.location.origin}/FiveMinuteTest/Pass?encryptedId=${encryptedId}`;

		// Copy the URL to the clipboard
		navigator.clipboard.writeText(url)
			.then(() => {
				showPopup("Ссылка успешно скопирована в буфер обмена!", "notification");
			})
			.catch(err => {
				console.error('Ошибка при копировании ссылки:', err);
				showPopup('Не удалось скопировать ссылку.', "error");
			});
	});
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


function uncheckTimePlaneCheckboxes() {
	uncheckStartPlanned();
	uncheckEndPlanned();
}

function uncheckStartPlanned() {
	document.getElementById('startPlanned').checked = false;
	toggleStartTimeInput();
}

function uncheckStartPlanned() {
	document.getElementById('endPlanned').checked = false;
	toggleEndTimeInput();
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