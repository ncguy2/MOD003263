# MOD003263

A Degree course module project.

As a group, we were tasked with creating a tool to streamline an interview process.

## Functionality
* Connects to external database to store applicants
* Provides search functionality for the applicants within the database
  * Uses a 'Smart Search' on the client, allowing for individual properties to be queried by the user, without cluttering the interface
* Allows for Questions to be created, with a range of options and an additional comment field
* Allows for an Interview to be constructed by allocating a set of questions
* Sends emails (when appropriately configured) to the applicants stating the result of the interview
* Uses a template system for the email contents
* Internal features:
  * Asynchronous database communications
  * Off-main-thread processing for all non-UI related tasks
  * Eventbus system to allow the UI to communicate to the backend in a thread-safe way
