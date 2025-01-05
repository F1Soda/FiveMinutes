using FiveMinute.Data;

namespace FiveMinute.Application.Helpers
{
	public static class TestStatusHelper
	{
		public static string GetStatusClass(TestStatus status)
		{
			return status switch
			{
				//TestStatus.Planned => "bg-warning",
				TestStatus.Open => "bg-primary",
				//TestStatus.InRechekingProcess => "bg-brown",
				//TestStatus.Completed => "bg-success",
				TestStatus.Closed => "bg-danger",
				_ => "bg-light"
			};
		}

		public static string ConvertEnumToString(TestStatus status)
		{
			return status switch
			{
				//TestStatus.Planned => "Запланирована",
				TestStatus.Open => "Активна",
				//TestStatus.InRechekingProcess => "Требует проверки",
				//TestStatus.Completed => "Завершена",
				TestStatus.Closed => "Закрыта",
				_ => "Неизвестный статус"
			};
		}

	}
}
