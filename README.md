# Assessment
Synic Software interview assessment

- Implement the All, CreateUpdate, DeleteById, Get, GetbyID in the service layer.
- Create a HTML table in the view and load the blogs and author data to the table. (You did it during the interview)
- Create pages or modals where users can create/update/delete blogs and authors.(You will need to create a viewmodel for this) 

**Bug list:**
- Add a Blog with existing author ---> Have a same name author but different AuthorID.

**Solution**: 
- Users should only have authority to edit their own blogs(Using user model)
