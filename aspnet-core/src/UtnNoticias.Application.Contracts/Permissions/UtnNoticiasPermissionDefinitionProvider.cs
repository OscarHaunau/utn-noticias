using UtnNoticias.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace UtnNoticias.Permissions;

public class UtnNoticiasPermissionDefinitionProvider : PermissionDefinitionProvider
{
	public override void Define(IPermissionDefinitionContext context)
	{
		var group = context.AddGroup(UtnNoticiasPermissions.GroupName);

		var readingLists = group.AddPermission(
			UtnNoticiasPermissions.ReadingLists.Default,
			L("Permission:ReadingLists")
		);

		readingLists.AddChild(
			UtnNoticiasPermissions.ReadingLists.Create,
			L("Permission:ReadingLists.Create")
		);
		readingLists.AddChild(
			UtnNoticiasPermissions.ReadingLists.Update,
			L("Permission:ReadingLists.Update")
		);
		readingLists.AddChild(
			UtnNoticiasPermissions.ReadingLists.Delete,
			L("Permission:ReadingLists.Delete")
		);
		readingLists.AddChild(
			UtnNoticiasPermissions.ReadingLists.AddItem,
			L("Permission:ReadingLists.AddItem")
		);
	}

	private static LocalizableString L(string name)
	{
		return LocalizableString.Create<UtnNoticiasResource>(name);
	}
}
