
let timer = document.getElementById("timer");
let isoEndTime = document.getElementById("isoTime").innerHTML;
let endTime = new Date(isoEndTime);
    // console.log(timer, isoEndTime, endTime)
updateRemainingTime();
setInterval(updateRemainingTime, 1000);

// Функция для обновления оставшегося времени
function updateRemainingTime() {
    const now = new Date(); // Текущее время
    //const remainingTime = targetDate - now; // Разница в миллисекундах
    // Если время вышло, выводим сообщение
    //if (remainingTime < 0) {
    //	console.log("Время истекло!");
    //	clearInterval(timer); // Останавливаем таймер
    //	return;
    //}

    // Вычисляем оставшиеся компоненты времени
    let years = endTime.getUTCFullYear()- now.getUTCFullYear();
    let months = endTime.getUTCMonth() -  now.getUTCMonth();
    let days = endTime.getUTCDate() - now.getUTCDate();
    let hours = endTime.getUTCHours() - now.getUTCHours();
    let minutes = endTime.getUTCMinutes() - now.getUTCMinutes();
    let seconds = endTime.getUTCSeconds() - now.getUTCSeconds();

    // Корректируем значения, если они отрицательные
    if (seconds < 0) {
        seconds += 60;
        minutes--;
    }
    if (minutes < 0) {
        minutes += 60;
        hours--;
    }
    if (hours < 0) {
        hours += 24;
        days--;
    }
    if (days < 0) {
        const lastMonth = new Date(now.getFullYear(), now.getMonth(), 0); // Предыдущий месяц
        days += lastMonth.getDate();
        months--;
    }
    if (months < 0) {
        months += 12;
        years--;
    }

    // Создаем массив для хранения оставшегося времени
    const timeComponents = [];
    if (years > 0) timeComponents.push(`${years} лет`);
    if (months > 0) timeComponents.push(`${months} месяцев`);
    if (days > 0) timeComponents.push(`${days} дней`);
    if (hours > 0) timeComponents.push(`${hours} часов`);
    if (minutes > 0) timeComponents.push(`${minutes} минут`);
    if (seconds > 0) timeComponents.push(`${seconds} секунд`);

    // Объединяем компоненты в строку
    const formattedTime = timeComponents.join(', ') || '0 секунд';

    timer.textContent = formattedTime;
}