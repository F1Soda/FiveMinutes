function initializeQuestionCheckboxes(hiddenInputId) {
    const checkboxes = document.querySelectorAll('.question-checkbox');
    const hiddenInput = document.getElementById(hiddenInputId);

    // Функция обновления скрытого поля с ID исключенных вопросов
    function updateExcludedIds() {
        const uncheckedIds = [];
        console.log('zZZZ');
        checkboxes.forEach(chk => {
            if (!chk.checked) {
                uncheckedIds.push(chk.value);
            }
        });
        console.log(uncheckedIds);
        hiddenInput.value = uncheckedIds.join(',');
    }

    // Добавляем обработчик для каждого чекбокса
    checkboxes.forEach((checkbox, index) => {
        checkbox.addEventListener('change', updateExcludedIds);
    });
    updateExcludedIds();
}