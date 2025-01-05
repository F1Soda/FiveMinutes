document.addEventListener('DOMContentLoaded', function () {
	const timerElement = document.getElementById('timer');
	const totalSeconds = duration;
	const form = document.getElementById('autoSubmitForm');

	function formatTime(seconds) {
		const hours = Math.floor(seconds / 3600);
		const minutes = Math.floor((seconds % 3600) / 60);
		const secondsLeft = seconds % 60;

		let formattedTime = '';
		if (hours > 0) {
			formattedTime += `${hours} час${hours % 10 === 1 && hours !== 11 ? '' : (hours % 10 >= 2 && hours % 10 <= 4 && !(hours >= 12 && hours <= 14)) ? 'а' : 'ов'}`;
		}
		if (minutes > 0 || hours > 0) {
			formattedTime += (formattedTime ? ', ' : '') + `${minutes} минут${minutes % 10 === 1 && minutes !== 11 ? 'а' : (minutes % 10 >= 2 && minutes % 10 <= 4 && !(minutes >= 12 && minutes <= 14)) ? 'ы' : ''}`;
		}
		if (secondsLeft >= 0) {
			formattedTime += (formattedTime ? ', ' : '') + `${secondsLeft} секунд${secondsLeft % 10 === 1 && secondsLeft !== 11 ? 'а' : (secondsLeft % 10 >= 2 && secondsLeft % 10 <= 4 && !(secondsLeft >= 12 && secondsLeft <= 14)) ? 'ы' : ''}`;
		}
		return formattedTime;
	}

	let remainingSeconds = totalSeconds;

	function autoSubmitForm() {
		if (form) {
			// Add visual feedback (optional)
			const submitButton = document.getElementById('submit');
			const formData = new FormData(form);

			submitButton.disabled = true;
			submitButton.value = 'Автоматическая отправка...';


			fetch(form.action, {
				method: 'POST',
				body: formData
			})
				.then(response => {
					if (!response.ok) {
						throw new Error(`HTTP error! status: ${response.status}`);
					}
					return response.text(); // Get the HTML from the response
				})
				.then(html => {
					// Replace the current page with the response HTML
					document.documentElement.innerHTML = html;
				})
				.catch(error => {
					console.error('Error:', error);
					alert('Произошла ошибка при отправке данных.');
				});

			//try {
			//	const formData = new FormData(document.getElementById('autoSubmitForm'));

			//	const response = await fetch('/FiveMinuteTest/SendTestResultsAutomatically', {
			//		method: 'POST',
			//		body: formData,
			//	});
			//	// const result = await response.json();
				
			//} catch (error) {
			//	console.error('Fetch error:', error);
			//	alert('Something went wrong!');
			//}

		}
	}

	function updateTimer() {
		if (remainingSeconds >= 0) {
			timerElement.textContent = formatTime(remainingSeconds);
			remainingSeconds -= 1;
		} else {
			clearInterval(timerInterval);
			timerElement.textContent = 'Время истекло';

			// Auto-submit the form when the time is over
			autoSubmitForm();
		}
	}

	// Initialize the timer
	updateTimer();
	const timerInterval = setInterval(updateTimer, 1000);
});

function setQuestionPosition(position, obj) {
	// Установите значение hidden input для текущего выбранного ответа
	var hiddenInput = obj.closest('.card-body').querySelector(`input[name="${obj.name.split('.')[0]}.Position"]`);
	hiddenInput.value = position;
}

function validateForm() {
	const requiredFields = document.querySelectorAll('.required-field');
	let allFilled = true;

	requiredFields.forEach(field => {
		if ((field.type === 'text' && field.value.trim() === '') ||
			((field.type === 'radio' || field.type === 'checkbox') && !document.querySelector(`input[name="${field.name}"]:checked`))) {
			allFilled = false;
		}
	});

	if (!allFilled) {
		document.getElementById('error-message').style.display = 'block';
		return false; // Останавливаем отправку формы
	} else {
		document.getElementById('error-message').style.display = 'none';
		return true;
	}
}