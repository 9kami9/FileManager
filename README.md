# FileManager
 WPF application (Test task for Webtronics)


## Initial setup
No setup is needed :)

## Database
SQLite is used as the default database.
You can find it at the following localtion:
```"~FileManager\bin\Debug\net6.0-windows\Database.db"```

## Implemented functionalities:
### 1. Working area with drives, files and folders
### 2. Double clicking on an element
- If it is a file, then the application tries to open using windows;
- If it is a folder, then the working area is filled with the contents of this folder
### 3. Single clicking on an element
- File info is displayed in the side section
### 4. Upon opening a file, a record is added to the Database
### 5. Implemented a path in the folder hierarchy at the top of the work area
- You can navigate to a path by writing a custom path in the text box
### 6. Implemented a search function by name
- Upon clicking on the search button the working area is filtered using the text from the search box. It's case insensitive.
### 7. Implemented the back button
### 8. Viewing file visit history
- Upon clicking on the ```file visit history``` button, a new window is opened containing database records