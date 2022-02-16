# Markdown File

[+]1. add drop down menu Administration
[+]1.1. add drop down menu item Administration -> Manage Users
[-]1.2. create view ListUsersl
	[+]	create table row schema {"number", "email", "delete", "edit"}
	[+] create form with submit button to delete user
		[+] use case: try delete not found user - show custom View 
			[+] create /Views/Administration/NotFoundIdentityUser.cshtml
			[+] on click list users display errors when delete user operation failed view
		[-] edit button as anchor element:
			[-] behavior:
			[?] whether to inform user when it links external and internal users
[~]
[-]1.2.2. fix styling of table
