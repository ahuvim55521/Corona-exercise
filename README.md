# Corona-exercise
Corona exercise
התשובה לתרגיל 2-ב נמצאת בקובץ "תרגיל 2ב"

השתמשתי ב

                                                                                                                                                 entity framework code first

בפתיחת הפרויקט יש ליצור את הDB

פותחים את
                                                                                                                                      package manager console ב visual studio
                                                                                                                                      
שם נכתוב

                                                                                                                                                Add-Migration firstMigration
                                                                                                                                                             Update-Database
                                                                                                                                                             
השתמשתי בswagger - openAPI

כדי ליצור קריאות לשרת בקלות.

בהרצת הפרויקט - נפתח דף של swagger עם אפשרות ליצור קריאות לAPI

יצרתי ארבעה controllers

מבוטחים- InsuredsController

בGET - מתקבל רשימת מבוטחים עם פרטי חיסונים והדבקות (ברשימה או לפי ID).

בPOST - אפשר ליצור מבוטח עם רשימה של חיסונים והדבקות.

בPOST יש בדיקות תקינות שונות, לדוגמה:

בUploadFile -ניתן להעלות תמונה לשרת,

השרת שומר את התמונה במחשב ושומר את הנתיב של התמונה בDB בטבלת Insured. .

בGetFile - מחזיר את התמונה לפי הID של המבוטח

בDELETE מוחק את הרשומה לפי ID

חיסונים- ImmunizationController

                                                                                                                                              get, post-כולל בדיקות תקינות, delet

מחלה- DiseaseController
                                                                                                                                              
                                                                                                                                              get, post-כולל בדיקות תקינות, delet

יצרנים- ManufacturerController

יצרתי את הקונטרולר הזה כדי לאפשר לעבוד על הטבלה של היצרנים דרך הסרבר, חייבים למלא אותה - כדי לאפשר ליצור חיסונים למבוטחים עם FK לטבלה זו
.


