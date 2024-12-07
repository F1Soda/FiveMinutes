document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".confirm-delete-button");

    deleteButtons.forEach(function (button) {
        button.addEventListener("click", function (event) {
            event.preventDefault();

            const deleteUrl = button.getAttribute("data-delete-url");
            const itemId = button.getAttribute("data-id");
            const userConfirmed = confirm("Вы действительно хотите удалить этот элемент?");

            if (userConfirmed) {
                fetch(deleteUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ Id: parseInt(itemId) })
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            // Обновляем таблицы, если они есть в ответе
                            if (data.templatesHtml) {
                                document.getElementById("FMTemplateTable").innerHTML = data.templatesHtml;
                            }
                            if (data.testsHtml) {
                                document.getElementById("FMTestTable").innerHTML = data.testsHtml;
                            }

                            // Повторно назначим обработчики для новых кнопок удаления
                            const newDeleteButtons = document.querySelectorAll(".confirm-delete-button");
                            newDeleteButtons.forEach(btn => {
                                if (!btn.hasAttribute("data-initialized")) {
                                    btn.setAttribute("data-initialized", "true");
                                    btn.addEventListener("click", event.currentTarget.onclick);
                                }
                            });
                        } else {
                            alert(data.reason || "Не удалось удалить элемент.");
                        }
                    })
                    .catch(error => console.error('Error:', error));
            }
        });
    });
});
