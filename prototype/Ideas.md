# Template system
 
## Macros

The template system can use macros (`{Macro}`) to define variable elements of the template. This could be expanded to allow for user-defined macros which could feasibly use other macros through the use of a recursive function (to a point, the "system" macros, such as getting the applicant name, id and such, would not be modifiable by the user). However, the recursive nature would produce issues that could cause infinite loops if they are not handled correctly.

### Example

With an applicant called "David Davidson" who passed the review process, and the template of `Hello {Name}, You {Result}` would result in the email reading `Hello David Davidson, You passed`