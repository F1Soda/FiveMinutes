document.addEventListener('DOMContentLoaded', () => {
    // Target the form and the submit button
    const form = document.getElementById('edit-user-form');

    // Listen for the form submission event
    form.addEventListener('submit', async (event) => {
        event.preventDefault(); // Prevent the default form submission

        // Verify data before submitting
        if (!verifyDataBeforeSubmit()) {
            showPopup("Введите корректные данные.", "error")
            return;
        }

        // Collect form data
        const formData = new FormData(form);

        // Convert FormData to a plain object
        const data = Object.fromEntries(formData.entries());

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest', // Indicates it's an AJAX request
                },
                body: JSON.stringify(data), // Send JSON data to the server
            });
            const result = await response.json();
            if (result.success) {
                // Show success notification
                showPopup("Данные обновлены!", "notification");
            } else {
                // Handle server-side validation errors or other issues
                const error = await response.text();
                console.error('Server error:', error);
                showPopup("Возникла ошибка. Попробуйте позже.", "error");
            }
        } catch (error) {
            console.error('Fetch error:', error);
            showPopup("Возникла ошибка. Попробуйте позже.", "error");
        }
    });
});

function verifyDataBeforeSubmit() {
    const firstName = document.getElementById('FirstName').value.trim();
    const lastName = document.getElementById('LastName').value.trim();
    const email = document.getElementById('Email').value.trim();
    const group = document.getElementById('Group')?.value.trim(); // Optional field

    let isValid = true;
    let errorMessage = '';

    // Validate First Name
    if (!firstName) {
        errorMessage += 'Имя обязательно.\n';
        isValid = false;
    } else if (firstName.length > 50) {
        errorMessage += 'Имя должно быть не более 50 символов.\n';
        isValid = false;
    }

    // Validate Last Name
    if (!lastName) {
        errorMessage += 'Фамилия обязательна.\n';
        isValid = false;
    } else if (lastName.length > 50) {
        errorMessage += 'Фамилия должна быть не более 50 символов.\n';
        isValid = false;
    }

    // Validate Email
    if (!email) {
        errorMessage += 'Электронная почта обязательна.\n';
        isValid = false;
    } else if (!validateEmail(email)) {
        errorMessage += 'Введите действительный адрес электронной почты.\n';
        isValid = false;
    }

    // Validate Group (Optional)
    if (group && group.length > 20) {
        errorMessage += 'Название группы должно быть не более 20 символов.\n';
        isValid = false;
    }

    //if (!isValid) {
    //    alert(errorMessage);
    //}

    return isValid;
}

// Utility function to validate email format
function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}
