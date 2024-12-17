# Tasks and notes

## Tasks
- [x] Use WPF UI framework
- [x] Refactor the code to use MVVM pattern
- [ ] Re-write the Login window using WPF UI framework
- [ ] Create the necessary pages (e.g. Dashboard, Settings, Students, Majors, etc.) using WPF UI framework
	- [ ] Dashboard
	- [ ] Settings
	- [x] Students
	- [x] Majors
	- [ ] Statistics
- [ ] Implement the necessary functionalities for each page
	- [ ] Dashboard
	- [ ] Settings
	- [x] Students
	- [x] Majors
	- [ ] Statistics
- [ ] Implement the backend functionalities
	- [ ] Database connection
	- [ ] CRUD operations

## Notes (for backend functionalities)
- Database connection: We are using MySQL database for this project. The connection string is stored in the `appsettings.json` file. Here is a template for the connection string:
```json
{
    "ConnectionStrings": {
        "MySqlConnection": "..."
    }
}
```
- The `Student` and `Major` models are defined in the `Models` folder. These models will be used to interact with the database.
- The `StudentViewModel` and `MajorViewModel` classes are defined in the `ViewModels` folder. These classes will be used to bind the data to the UI. They contain observable collections for data binding purposes. Replace the code to fill these collections with actual data from the database.
- Implement the CRUD operations for `Student` and `Major` models. These operations will be used to interact with the database.
- Implement filtering functionality for the students page. The user should be able to filter students by name and major. The majors combo box is already populated with data from the observable collection. It also contains an "All" option to show all students regardless of major. It is not yet functional. Implement the filtering logic in the `StudentsViewModel` class.
- Implement the add, edit, and delete functionalities for students and majors. These functionalities should interact with the database to perform the necessary operations.
	- The add functionality already opens a dialog window to add a new student or major and adds it to the observable collection. Implement the logic to add the data to the database as well.
	- The students data grid contains a column with checkboxes to select students for deletion. Implement the delete functionality to remove the selected students from the database. The logic is in the `DeleteButton_Click` event handler in the `StudentsPage.xaml.cs` file.
	- Editing students is as simple as double-clicking on a cell in the data grid. This already edits the data in the observable collection. Implement the logic to update the data in the database as well.
- Implement the backend functionalities for the remaining pages (e.g. Dashboard, Settings, Statistics). These functionalities will interact with the database to retrieve and update data as needed.