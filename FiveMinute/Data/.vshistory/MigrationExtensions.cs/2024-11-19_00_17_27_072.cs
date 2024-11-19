namespace FiveMinute.Data
{
	public static class MigrationExtensions
	{

		public static void ApplyMigrations(this IApplicationBuilder app)
		{
			using IServiceScope scope = app.ApplicationServices.CreateScope();
		}
	}
}
