namespace UtnNoticias.Permissions;

public static class UtnNoticiasPermissions
{
	public const string GroupName = "UtnNoticias";

	public static class ReadingLists
	{
		public const string Default = GroupName + ".ReadingLists";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
		public const string AddItem = Default + ".AddItem";
	}
}
